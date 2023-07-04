using System;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;


namespace FishNet.Component.ColliderRollback
{
    public class ColliderRollback : NetworkBehaviour
    {
        #region Serialized.

        /// <summary>
        /// Objects holding colliders which can rollback.
        /// </summary>
        [Tooltip("Objects holding colliders which can rollback.")] [SerializeField]
        private GameObject[] _colliderParents = new GameObject[0];

        #endregion

        #region SelfVariables
        public Dictionary<int, FrameData> FrameData = new Dictionary<int, FrameData>();
        public List<int> Framekeys = new List<int>();

        private FrameData savedFrameData=new FrameData(null);
        private bool _subscribed;

        [SerializeField] private Mesh DrawMesh;
        [SerializeField] private Vector3 DrawScale;
        [SerializeField] private Vector3 DrawPositionOffset;
        #endregion


        #region ServerMethods

        public override void OnStartServer()
        {
            base.OnStartServer();
            savedFrameData = new FrameData(GetTransFormData(ref _colliderParents));
            ChangeSubscriptions(true);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if(IsOwner is false) return;

        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            ChangeSubscriptions(false);

        }
        

        private void TimeManager_OnTick()
        {
            AddFrame();
        }
        
        
        #endregion

        #region MainMethods


        [Server]
        public void AddRollBackServer(int roomid) => RollbackManager.AddColliderRollBack(roomid, this);
        
        [Server]
        public  void RemoveRollBackServer(int roomid)=>RollbackManager.RemoveColliderRollBack(roomid, this);
        
        public void AddFrame()
        {
            if (Framekeys.Count >= 60)
            {
                int key = Framekeys[0];
                Framekeys.RemoveAt(0);
                FrameData.Remove(key);
            }

            FrameData.Add((int)base.TimeManager.Tick, new FrameData(GetTransFormData(ref _colliderParents)));

            Framekeys.Add((int)base.TimeManager.Tick);
        }
        
        public void SetStateTransform(int frameId, float lerpAmount)
        {
            //save
            for (int i = 0; i < _colliderParents.Length; i++)
            {
                savedFrameData.TransformDatas[i].position = _colliderParents[i].transform.position;
                savedFrameData.TransformDatas[i].rotation = _colliderParents[i].transform.eulerAngles;
                Debug.Log(savedFrameData.TransformDatas[i].position);
            }

            for (int j = 0; j < FrameData[frameId].TransformDatas.Length; j++)
            {
                _colliderParents[j].transform.position=
                    Vector3.Lerp( FrameData[frameId-1].TransformDatas[j].position, 
                        FrameData[frameId].TransformDatas[j].position, lerpAmount);
                
                _colliderParents[j].transform.rotation =
                    Quaternion.Euler(FrameData[frameId - 1].TransformDatas[j].rotation);
            }
  
        }
        
        public void ResetStateTransform()
        {
            for (int i = 0; i < _colliderParents.Length; i++)
            {
                _colliderParents[i].transform.position = savedFrameData.TransformDatas[i].position;
                _colliderParents[i].transform.rotation = Quaternion.Euler(savedFrameData.TransformDatas[i].rotation);
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
                

            }
            else
            {
                base.TimeManager.OnTick -= TimeManager_OnTick;

            }
        }

       

        #endregion

        #region ResuableMethods

        private TransformData[] GetTransFormData(ref GameObject[] gameObjects)
        {
            TransformData[] datas = new TransformData[gameObjects.Length];
            for (int i = 0; i < gameObjects.Length; i++)
            {
                datas[i] = new TransformData(gameObjects[i].transform.position, gameObjects[i].transform.eulerAngles);

            }

            return datas;
        }

        #endregion


        private void OnDrawGizmos()
        {
            if(FrameData.Count==0)return;
            
            for (int i = 0; i < Framekeys.Count; i++)
            {
                for (int j = 0; j < FrameData[Framekeys[i]].TransformDatas.Length; j++)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireMesh(DrawMesh,FrameData[Framekeys[i]].TransformDatas[j].position+DrawPositionOffset,
                        Quaternion.Euler(FrameData[Framekeys[i]].TransformDatas[j].rotation),DrawScale);
                }
            }
            if(savedFrameData.TransformDatas == null) return;
            for (int i= 0; i < savedFrameData.TransformDatas.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireMesh(DrawMesh,savedFrameData.TransformDatas[i].position+DrawPositionOffset,
                    Quaternion.Euler(savedFrameData.TransformDatas[i].rotation),DrawScale);
            }
        }
    }

    public record FrameData
    {
        public TransformData[] TransformDatas;


        public FrameData(TransformData[] _transformDatas)
        {
            TransformDatas = _transformDatas;
        }
    }

    public record TransformData
    {
        public Vector3 position;

        public Vector3 rotation;

        public TransformData(Vector3 _position, Vector3 _rotation)
        {
            position = _position;
            rotation = _rotation;
        }
    }
}