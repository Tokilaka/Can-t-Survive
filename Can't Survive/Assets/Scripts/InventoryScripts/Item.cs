using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    public Sprite image;
    public ItemType type;
    public ActionType actionType;
    public AnimState animState;
    public bool stackable = true;
    public GameObject itemPrefab;
}

public enum ItemType
{
    Tool,
    Food,
    Gun
}

public enum ActionType
{
    Shoot,
    Mine
}

public enum AnimState
{
    Default,
    Pistol,
    Rifle
}
