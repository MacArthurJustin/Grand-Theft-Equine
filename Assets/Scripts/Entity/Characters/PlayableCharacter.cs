using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Playable Character can Equip Items for bonuses as well as use.
/// </summary>
public class PlayableCharacter : Character {
    private List<IUsable> _items = new List<IUsable>();
    private Weapon Weapon;
    
    public Text HealthText;

    public List<IUsable> Items
    {
        get
        {
            return _items;
        }
    }

    public void AddItem(IUsable Item)
    {
        _items.Add(Item);

        HealthText.text = _items.Where(item => item is HealingItem).Count().ToString();
    }

    public void RemoveItem(IUsable Item)
    {
        if (_items.Count <= 0) return;

        _items.Remove(Item);

        HealthText.text = _items.Where(item => item is HealingItem).Count().ToString();
    }

    public void SetWeapon(Weapon newWeapon)
    {
        this.Weapon = newWeapon;
    }

    public override void HandleInput(Controls Control)
    {
        base.HandleInput(Control);

        if (Control.TopLeft == ButtonState.Pressed)
        {
            Weapon.Use(this);
        }
        if (Control.TopRight == ButtonState.Pressed)
        {
            if (_items.Count > 0)
            {
                IUsable Item = _items.Where(item => item is HealingItem).First();

                if (Item != null)
                {
                    Item.Use(this);
                }
            }
        }
    }
}
