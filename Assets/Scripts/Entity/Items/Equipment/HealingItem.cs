using UnityEngine;

public class HealingItem : IUsable
{
    public int GUID;
    public float Health;

    public HealingItem(float Health)
    {
        GUID = Random.Range(int.MinValue, int.MaxValue);
        this.Health = Health;
    }

    public bool CanUse
    {
        get
        {
            return true;
        }
    }

    public int Guid
    {
        get
        {
            return GUID;
        }
    }

    public void Use(PlayableCharacter C)
    {
        C.ApplyHealing(Health);
        C.RemoveItem(this);
    }
}
