﻿using UnityEngine;
using System;
using System.Linq;

public class Weapon : MonoBehaviour, IUsable
{
    public enum Type
    {
        Revolver,
        DualRevolvers,
        Shotgun,
        Rifle
    }

    public enum State
    {
        Idle,
        Reloading
    }

    private State _state = State.Idle;

    [Serializable]
    public struct Configuration
    {
        public Type Type;
        public float Damage;
        public int MagazineSize;
        public int Ammuniton;
        public float TimeBetweenFiring;
        public float TimeToReload;
        public Bullet BulletPrefab;
    }

    public Configuration WeaponConfiguration;

    private float shot_timer = 0;
    private float reload_timer = 0;

    public bool CanUse
    {
        get
        {
            return WeaponConfiguration.Ammuniton > 0 && shot_timer <= 0;
        }
    }

    public int Guid
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    void Update()
    {
        shot_timer -= Time.deltaTime;
        reload_timer -= Time.deltaTime;

        if(reload_timer <= 0 && _state == State.Reloading)
        {
            WeaponConfiguration.Ammuniton = WeaponConfiguration.MagazineSize;
            _state = State.Idle;
        }
    }

    public void Use(PlayableCharacter C)
    {
        Use(C, new Collider2D[] { C.GetComponent<Collider2D>() });
    }

    public void Use(PlayableCharacter C, Collider2D[] IgnoreColliders)
    {
        if (_state == State.Reloading) return;

        if(CanUse)
        {
            WeaponConfiguration.Ammuniton--;

            Bullet Bill = (Bullet)Instantiate(WeaponConfiguration.BulletPrefab);

            if(Bill != null)
            {
                Vector2 EndPoint = (Vector2)C.transform.position + (C.Forward * Bill.Distance);

                RaycastHit2D[] RCH = Physics2D.RaycastAll(C.transform.position, C.Forward, Bill.Distance );

                Debug.Log(RCH.Length);
                
                foreach (RaycastHit2D Hit in RCH)
                {
                    if (Hit.collider != null)
                    {
                        if(IgnoreColliders.Contains(Hit.collider))
                        {
                            continue;
                        }

                        IDamagable Char = Hit.collider.GetComponent<IDamagable>();

                        if(Char != null)
                        {
                            if (Char.StopsBullet) EndPoint = Hit.point;

                            Char.ApplyDamage(WeaponConfiguration.Damage);
                        }

                        break;
                    }
                }

                Bill.Fire(C.transform.position, EndPoint);
            }

            // Fire Weapon
            // Spawn Bullet
            // Decrement Ammuniton
            shot_timer = WeaponConfiguration.TimeBetweenFiring;
            return;
        }

        if(WeaponConfiguration.Ammuniton <= 0)
        {
            // Active Reload Start
            reload_timer = WeaponConfiguration.TimeToReload;
            _state = State.Reloading;
        }
    }
}
