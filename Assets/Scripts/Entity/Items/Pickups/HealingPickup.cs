using UnityEngine;
using System.Collections;

public class HealingPickup : Pickup {
    public float Health;

    protected override void HandlePickup(PlayableCharacter PC)
    {
        PC.AddItem(new HealingItem(Health));
    }
}
