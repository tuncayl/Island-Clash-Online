using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using Keys;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.controllers.Island
{
    public class IslandSpawnController : NetworkBehaviour, ISpawn
    {
        #region SelfVariables

        [Header("SpawnRoot")] [SerializeField] private Transform SpawnRoot;


        private Queue<int> SpawnQue = new Queue<int>();

        #endregion

        #region UnityMethods

      
        #endregion

        #region OtherMethods

        public void INIT()
        {
            for (int i = 0; i < SpawnRoot.childCount; i++) SpawnQue.Enqueue(i);
        }


        [ServerRpc(RequireOwnership = false)]
        private void OnPlayerSpawn(Player _player)
        {
            Debug.Log("SPAWNED "+_player.Data.name);
            _player.transform.position = GetSpawnPosition();
            _player._PlayerPhsicsController.ObserverPosition(_player.transform.position);
            _player._PlayerHealthController.TakeHeal(_player.Data.HealtData.MaxHealth);
            _player._PlayerHealthController.ServerBirth();
            _player._PlayerHealthController.ObserverBirth();
        }

        
        #endregion


        #region ServerMethods

        public override void OnStartClient()
        {
            base.OnStartClient();
            Subscire();
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            UnSubscire();
        }

        [ServerRpc(RequireOwnership = false)]
        private void OnStartSpawnCounter(NetworkConnection con)
        {
            Debug.Log("SPAWNED SYNC TIMER");
            GameObject spawnobject=Instantiate(Resources.Load("SyncSpawnTimer", typeof(GameObject))) as GameObject;
            
            if (spawnobject.TryGetComponent<ISetCounter>(out ISetCounter counter))counter.SetCounter(6);
           
            Spawn(spawnobject,con);
        }

        
        [ServerRpc(RequireOwnership = false)]
        public void SpawnChibi(SpawnArgs args)
        {
            Debug.Log(args.roomid+"ROOMID ARGS");
            GameObject spawnobject =
                Instantiate(Resources.Load("Chibi", typeof(GameObject)), args.Parent) as GameObject;
            if (spawnobject.TryGetComponent<IPlayer>(out IPlayer playerSet))
            {
                int index = SpawnQue.Peek();
                playerSet.SetPlayer(new PlayerSetArgs(GetSpawnPosition(), index,args.roomid));
            }
            Spawn(spawnobject, args.Connection);
        }
        
        
        [ServerRpc(RequireOwnership = false)]
        public void DeSpawnChibi(NetworkObject networkObject)
        {
            base.Despawn(networkObject);
        }


        [Server]
        public void SpawnGameCounter(int timer,int roomid)
        {
            Debug.Log("SPAWNED SYNC GAME COUNTER");
            GameObject spawnobject=Instantiate(Resources.Load("SyncGameTimer", typeof(GameObject)),this.transform) as GameObject;
            
            if (spawnobject.TryGetComponent<ISetCounter>(out ISetCounter counter))counter.SetCounter(timer,roomid);
           
            Spawn(spawnobject);
        }
        
        #endregion

        #region ResuableMethods

        private Vector3 GetSpawnPosition()
        {
            int index = SpawnQue.Dequeue();
            SpawnQue.Enqueue(index);
            return SpawnRoot.GetChild(index).position;
        }

        #endregion

        #region SubscireMethods

        public void Subscire()
        {
            if(IsServer)return;
            
             IslandSignals.Instance.OnSpawnPosition += OnPlayerSpawn;
             IslandSignals.Instance.OnStartSpawnCounter += OnStartSpawnCounter;
            
        }

        private void UnSubscire()
        {
            if(IsServer)return;

             IslandSignals.Instance.OnStartSpawnCounter -= OnStartSpawnCounter;
             IslandSignals.Instance.OnSpawnPosition -= OnPlayerSpawn;

        }

        #endregion
    }
}