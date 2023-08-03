using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventsManager
{
    public static Action<ItemData> AddToInventory;
    public static Action<ItemData> RemoveFromInventory;
    public static Action<Boolean, GameObject> Interactable;
    public static Action<Vector3> StairsRevealed;
    public static Action<ItemData> EquipableItem;
    public static Action EquipItem;
    public static Action UnEquipItem;
    public static Action EatButton;
    public static Action <int>EatConsumerable;
    public static Action<Boolean> EquipButton;
    public static Action<Boolean> UnEquipButton;
    public static Action<Boolean, ItemData> EquipToggle;
    public static Action<Boolean> EquipToggleInteractable;
    public static Action RemoveEquipableItem;
    public static Action<GameObject> GroundObjectRemoved;
    public static Action DownStairs;
    public static Action UpStairs;
    public static Action<Vector3> UpdatePlayerLocation;
    public static Action<int> FloorChange;
    public static Action PlayerMarker;
    public static Action<GameObject> ItemInScene;
    public static Action Stamina;
    public static Action StaminaRestored;
    public static Action<String> SceneChange;
    public static Action UpdateCamera;
    public static Action UnderGroundCheck;
    public static Action<Boolean> IsUnderGround;
    public static Action Fainted;
}
