using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace onlinetutorial.controllers
{
    public class PlayerInput : MonoBehaviour
    {
        private Player _player { get; set; }
        public PlayerInputActions InputActions { get; private set; }
        public PlayerInputActions.PlayerActions PlayerActions { get; private set; }

        private bool inputAcces = true;

        private bool jupmqueue = false;

        private bool attackqeue = false;

        private int sprint = 1;


        private bool testLag = false;

        private Vector2 testmovevector = Vector2.left;

        private void Awake()
        {
            InputActions = new PlayerInputActions();
            _player = GetComponent<Player>();
            PlayerActions = InputActions.Player;
        }

        private void OnEnable()
        {
            InputActions.Enable();
            GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
            GetComponent<PlayerAnimationEvents>().enabled = true;

        }

        private void OnDisable()
        {
            InputActions.Disable();
            UnSubscire();
        }

        private void Start()
        {
            Subscire();
        }

        [ContextMenu("TestLagCompensation")]
        private void TesLagCompensation()
        {
            testLag = true;
            StartCoroutine(ITesLagCompensation());
        }

        IEnumerator ITesLagCompensation()
        {
            testmovevector = testmovevector == Vector2.left ? Vector2.right : Vector2.left;

            yield return new WaitForSeconds(3f);

            StartCoroutine(ITesLagCompensation());
        }

        public void CheckInput(out InputData md)
        {
            if (inputAcces is false)
            {
                md = default;
                return;
            }

            Vector2 movevec = testLag is false ? PlayerActions.Move.ReadValue<Vector2>() : testmovevector;
            float camangle = CameraSignals.Instance.OnGetCamera.Invoke().transform.eulerAngles.y;
            md = new InputData(movevec, camangle, jupmqueue, attackqeue, sprint);
            jupmqueue = false;
            attackqeue = false;
        }


        private void OnPlayerDead() => inputAcces = false;

        private void OnPlayerSpawned() => inputAcces = true;

        private void OnJump(InputAction.CallbackContext context)
        {
            if (jupmqueue) return;
            jupmqueue = true;
        }

        private void OnAttackPerformed(InputAction.CallbackContext context)
        {
            attackqeue = true;
        }

        private void OnRunPerformed(InputAction.CallbackContext context)
        {
            sprint = (int)context.ReadValue<float>() + 1;
        }

        private void OnRunCancelled(InputAction.CallbackContext context)
        {
            sprint = 1;
        }

        #region SubscireMethods

        private void Subscire()
        {
            if (InstanceFinder.IsClient is false) return;
            PlayerSignal.Instance.OnPlayerSpawned += OnPlayerSpawned;
            CoreGameSignals.Instance.OnGameEnd += OnPlayerDead;
            _player._PlayerLocalSignals.OnDead += OnPlayerDead;

            PlayerActions.Jump.started += OnJump;
            PlayerActions.Run.performed += OnRunPerformed;
            PlayerActions.Run.canceled += OnRunCancelled;
            PlayerActions.Fire.started += OnAttackPerformed;
        }

        private void UnSubscire()
        {
            if (InstanceFinder.IsClient is false) return;
            PlayerSignal.Instance.OnPlayerSpawned -= OnPlayerSpawned;
            CoreGameSignals.Instance.OnGameEnd -= OnPlayerDead;
            _player._PlayerLocalSignals.OnDead -= OnPlayerDead;

            PlayerActions.Jump.started -= OnJump;
            PlayerActions.Run.performed -= OnRunPerformed;
            PlayerActions.Run.canceled -= OnRunCancelled;
            PlayerActions.Fire.started -= OnAttackPerformed;
        }

        #endregion
    }
}