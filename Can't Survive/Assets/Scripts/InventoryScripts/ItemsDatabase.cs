using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/ItemDatabase")]
public class ItemsDatabase : ScriptableObject
{
    public Item[] items;
}

[System.Serializable]
public class InventoryData
{
    public string[] id = new string[InventoryManager.Instance.inventorySlots.Length];
    public int[] count = new int[InventoryManager.Instance.inventorySlots.Length];
}