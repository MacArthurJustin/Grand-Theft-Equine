using UnityEngine;
using System.Collections;
using System;

public class VehicleCharacter : PlayableCharacter, IInteractable
{
    private PlayableCharacter Rider;

    public void SetRider(PlayableCharacter PC)
    {
        if(Rider != null)
        {
            Rider.transform.parent = null;
            Rider.transform.position -= Vector3.up;

            Collider2D col = Rider.GetComponent<Collider2D>();
            col.enabled = true;

            Controller.SetTarget(Rider);
        }

        Rider = PC;

        if (PC != null)
        {
            PC.transform.position += Vector3.up;
            PC.transform.parent = transform;
            PC.Controller.SetTarget(this);

            Collider2D col = PC.GetComponent<Collider2D>();
            col.enabled = false;
        }
    }

    public bool CanInteract
    {
        get
        {
            return Rider == null;
        }
    }

    public void Interact(PlayableCharacter PC)
    {
        if(CanInteract)
        {
            SetRider(PC);
        }
    }

    public override void AddItem(IUsable Item)
    {
        if(Rider != null)
        {
            Rider.AddItem(Item);
        }
    }

    public override void HandleInput(Controls Control)
    {
        base.HandleInput(Control);

        if(Rider != null)
        {
            Rider.SetSprite(Control.Movement, Control.BottomLeft == ButtonState.Pressed || Control.BottomLeft == ButtonState.Held);

            if (Control.TopLeft == ButtonState.Pressed)
            {
                Rider.FireWeapon(new Collider2D[] { _collider, Rider.GetComponent<Collider2D>() });
            }

            if (Control.TopMiddle == ButtonState.Pressed)
            {
                if (Control.Movement.sqrMagnitude < float.Epsilon)
                {
                    SetRider(null);
                }
            }
            
            if (Control.TopRight == ButtonState.Pressed)
            {
                Rider.UseHealingItem();
            }
        }
    }
}
