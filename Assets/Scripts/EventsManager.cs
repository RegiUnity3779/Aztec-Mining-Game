using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventsManager
{
    public static Action<ItemData> AddToInventory;
    public static Action<Boolean, GameObject> Interactable;
    public static Action<Vector3> StairsRevealed;
    public static Action<ItemData> EquipableItem;
    public static Action EquipItem;
    public static Action UnEquipItem;
    public static Action<GameObject> GroundObjectRemoved;
    public static Action DownStairs;
    public static Action<GameObject> FindPlayerInteractor;
    public static Action<GameObject> ItemInScene;
}
