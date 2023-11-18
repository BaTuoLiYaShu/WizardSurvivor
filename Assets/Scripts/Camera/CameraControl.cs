using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;

    private void OnEnable()
    {
        EventHandler.CameraShakeEvent += AtCameraShakeEvent;
    }

    private void OnDisable()
    {
        EventHandler.CameraShakeEvent -= AtCameraShakeEvent;
    }

    private void AtCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }
}
