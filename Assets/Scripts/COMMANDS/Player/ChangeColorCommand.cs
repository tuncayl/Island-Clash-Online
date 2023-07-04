using System;
using DG.Tweening;
using onlinetutorial.interfaces;
using onlinetutorial.controllers;
using onlinetutorial.data;
using onlinetutorial.Enums;
using onlinetutorial.Signals;
using UnityEngine;

namespace COMMANDS.Playercommands
{
    public class ChangeColorCommand : PlayerCommand
    {
        private ResuableGeneralData GeneralData;
        private SkinnedMeshRenderer Renderer;
        private chibicolor Chibicolor;

        public ChangeColorCommand(ref SkinnedMeshRenderer _renderer,ref ResuableGeneralData _generalData , chibicolor _chibicolor)
        {
            Renderer = _renderer;
            Chibicolor = _chibicolor;
            GeneralData = _generalData;
        }


        public override void execute()
        {
            GeneralData.Chibicolor = Chibicolor;
            Renderer.sharedMaterial =
                PlayerSignal.Instance.OnGetColor.Invoke(Chibicolor).Item1;
        }
    }
}