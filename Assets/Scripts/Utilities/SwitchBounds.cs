using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchBounds : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += AtAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= AtAfterSceneLoadedEvent;
    }

    private void AtAfterSceneLoadedEvent()
    {
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner2D confiner = GetComponent<CinemachineConfiner2D>();

        confiner.m_BoundingShape2D = confinerShape;
    }
}
