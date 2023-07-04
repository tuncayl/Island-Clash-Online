using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslanPhysicController : NetworkBehaviour
{
    #region SelfVariables

    private PhysicsScene physicsScene;
    #endregion


    #region UnityMethods

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();       
        Subscire();

    }

    private void OnDisable()
    {
        UnSubscire();
    }


    private void Awake()
    {
        physicsScene = gameObject.scene.GetPhysicsScene();

    }

    #endregion

    #region OtherMethods

    private PhysicsScene OnGetPhysicScene() => physicsScene;
    private void TimeManager_PhysSim(float delta)
    {
        physicsScene.Simulate(delta);
    }
    #endregion


    #region SubscireMethods

    private void Subscire()
    {

        if(base.IsServer is false )return;
        InstanceFinder.TimeManager.OnPrePhysicsSimulation += TimeManager_PhysSim;

    }

 

    private void UnSubscire()
    {
        if(base.IsServer is false )return;
        InstanceFinder.TimeManager.OnPrePhysicsSimulation -= TimeManager_PhysSim;

    }

    #endregion
}