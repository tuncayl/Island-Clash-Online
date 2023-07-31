using System.Collections;
using System.Collections.Generic;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneObserverController : MonoBehaviour
{
    #region SelfVariables

    [Header("SceneObjects")] 
    [SerializeField] private GameObject[] _VisualObjects;
    #endregion

    #region UnityMethods

    private void Start()
    {

    }

    private void OnEnable()
    {
        Subscire();
    }

    private void OnDisable()
    {
        UnSubscire();
    }

    #endregion

    #region MainMethods

    private void OnMainSceneLoading() => SetEnabledSceneObject(false);

    private void OnMenuSceneloaded() => SetEnabledSceneObject(true);

    #endregion

    #region OtherMethods


    private void SetEnabledSceneObject(bool value)
    {
        for (int i = 0; i < _VisualObjects.Length; i++)
        {
            _VisualObjects[i].SetActive(value);
        }

    }
    #endregion

    #region SubscireMethods

    private void Subscire()
    {
        SceneSignals.Instance.OnMainSceneloading += OnMainSceneLoading;
        SceneSignals.Instance.OnMenuSceneLoaded += OnMenuSceneloaded;
    }

   

    private void UnSubscire()
    {
        SceneSignals.Instance.OnMainSceneloading -= OnMainSceneLoading;
        SceneSignals.Instance.OnMenuSceneLoaded -= OnMenuSceneloaded;


    }
    #endregion

}
