using System;
using FishNet.Connection;
using onlinetutorial.controllers;
using onlinetutorial.interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace onlinetutorial.Signals
{
    public class IslandSignals: MonoSingleton<IslandSignals>
    {
       public  Func<int> OnGetIslandId= delegate { return default;};
       

       public  UnityAction<NetworkConnection> OnStartSpawnCounter=delegate(NetworkConnection arg0) {  };
       
       public  UnityAction<Player> OnSpawnPosition=delegate(Player arg0) {  };


    }
}