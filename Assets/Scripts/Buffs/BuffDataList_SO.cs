using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Buff/BuffDataList_SO")]
public class BuffDataList_SO : ScriptableObject
{
    public List<Buff> buffDataList;
}
