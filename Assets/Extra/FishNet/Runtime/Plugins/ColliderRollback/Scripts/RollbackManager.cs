using FishNet.Managing;
using FishNet.Managing.Timing;
using FishNet.Transporting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FishNet.Component.ColliderRollback
{
    public class RollbackManager : MonoBehaviour
    {
        #region Serialized.

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("Maximum time in the past colliders can be rolled back to.")] [SerializeField]
        private float _maximumRollbackTime = 1.25f;

        /// <summary>
        /// Maximum time in the past colliders can be rolled back to.
        /// </summary>
        internal float MaximumRollbackTime => _maximumRollbackTime;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("Interpolation value for the NetworkTransform or object being rolled back.")]
        [Range(0, 250)]
        [SerializeField]
        internal ushort Interpolation = 2;

        #endregion

        #region SelfVariables

        private Dictionary<int, List<ColliderRollback>>
            ColliderRollbacks = new Dictionary<int, List<ColliderRollback>>();

        #endregion


        /// <summary>
        /// Initializes this script for use.
        /// </summary>
        /// <param name="manager"></param>
        internal void InitializeOnce_Internal(NetworkManager manager)
        {
        }

        #region RollBackMethods

    

        public void Rollback(PreciseTick pt, int playerid, int roomid)
        {
            Debug.Log("server tick" + InstanceFinder.TimeManager.Tick);
            Debug.Log("Percent tick =>" + pt.Tick);
            if (ColliderRollbacks.Count == 0) return;
            if (ColliderRollbacks.ContainsKey(roomid) is false) return;
            for (int i = 0; i < ColliderRollbacks[roomid].Count; i++)
            {
                if (ColliderRollbacks[roomid][i].OwnerId == playerid)
                    continue;
                if (ColliderRollbacks[roomid][i] == null) continue;
                ColliderRollbacks[roomid][i].SetStateTransform((int)pt.Tick, (float)pt.Percent);
            }
        }


        public void Return(int playerid, int roomid)
        {
            if (ColliderRollbacks.Count == 0) return;
            if (ColliderRollbacks.ContainsKey(roomid) is false) return;

            for (int i = 0; i < ColliderRollbacks[roomid].Count; i++)
            {
                if (ColliderRollbacks[roomid][i].OwnerId == playerid)
                    continue;
                if (ColliderRollbacks[roomid][i] == null) continue;

                ColliderRollbacks[roomid][i].ResetStateTransform();
            }
        }

        #endregion

        #region OtherMethods

        public void AddColliderRollBack(int roomid, ColliderRollback _colliderRollback)
        {
            Debug.Log("ROOMID >" + roomid);

            if (ColliderRollbacks.ContainsKey(roomid))
            {
                ColliderRollbacks[roomid].Add(_colliderRollback);
                return;
            }

            if (ColliderRollbacks.ContainsKey(roomid) is false)
            {
                ColliderRollbacks.Add(roomid, new List<ColliderRollback>() { _colliderRollback });
                return;
            }
        }


        public void RemoveColliderRollBack(int roomid, ColliderRollback _colliderRollback)
        {
            Debug.Log($"<color=#00FF00> Room ID  ={roomid} </color>");
            if (ColliderRollbacks.ContainsKey(roomid))
            {
                ColliderRollbacks[roomid].Remove(_colliderRollback);

                if (ColliderRollbacks[roomid].Count == 0)
                {
                    ColliderRollbacks.Remove(roomid);
                }
            }
        }

        #endregion
    }
}