using UnityEngine;
using System.Collections;
using System;

public enum Direction
{
    East,
    NorthEast,
    North,
    NorthWest,
    West,
    SouthWest,
    South,
    SouthEast
}

[Serializable]
public struct AnimationSet
{
    public int Frame;
    public Sprite[] Frames;
}


public class Character : MonoBehaviour, IControllable, IDamagable
{
    [Serializable]
    public struct Configuration
    {
        public float MovementSpeed;
        public float MaximumHealth;
        public float CurrentHealth;
        public float Alignment;
        public float Reputation;
        public bool CanInteract;
    }

    protected Transform _transform;
    protected SpriteRenderer _renderer;
    protected Collider2D _collider;

    /// <summary>
    /// List of Sprites used in Walking animations
    /// </summary>
    public AnimationSet[] DirectionalSprites;

    public AnimationSet DeathAnim;

    /// <summary>
    /// Configuration values for the Character
    /// </summary>
    public Configuration CharacterConfiguration;
    
    protected IController Owner;
    public IController Controller
    {
        get
        {
            return Owner;
        }
    }

    public void SetController(IController Controller)
    {
        Owner = Controller;
    }

    /// <summary>
    /// Direction the Character is facing
    /// 0: East
    /// 1: North East
    /// 2: North
    /// 3: North West
    /// 4: West
    /// 5: South West
    /// 6: South
    /// 7: South East
    /// </summary>
    private int _direction = 0;
    public Direction Direction
    {
        get
        {
            return (Direction)_direction;
        }
    }

    public Vector2 Forward
    {
        get
        {
            float Radian = _direction * ((Mathf.PI * 2) / 8);

            return new Vector2(-Mathf.Cos(Radian), Mathf.Sin(Radian));
        }
    }

    void Start()
    {
        _transform = GetComponent<Transform>();
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public bool isAlive
    {
        get
        {
            return CharacterConfiguration.CurrentHealth > 0;
        }
    }

    public bool StopsBullet
    {
        get
        {
            return true;
        }
    }

    protected virtual void OnDeath()
    {

    }

    public void ApplyDamage(float Damage)
    {
        CharacterConfiguration.CurrentHealth -= Damage;

        if(!isAlive)
        {
            OnDeath();
        }
    }

    public void ApplyHealing(float Amount)
    {
        CharacterConfiguration.CurrentHealth = Mathf.Min(CharacterConfiguration.MaximumHealth, CharacterConfiguration.CurrentHealth + Amount);
    }

    public void SetSprite(Vector2 Movement, bool strafe)
    {
        if (Movement.SqrMagnitude() > 0)
        {
            if (!strafe)
            {
                float Angle = Mathf.Atan2(Movement.y, -Movement.x);

                int Octant = Mathf.RoundToInt(8 * Angle / (2 * Mathf.PI) + 8) % 8;

                if (Octant != _direction)
                {
                    _direction = Octant;
                    DirectionalSprites[_direction].Frame = 0;
                }
            }

            _renderer.sprite = DirectionalSprites[_direction].Frames[DirectionalSprites[_direction].Frame++];

            DirectionalSprites[_direction].Frame = DirectionalSprites[_direction].Frame % DirectionalSprites[_direction].Frames.Length;
        }
    }

    public virtual void HandleInput(Controls Control)
    {
        _transform.Translate(Control.Movement * Time.deltaTime * CharacterConfiguration.MovementSpeed);
        SetSprite(Control.Movement, Control.BottomLeft == ButtonState.Pressed || Control.BottomLeft == ButtonState.Held);
    }

    void FixedUpdate()
    {
        if(!isAlive)
        {
            if(DeathAnim.Frames.Length > 0)
            {
                _renderer.sprite = DeathAnim.Frames[DeathAnim.Frame++];

                if(DeathAnim.Frame > DeathAnim.Frames.Length)
                {
                    Destroy(gameObject);
                }
                return;
            }

            Destroy(gameObject);
        }
    }
}
