using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{
    public List<Stat> statsList;
}

[System.Serializable]
[CreateAssetMenu(menuName ="PlayerStats")]
public class Stat : ScriptableObject
{
    public string statName;
    public float currentValue;

    public float minValue, maxValue;
}




