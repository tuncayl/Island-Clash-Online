using System;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Serializing;
using onlinetutorial.data;
using onlinetutorial.Enums;
using onlinetutorial.interfaces;
using UnityEngine;

namespace onlinetutorial.Extensions
{
    public static class ExtensionsMethods
    {
        #region CustomSerializers

        //Custom Serializers ResuableGeneralData
        public static void WriteResuableGeneralData(this Writer writer, ResuableGeneralData value)
        {
            writer.WriteSingle((int)value.Chibicolor);
        }

        public static ResuableGeneralData ReadEnemy(this Reader reader)
        {
            ResuableGeneralData e = new ResuableGeneralData((chibicolor)reader.ReadSingle());

            return e;
        }

        #endregion

        #region LayerMask

        /// <summary> Converts layermask to layer number </summary>
        internal static int ToLayer(this LayerMask mask)
        {
            var bitmask = mask.value;
            int result = bitmask > 0 ? 0 : 31;
            while (bitmask > 1)
            {
                bitmask = bitmask >> 1;
                result++;
            }

            return result;
        }

        #endregion

        #region Network

        public static NetworkObject[] CoppyNetworkObjects<T>(this T pb, ref List<NetworkConnection> connections)
        {
            NetworkObject[] comms = new NetworkObject[connections.Count];
            for (int i = 0; i < connections.Count; i++) comms[i] = connections[i].FirstObject;
            return comms;
        }
        public static NetworkConnection[] CoppyNetworkConnections<T>(this T pb, ref List<NetworkConnection> connections)
        {
            NetworkConnection[] cons = new NetworkConnection[connections.Count];
            for (int i = 0; i < connections.Count; i++) cons[i] = connections[i];
            return cons;
        }
        #endregion
        
        #region Transform

        public static void ClearRoot(this Transform root)
        {
            foreach (Transform child in root)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }

        public static void AddRoot<T>(this Transform root, ref List<T> Datas, GameObject InstanceObject)
        {
            for (int i = 0; i < Datas.Count; i++)
            {
                if (GameObject.Instantiate(InstanceObject, root).TryGetComponent(out IUilist IUlist))
                {
                    IUlist.SetUiListData(Datas[i]);
                }
            }
        }

        #endregion

        #region Enum

        public static T ToEnum<T>(this object obj)
        {
            var objType = obj.GetType();
            if (typeof(T).IsEnum)
            {
                if (objType == typeof(string))
                    return (T)Enum.Parse(typeof(T), obj.ToString());
                return (T)Enum.ToObject(typeof(T), obj);
            }
            if (objType == typeof(string))
                return (T)Enum.Parse(Nullable.GetUnderlyingType(typeof(T)), obj.ToString());
            return (T)Enum.ToObject(Nullable.GetUnderlyingType(typeof(T)), obj);
        }

        #endregion

        #region String

        public static string ConvertMinuteSecond(this float time)
        {
            var minutes = Mathf.FloorToInt(time / 60);  
            var seconds = Mathf.FloorToInt(time % 60);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        public static string ConvertMinuteSecond(this int time)
        {
            var minutes = Mathf.FloorToInt(time / 60);  
            var seconds = Mathf.FloorToInt(time % 60);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        #endregion
    }
}