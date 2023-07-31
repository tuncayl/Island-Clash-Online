using System;
using System.Collections;
using System.Collections.Generic;
using Customdelegates.v1;
using onlinetutorial;
using onlinetutorial.controllers;
using onlinetutorial.Enums;
using UnityEngine;
using UnityEngine.Events;


namespace onlinetutorial.Signals
{
    public class PlayerSignal : MonoSingleton<PlayerSignal>
    {
        public Func<chibicolor, (Material, Material)> OnGetColor = delegate(chibicolor arg0) { return default; };

        public Func<Player> OnGetPlayer = delegate() { return default; };

        public UnityAction OnPlayerSpawned = delegate() { };



    }
}