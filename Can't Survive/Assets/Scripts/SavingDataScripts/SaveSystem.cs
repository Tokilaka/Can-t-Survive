using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    void Start()
    {
        InventoryManager.Instance.LoadInventory();
    }

    private void OnApplicationQuit()
    {
        InventoryManager.Instance.SaveInventory();
    }
}
