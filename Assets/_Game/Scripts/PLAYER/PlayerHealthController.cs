using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using onlinetutorial.data;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace onlinetutorial.controllers
{
    public class PlayerHealthController : NetworkBehaviour, IHealth
    {
        #region SelfVariables

        [field:SerializeField]private Player _Player { get; set; }

        private ResuableHealtData resuableData;

        #endregion

        #region ServerVariables

        [SyncVar(Channel = Channel.Reliable, OnChange = nameof(OnHealth))]
        private float _health;

        #endregion

        #region unityMethods

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            UnSubsicreEvent();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            SubscireEvent();
        }

        private void Awake()
        {
           
            resuableData = new ResuableHealtData(_Player.Data.HealtData.MaxHealth);
            _health = _Player.Data.HealtData.MaxHealth;
        }

        [ContextMenu("damage")]
        private void ServerDamage()
        {
            TakeDamage(50);
        }

        #endregion

        #region IhealthMethods

        [Server]
        public void TakeDamage(float damage = 100)
        {
            if (_health <= 0) return;
            _health -= damage;
        }

        [Server]
        public void TakeHeal(float Heal = 50)
        {
            _health += Heal;
        }

        public float GetHealth() => resuableData.Health;

        #endregion


        #region OtherMethods

        private void OnPlayerDead()=> IslandSignals.Instance.OnStartSpawnCounter.Invoke(this.Owner);
  


        #endregion


        #region ServerMethods

        [ObserversRpc(BufferLast = true)]
        public void ObserverBirth() => _Player._PlayerLocalSignals.OnBirth.Invoke();

        [Server]
        public void ServerBirth() => _Player._PlayerLocalSignals.OnBirth.Invoke();
        private void OnHealth(float prev, float next, bool asServer)
        {
            Debug.Log(next + "NEXT HEALTH");
            resuableData.Health = next;


            if (!asServer)
                _Player._PlayerLocalSignals.OnChangeHealth.Invoke(
                    (resuableData.Health / _Player.Data.HealtData.MaxHealth));

            if (next <= 0 is false) return;
            _Player._PlayerLocalSignals.OnDead.Invoke();
        }

        #endregion

        #region SubscireEvent

        private void SubscireEvent()
        {
            
            if(IsOwner is false) return;
            _Player._PlayerLocalSignals.OnDead += OnPlayerDead;
        }

        private void UnSubsicreEvent()
        {
            if(IsOwner is false) return;

            _Player._PlayerLocalSignals.OnDead -= OnPlayerDead;

        }

        #endregion
    }
}