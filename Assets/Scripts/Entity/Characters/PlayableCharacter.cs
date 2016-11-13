using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Playable Character can Equip Items for bonuses as well as use.
/// </summary>
public class PlayableCharacter : Character {
    private List<IUsable> _items = new List<IUsable>();
    private Weapon _weapon;
    
    public Text HealthText;
    public Image[] HealthImages;

    public List<IUsable> Items
    {
        get
        {
            return _items;
        }
    }

    public Weapon Weapon
    {
        get
        {
            return _weapon;
        }
    }

    public virtual void AddItem(IUsable Item)
    {
        _items.Add(Item);

        if (HealthText != null) HealthText.text = _items.Where(item => item is HealingItem).Count().ToString();
    }

    public void RemoveItem(IUsable Item)
    {
        if (_items.Count <= 0) return;

        _items.Remove(Item);

        if(HealthText != null) HealthText.text = _items.Where(item => item is HealingItem).Count().ToString();
    }

    public void SetWeapon(Weapon newWeapon)
    {
        this._weapon = newWeapon;
    }

    public void FireWeapon(Collider2D[] Colliders)
    {
        if (_weapon != null)
        {
            _weapon.Use(this, Colliders);
        }
    }

    public void UseHealingItem()
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

    void SetHealthImage()
    {
        if(HealthImages.Length > 0)
        {
            int perc = Mathf.FloorToInt((CharacterConfiguration.CurrentHealth / CharacterConfiguration.MaximumHealth) * HealthImages.Length);
            for (int index = 0; index < HealthImages.Length; index++) {
                HealthImages[index].enabled = index < perc;
            }
        }
    }

    public override void ApplyDamage(float Damage)
    {
        base.ApplyDamage(Damage);
        SetHealthImage();
    }

    public override void ApplyHealing(float Amount)
    {
        base.ApplyHealing(Amount);
        SetHealthImage();
    }

    public override void HandleInput(Controls Control)
    {
        base.HandleInput(Control);

        if (Control.TopLeft == ButtonState.Pressed)
        {
            FireWeapon(new Collider2D[] { _collider });
        }

        if(Control.TopMiddle == ButtonState.Pressed)
        {
            if (CharacterConfiguration.CanInteract)
            {
                RaycastHit2D[] RCH = Physics2D.RaycastAll(transform.position, Forward, 1);

                foreach (RaycastHit2D Hit in RCH)
                {
                    if (Hit.collider != null && Hit.collider != _collider)
                    {
                        IInteractable Interact = Hit.collider.GetComponent<IInteractable>();

                        if (Interact != null)
                        {
                            Interact.Interact(this);
                            break;
                        }
                    }
                }
            }
        }

        if (Control.TopRight == ButtonState.Pressed)
        {
            UseHealingItem();
        }
    }
}
