using UnityEngine;
using System.Collections;
using System;

public class HealingItem : MonoBehaviour, IUsable
{
    public long GUID;
    public float Health;

    public bool CanUse
    {
        get
        {
            return true;
        }
    }

    public long Guid
    {
        get
        {
            return GUID;
        }
    }

    public void Use(PlayableCharacter User)
    {
        User.ApplyHealing(Health);
        User.RemoveItem(GUID);
    }
}
