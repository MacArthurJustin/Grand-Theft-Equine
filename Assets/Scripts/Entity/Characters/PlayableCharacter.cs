using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Playable Character can Equip Items for bonuses as well as use.
/// </summary>
public class PlayableCharacter : Character {
    private List<IUsable> Items = new List<IUsable>();
    private Weapon Weapon;

    public void AddItem(IUsable Item)
    {
        Items.Add(Item);
    }

    public void RemoveItem(long Guid) {
        int Index = Items.FindIndex(item => item.Guid == Guid);
        Items.RemoveAt(Index);
    }

    public void SetWeapon(Weapon newWeapon)
    {
        this.Weapon = newWeapon;
    }

    public override void HandleInput(Controls Control)
    {
        base.HandleInput(Control);

        if(Control.TopLeft == ButtonState.Pressed)
        {
            Debug.Log("Firing Weapon");
            Weapon.Use(this);
        }
    }
}
