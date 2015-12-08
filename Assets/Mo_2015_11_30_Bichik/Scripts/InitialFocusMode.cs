using UnityEngine;
using System.Collections;
using Vuforia;
using System;

[RequireComponent(typeof(VuforiaBehaviour))]
[DisallowMultipleComponent]
public class InitialFocusMode : MonoBehaviour
{
    private VuforiaBehaviour vuforiaBehaviour;

    public CameraDevice.FocusMode FocusMode = CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO;
    
    private void Awake()
    {
        vuforiaBehaviour = GetComponent(typeof(VuforiaBehaviour)) as VuforiaBehaviour;
    }

    private void Start()
    {
        vuforiaBehaviour.RegisterVuforiaStartedCallback(SetFocusMode);
    }

    private void SetFocusMode()
    {
        CameraDevice.Instance.SetFocusMode(FocusMode);

        vuforiaBehaviour.UnregisterVuforiaStartedCallback(SetFocusMode);
    }
}
