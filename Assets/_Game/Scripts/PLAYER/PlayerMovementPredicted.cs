using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Serializing;
using FishNet.Transporting;
using onlinetutorial.controllers;
using onlinetutorial.interfaces;
using onlinetutorial.interfaces.PlayerAttack;
using onlinetutorial.Signals;
using Unity.Mathematics;
using UnityEngine;
using IReconcileData = FishNet.Object.Prediction.IReconcileData;
using IReplicateData = FishNet.Object.Prediction.IReplicateData;

public struct InputData : IReplicateData
{
    public Vector2 move;
    public float camangle;
    public bool jump;
    public bool Attack;
    public int sprint;


    public InputData(Vector2 _move, float _camangle, bool _jump, bool _attack, int _sprint)
    {
        move = _move;
        Attack = _attack;
        camangle = _camangle;
        jump = _jump;
        sprint = _sprint;
        _tick = 0;
    }

    // // /// ////////////////////////
    private uint _tick;

    public void Dispose()
    {
    }

    public uint GetTick() => _tick;
    public void SetTick(uint value) => _tick = value;
}

public struct ReconcileData : IReconcileData
{
    public Vector3 Position;
    public float RotationY;
    public Vector3 Velocity;
    public Vector3 AngularVelocity;

    public ReconcileData(Vector3 position, float rotationY, Vector3 velocity, Vector3 angularVelocity)
    {
        Position = position;
        Velocity = velocity;
        AngularVelocity = angularVelocity;
        RotationY = rotationY;
        _tick = 0;
    }

    // /// ////////////////////////
    private uint _tick;

    public void Dispose()
    {
    }

    public uint GetTick() => _tick;
    public void SetTick(uint value) => _tick = value;
}

public class PlayerMovementPredicted : NetworkBehaviour
{
    #region SelfVariables

    private PlayerInput _input;

    private Player _player;

    private InputData _lastInput;


    private CachedStateInfo? _cachedStateInfo;

    private Writer _writer = new Writer();


    private bool _subscribed = false;

