using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionData", menuName = "lib/GameDependent/InteractionData", order = 1)]
public class InteractionData : ScriptableObject
{
    [Header("General Variables")]
    public int Capacity;
    public bool isUseBeacon;
    public float InteractionRange;
    public int Priority;
    public Sprite Icon;

    [Header("Collectable Object Variables")]
    public int CollectableObjectId;
    // public CollectableShowType showType;

    [Header("Upgrade Datas")]
    public List<InteractionUpgradeItem> UpgradeItems;
}

[Serializable]
public class InteractionUpgradeItem
{
    public string UpgradeName;
    public float Multiplier;
    public float Addition;
    public List<int> Prices = new List<int>();
}