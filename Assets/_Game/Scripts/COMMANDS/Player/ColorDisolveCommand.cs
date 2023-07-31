using System;
using DG.Tweening;
using JetBrains.Annotations;
using onlinetutorial.interfaces;
using onlinetutorial.controllers;
using onlinetutorial.Enums;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace COMMANDS.Playercommands
{
    public class ColorDisolveCommand : PlayerCommand
    {
        private SkinnedMeshRenderer renderer;
        private float disolvetime;
        private chibicolor Chibicolor;

        public ColorDisolveCommand(ref SkinnedMeshRenderer _renderer, float _disolvetime, chibicolor chibicolor)
        {
            renderer = _renderer;
            disolvetime = _disolvetime;
            Chibicolor = chibicolor;
        }


        public override void execute()
        {
            renderer.sharedMaterial =
                PlayerSignal.Instance.OnGetColor.Invoke(Chibicolor).Item2;

            renderer.sharedMaterial.SetVector("_DissolveOffest",
                new Vector3(0, -2.2f, 0));

            renderer.sharedMaterial.DOVector(new Vector3(0, 3.7f, 0), "_DissolveOffest",
                disolvetime);
        }
    }
}