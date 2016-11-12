using UnityEngine;
using System.Collections;
using System;

public class Weapon : MonoBehaviour, IUsable
{
    public enum Type
    {
        Revolver,
        DualRevolvers,
        Shotgun,
        Rifle
    }

    [Serializable]
    public struct Configuration
    {
        public Type Type;
        public float Damage;
        public int MagazineSize;
        public int Ammuniton;
        public GameObject BulletPrefab;
    }

    public Configuration WeaponConfiguration;

    public bool CanUse
    {
        get
        {
            return WeaponConfiguration.Ammuniton > 0;
        }
    }

    public long Guid
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public void Use(PlayableCharacter C)
    {
        if(CanUse)
        {
            //WeaponConfiguration.Ammuniton--;

            Debug.DrawRay(C.transform.position, C.Forward * 10, Color.red, 1);
            // Fire Weapon
            // Spawn Bullet
            // Decrement Ammuniton
            return;
        }

        // Active Reload Start
    }
}
