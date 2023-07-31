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
    public class ColorReDisolveCommand : PlayerCommand
    {
        private SkinnedMeshRenderer renderer;
        private float disolvetime;
        private chibicolor Chibicolor;
        private Action<chibicolor> OnComplete;

        public ColorReDisolveCommand(ref SkinnedMeshRenderer _renderer, 
            float _disolvetime, chibicolor chibicolor,Action<chibicolor> _onComplete)
        {
            renderer = _renderer;
            disolvetime = _disolvetime;
            Chibicolor = chibicolor;
            OnComplete = _onComplete;
        }


        public override void execute()
        {
            
            renderer.sharedMaterial =
                PlayerSignal.Instance.OnGetColor.Invoke(Chibicolor).Item2;

            renderer.sharedMaterial.SetVector("_DissolveOffest",
                new Vector3(0, 3.7f, 0));

            renderer.sharedMaterial.DOVector(new Vector3(0, -2.2f, 0), "_DissolveOffest",
                disolvetime).OnComplete(()=>OnComplete.Invoke(Chibicolor));
        }
    }
}