    private bool isInitialised = false;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        ChangeSubscriptions(true);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        UnSubscire();
        isInitialised = false;
    }

    private void OnDestroy()
    {
        ChangeSubscriptions(false);
        isInitialised = false;
    }

    #endregion

    #region Predicted

    [Replicate]
    private void Move(InputData ınputData, bool asServer, Channel channel = Channel.Unreliable, bool replaying = false)
    {
        if (asServer)
        {
            _lastInput = ınputData;
        }

        SimulateMove(ref ınputData, (float)base.TimeManager.TickDelta);
    }

    [Reconcile]
    private void Reconcile(ReconcileData recData, bool asServer, Channel channel = Channel.Unreliable)
    {
        transform.position = recData.Position;
        transform.eulerAngles = new Vector3(0, recData.RotationY, 0);
        _player.Rigidbody.velocity = recData.Velocity;
        _player.Rigidbody.angularVelocity = recData.AngularVelocity;
    }

    #endregion

    #region ServerMethods

    public override void OnStartServer()
    {
        base.OnStartServer();
        _player.Animator.enabled = true;

        Subscire();

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        _player.Animator.enabled = true;
        Subscire();

        if (IsOwner is false) return;
    }


    public override void OnStopNetwork()
    {
        base.OnStopNetwork();

        ChangeSubscriptions(false);
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        _player._Animatiodata.Initialize();
        _player._movementStateMachine = new MovementStateMachine(_player);
        _player._attackStateMachine = new AttackStateMachine(_player);
        _player._movementStateMachine.ChangeState(_player._movementStateMachine.FallingState);
        _player._attackStateMachine.ChangeState(_player._attackStateMachine.AttackIdle);
        isInitialised = true;
    }

    private void TimeManager_OnTick()
    {
        if (isInitialised is false) return;

        if (base.IsOwner)
        {
            Reconcile(default, false);
            _input.CheckInput(out InputData md);
            Move(md, false);
        }

        if (base.IsServer)
        {
            Move(default, true);
        }

        if (!base.IsOwner && !base.IsServer)
        {
            SimulateMove(ref _lastInput, (float)base.TimeManager.TickDelta);
        }
    }


    private void TimeManager_OnPostTick()
    {
        if (base.IsServer)
        {
            SendChibiToObservers();


            ReconcileData rd =
                new ReconcileData(transform.position, _player.transform.eulerAngles.y,
                    _player.Rigidbody.velocity, _player.Rigidbody.angularVelocity);
            Reconcile(rd, true);
        }
    }


    private void PredictionManager_OnPreReplicateReplay(uint arg0, PhysicsScene arg1, PhysicsScene2D arg2)
    {
        if (isInitialised is false) return;


       
    }

    private void PredictionManager_OnPostReplicateReplay(uint arg0, PhysicsScene arg1, PhysicsScene2D arg2)
    {
        if (isInitialised is false) return;
         if (!base.IsOwner && !base.IsServer)
        {
           _player._movementStateMachine.Input(_lastInput);
           _player._attackStateMachine.Input(_lastInput);

        }
    }

    private void TimeManager_PhysSim(float delta)
    {
        if (isInitialised is false) return;
        if (IsOwner || IsServer) _player._movementStateMachine.PhysicsUpdate(delta);
    }

    private void TimeManager_OnPreReconcile(NetworkBehaviour obj)
    {
        if (isInitialised is false) return;
        if (!base.IsOwner && !base.IsServer)
        {
            _lastInput = default;
        
            if (_cachedStateInfo.HasValue)
            {
                var reader = new Reader(_cachedStateInfo.Value.Data, base.NetworkManager);
                _lastInput = _cachedStateInfo.Value.Input;
            }
        }
    }

    #endregion

    #region OtherMethods

    private void SendChibiToObservers()
    {
        _writer.Reset();

        var stateData = new byte[_writer.Length];
        Array.Copy(_writer.GetBuffer(), stateData, _writer.Length);
        ObserversSendChibiState(_lastInput, stateData);
    }

    [ObserversRpc(ExcludeOwner = false, BufferLast = true)]
    private void ObserversSendChibiState(InputData lastInput, byte[] stateData, Channel channel = Channel.Unreliable)
    {
        if (!base.IsServer && !base.IsOwner)
        {
            _cachedStateInfo = new CachedStateInfo { Input = lastInput, Data = stateData };
        }
    }

    private void SimulateMove(ref InputData md, float deltatime)
    {
        
        _player._movementStateMachine.Input(md);
        _player._attackStateMachine.Input(md);
        _player._movementStateMachine.Update(deltatime);
        _player._attackStateMachine.Update(deltatime);
    }

    private void OnGameEnd()
    {
        isInitialised = false;
       
        if (!IsServer)
        {
            _player.Animator.SetFloat("speed",0);
            _player.Animator.SetTrigger(_player._Animatiodata.IdleParameterHash);
        }

        
      
    }

    #endregion

    #region SubscireMethods

    private void ChangeSubscriptions(bool subscribe)
    {
        if (base.TimeManager == null)
            return;
        if (subscribe == _subscribed)
            return;

        _subscribed = subscribe;

        if (subscribe)
        {
            base.TimeManager.OnTick += TimeManager_OnTick;
            base.TimeManager.OnPostTick += TimeManager_OnPostTick;
            base.TimeManager.OnPrePhysicsSimulation += TimeManager_PhysSim;
            base.PredictionManager.OnPreReconcile += TimeManager_OnPreReconcile;
            base.PredictionManager.OnPreReplicateReplay += PredictionManager_OnPreReplicateReplay;
            base.PredictionManager.OnPostReplicateReplay += PredictionManager_OnPostReplicateReplay;
        }
        else
        {
            base.TimeManager.OnTick -= TimeManager_OnTick;
            base.TimeManager.OnPostTick -= TimeManager_OnPostTick;
            base.TimeManager.OnPrePhysicsSimulation -= TimeManager_PhysSim;
            base.PredictionManager.OnPreReconcile -= TimeManager_OnPreReconcile;
            base.PredictionManager.OnPreReplicateReplay -= PredictionManager_OnPreReplicateReplay;
            base.PredictionManager.OnPostReplicateReplay -= PredictionManager_OnPostReplicateReplay;
        }
    }

    private void Subscire()
    {
        _player._PlayerLocalSignals.OnGameEnd += OnGameEnd;

        if (IsOwner is false) return;
        CoreGameSignals.Instance.OnGameEnd += OnGameEnd;
    }

    private void UnSubscire()
    {
        _player._PlayerLocalSignals.OnGameEnd -= OnGameEnd;

        if (IsOwner is false) return;
        CoreGameSignals.Instance.OnGameEnd -= OnGameEnd;
    }

    #endregion

    public struct CachedStateInfo
    {
        public InputData Input;
        public byte[] Data;
    }
}