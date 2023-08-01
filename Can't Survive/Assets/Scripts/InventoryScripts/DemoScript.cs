using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] items;

    public void PickUpItem(int Id)
    {
        bool result = inventoryManager.AddItem(items[Id]);
        if (result == true)
        {
            //added item
        }
        else
        {
            //not added item
        }
    }

    public void GetSelectedItem()
    {
        Item recivedItem = inventoryManager.GetSelectedItem(false);
        if (recivedItem != null)
        {
            Debug.Log(recivedItem);
        }
        else
        {
            Debug.Log("NOthing");
        }
    }

    public void UseSelectedItem()
    {
        Item recivedItem = inventoryManager.GetSelectedItem(true);
        if (recivedItem != null)
        {
            //used item
        }
        else
        {
            //not used item
        }
    }
}
