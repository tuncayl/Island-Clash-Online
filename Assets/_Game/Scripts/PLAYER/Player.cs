using System;
using FishNet.Component.Animating;
using FishNet.Component.ColliderRollback;
using FishNet.Component.Observing;
using FishNet.Connection;
using FishNet.Object;
using onlinetutorial.data;
using onlinetutorial.Enums;
using UnityEngine;
using onlinetutorial.interfaces;
using onlinetutorial.interfaces.PlayerAttack;
using onlinetutorial.Signals;
using UnityEngine.UI;

namespace onlinetutorial.controllers
{
    public class Player : NetworkBehaviour, IPlayer
    {
        #region SelfVariables

        private int roomid=-1;

        [field: Header("References")]
        [field: SerializeField]
        public PlayerSO Data { get; private set; }

        [field: SerializeField] public AnimationData _Animatiodata { get; private set; }

        public ResuableGeneralData ResuableGeneralData;

        [field: Header("Points")]
        [field: SerializeField] public Transform AttackPoint { get; private set; }

        [field: SerializeField] public Transform CameraPoint { get; private set; }

        [field: SerializeField] public Transform GroundPoint { get; private set; }


        public Rigidbody Rigidbody { get; private set; }

        public Animator Animator { get; private set; }

        public PlayerInput Input { get; private set; }

        public PlayerAnimationEvents AnimationEvents { get; private set; }

        public PlayerLocalSignals _PlayerLocalSignals { get; private set; }

        [field:SerializeField] public PlayerHealthController _PlayerHealthController { get; private set; }

        [field:SerializeField]public PlayerPhysicsController _PlayerPhsicsController { get; private set; }

        [field:SerializeField]public PlayerAttackController _playerAttackController { get; private set; }

        [field:SerializeField]public PlayerColorController _PlayerColorController { get; private set; }


        public MovementStateMachine _movementStateMachine { get; set; }

        public AttackStateMachine _attackStateMachine { get; set; }

        #endregion


        #region UnityMethods

        private void Awake()
        {

            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();
            Input = GetComponent<PlayerInput>();
            _playerAttackController = GetComponent<PlayerAttackController>();
            AnimationEvents = GetComponent<PlayerAnimationEvents>();
            _Animatiodata = new AnimationData();
            _PlayerLocalSignals = new PlayerLocalSignals();
            ResuableGeneralData = new ResuableGeneralData((chibicolor)0);
        }

        #endregion

        #region ServerMethods

        public override void OnStartServer()
        {
            base.OnStartServer();
            Subscire();
            GetComponent<ColliderRollback>().AddRollBackServer(roomid);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Subscire();
            if (IsOwner is false) return;
            OnSetGeneralData();
            INIT();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            if (IsOwner is false) return;
            Unsubscire();
        }


        public override void OnStopServer()
        {
            base.OnStopServer();
            GetComponent<ColliderRollback>()
                .RemoveRollBackServer(roomid);
        }


        [ServerRpc(RequireOwnership = true)]
        private void OnSetGeneralData() => OnSetGeneralData(ResuableGeneralData);

        [ObserversRpc(BufferLast = true)]
        private void OnSetGeneralData(ResuableGeneralData _resuableGeneralData)
        {
            ResuableGeneralData = _resuableGeneralData;
            if (IsOwner is false) return;
            _PlayerColorController.INIT();
        }


        [Server]
        private void ServerOnGameEnd() => ObserverOnGameEnd();
 

        [ObserversRpc]
        private void ObserverOnGameEnd()
        {
            if (IsOwner) CoreGameSignals.Instance.OnGameEnd.Invoke();
            _PlayerLocalSignals.OnGameEnd.Invoke();
        }


        [ServerRpc(RequireOwnership = false)]
        private void OnMenuSceneLoading() => Despawn(this.gameObject);

        #endregion

        #region OtherMethods

        private void INIT()
        {
            Input.enabled = true;
            CameraSignals.Instance.OnSetVmCamera?.Invoke(CameraPoint);
            SceneSignals.Instance.OnMainSceneloaded?.Invoke();
        }

        #endregion

        #region InterfaceMethods

        public void SetPlayer(PlayerSetArgs args)
        {
            transform.position = args.Startpositon;
            ResuableGeneralData = new ResuableGeneralData((chibicolor)args.Order);
            roomid = args.roomid;
        }

        public Player GetPlayer() => this;

        #endregion

        #region SubscireMethods

        private void Subscire()
        {
            if (IsServer)
            {
                _PlayerLocalSignals.OnGameEnd += ServerOnGameEnd;
            }

            if (IsOwner is false) return;
            SceneSignals.Instance.OnMenuSceneLoading += OnMenuSceneLoading;
            PlayerSignal.Instance.OnGetPlayer += GetPlayer;
        }

        private void Unsubscire()
        {
            if (IsServer)
            {
                _PlayerLocalSignals.OnGameEnd -= ServerOnGameEnd;
            }

            if (IsOwner is false) return;
            SceneSignals.Instance.OnMenuSceneLoading -= OnMenuSceneLoading;
            PlayerSignal.Instance.OnGetPlayer -= GetPlayer;
        }

        #endregion
    }
}