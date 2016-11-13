using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

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
    public struct Reloader
    {
        public Image Cylinder;
        public Image[] Bullets;

        public void RotateCylinder(float Value = float.NaN)
        {
            if(float.IsNaN(Value))
            {
                Cylinder.GetComponent<RectTransform>().rotation = Quaternion.identity;
                return;
            }
            Cylinder.GetComponent<RectTransform>().Rotate(Vector3.forward * Value);
        }

        public void RemoveBullet()
        {
            foreach (Image I in Bullets)
            {
                if (I.enabled)
                {
                    I.enabled = false;
                    break;
                }
            }
        }

        public void ReturnBullet(bool all = false)
        {
            for (int I = 5; I >= 0; I--)
            {
                if (!Bullets[I].enabled)
                {
                    Bullets[I].enabled = true;
                    if(!all) break;
                }
            }
        }
    }

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
    public Reloader[] Chambers;

    private int ChamberCount = 1;
    private float shot_timer = 0;
    private float reload_timer = 0;
    private float arc_timer = 0;
    private float shotArc = 0;
    private int CurrentChamber = 0;

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

    void Start()
    {
        shotArc = -60 / WeaponConfiguration.TimeBetweenFiring;
        switch(WeaponConfiguration.Type)
        {
            case Type.DualRevolvers:
                ChamberCount = 2;
                break;

            default:
                ChamberCount = 1;
                break;
        }
    }

    void Initialize(Configuration WeaponConfig)
    {
        this.WeaponConfiguration = WeaponConfig;
        switch(WeaponConfiguration.Type)
        {
            case Type.DualRevolvers:
                Chambers[0].Cylinder.gameObject.SetActive(true);
                Chambers[1].Cylinder.gameObject.SetActive(true);
                ChamberCount = 2;
                break;

            case Type.Revolver:
            default:
                Chambers[0].Cylinder.gameObject.SetActive(true);
                Chambers[1].Cylinder.gameObject.SetActive(false);
                ChamberCount = 1;
                break;
        }
    }

    void Update()
    {
        if(_state == State.Reloading)
        {
            if(HandleReload())
            {
                _state = State.Idle;
            }
            return;
        }

        shot_timer -= Time.deltaTime;
        if(shot_timer > 0)
        {
            Chambers[(WeaponConfiguration.Ammuniton % ChamberCount)].RotateCylinder(shotArc * Time.deltaTime);
        }
    }

    public void FireWeapon()
    {
        Chambers[(WeaponConfiguration.Ammuniton % ChamberCount)].RemoveBullet();
    }

    public bool HandleReload()
    {
        reload_timer -= Time.deltaTime;
        arc_timer -= Time.deltaTime;
        
        Chambers[CurrentChamber].RotateCylinder((60 * ((ChamberCount * 6) / WeaponConfiguration.TimeToReload)) * Time.deltaTime);

        if (arc_timer <= 0)
        {
            arc_timer = WeaponConfiguration.TimeToReload / (ChamberCount * 6);

            Chambers[CurrentChamber].ReturnBullet();
            CurrentChamber = (++CurrentChamber) % Chambers.Length;
        }

        if(reload_timer <= 0)
        {
            foreach(Reloader Chamber in Chambers)
            {
                Chamber.RotateCylinder();
                Chamber.ReturnBullet(true);
            }

            WeaponConfiguration.Ammuniton = WeaponConfiguration.MagazineSize;
            return true;
        }

        return false;
    }

    public Vector2 HandleBullet(Vector2 EndPoint, RaycastHit2D[] RCH, Collider2D[] IgnoreColliders)
    {
        foreach (RaycastHit2D Hit in RCH)
        {
            if (Hit.collider != null)
            {
                if (IgnoreColliders.Contains(Hit.collider))
                {
                    continue;
                }

                IDamagable Char = Hit.collider.GetComponent<IDamagable>();

                if (Char != null)
                {
                    bool stop = Char.StopsBullet;

                    Char.ApplyDamage(WeaponConfiguration.Damage);

                    if (stop)
                    {
                        return Hit.point;
                    }
                }
            }
        }

        return EndPoint;
    }

    public void Use(PlayableCharacter C)
    {
        Use(C, new Collider2D[] { C.GetComponent<Collider2D>() });
    }

    public void Use(PlayableCharacter C, Collider2D[] IgnoreColliders)
    {
        if(CanUse)
        {
            WeaponConfiguration.Ammuniton--;
            FireWeapon();

            Bullet Bill = (Bullet)Instantiate(WeaponConfiguration.BulletPrefab);

            if(Bill != null)
            {

                Vector2 EndPoint = (Vector2)C.transform.position + (C.Forward * Bill.Distance);

                RaycastHit2D[] RCH = Physics2D.RaycastAll(C.transform.position, C.Forward, Bill.Distance);

                EndPoint = HandleBullet(EndPoint, RCH, IgnoreColliders);

                Bill.Fire(C.transform.position, EndPoint);
            }
            
            shot_timer = WeaponConfiguration.TimeBetweenFiring;
            return;
        }

        if(WeaponConfiguration.Ammuniton <= 0)
        {
            if(_state == State.Reloading)
            {
                // Perform Active Reload
                return;
            }

            reload_timer = WeaponConfiguration.TimeToReload;
            arc_timer = WeaponConfiguration.TimeToReload / (ChamberCount * 6);
            _state = State.Reloading;
            CurrentChamber = 0;
        }
    }
}
