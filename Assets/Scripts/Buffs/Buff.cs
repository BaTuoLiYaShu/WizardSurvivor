using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Buff
{
    [FormerlySerializedAs("skillEffect")] public BuffType buffType;
    public Sprite icon;
    public string description;
}