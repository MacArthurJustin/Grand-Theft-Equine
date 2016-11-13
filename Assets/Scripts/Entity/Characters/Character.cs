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
public struct AnimationCombo
{
    public Sprite[] Value;
}

[Serializable]
public struct AnimationSet
{
    public int Frame;
    public int FrameCount;
    public Sprite[] Legs;
    public AnimationCombo[] Torso;
    public AnimationCombo[] Hands;
    public Sprite[] Bandana;
    public Sprite[] Face;
    public Sprite[] Hat;

    public int AddFrame()
    {
        ++Frame;

        if(Frame >= FrameCount)
        {
            Frame = 0;
        }

        return Frame;
    }

    public int ClearFrame()
    {
        Debug.Log(string.Format("Clearing"));
        return 0;
    }
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
        public bool Strafes;
    }

    [Serializable]
    public struct Layers
    {
        public SpriteRenderer Legs;
        public SpriteRenderer Torso;
        public SpriteRenderer Hands;
        public SpriteRenderer Bandana;
        public SpriteRenderer Face;
        public SpriteRenderer Hat;
    }

    [Serializable]
    public struct DirectionSet
    {
        public AnimationSet West;
        public AnimationSet NorthWest;
        public AnimationSet North;
        public AnimationSet NorthEast;
        public AnimationSet East;
        public AnimationSet SouthEast;
        public AnimationSet South;
        public AnimationSet SouthWest;

        public AnimationSet this[int index]
        {
            get
            { 
                switch(index)
                {
                    case 1:
                        return this.NorthWest;
                    case 2:
                        return this.North;
                    case 3:
                        return this.NorthEast;
                    case 4:
                        return this.East;
                    case 5:
                        return this.SouthEast;
                    case 6:
                        return this.South;
                    case 7:
                        return this.SouthWest;

                    case 0:
                    default:
                        return this.West;
                }
            }
            set
            {
                switch (index)
                {
                    case 1:
                        this.NorthWest = value;
                        break;
                    case 2:
                        this.North = value;
                        break;
                    case 3:
                        this.NorthEast = value;
                        break;
                    case 4:
                        this.East = value;
                        break;
                    case 5:
                        this.SouthEast = value;
                        break;
                    case 6:
                        this.South = value;
                        break;
                    case 7:
                        this.SouthWest = value;
                        break;

                    case 0:
                    default:
                        this.West = value;
                        break;
                }
            }
        }
    }

    public Layers SpriteRenderers;

    protected Transform _transform;
    protected SpriteRenderer _renderer;
    protected Collider2D _collider;

    /// <summary>
    /// List of Sprites used in Walking animations
    /// </summary>
    public DirectionSet DirectionalSprites;

    public singleAnimation DeathAnim;

    /// <summary>
    /// Configuration values for the Character
    /// </summary>
    public Configuration CharacterConfiguration;

    public int Frame = 0;

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
    public int _direction = 0;
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

    public virtual void ApplyDamage(float Damage)
    {
        CharacterConfiguration.CurrentHealth -= Damage;

        if(!isAlive)
        {
            OnDeath();
        }
    }

    public virtual void ApplyHealing(float Amount)
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
                    Frame = 0;
                }
            }
            
            Frame = Frame > 15 ? 0 : Frame;
            LookInDirection(_direction, (Frame++) / 4);
        }
    }

    public void LookInDirection(int Direction, int frame = 0)
    {
        SpriteRenderers.Legs.sprite = DirectionalSprites[Direction].Legs[frame];
        SpriteRenderers.Torso.sprite = DirectionalSprites[Direction].Torso[0].Value[frame];
        SpriteRenderers.Hands.sprite = DirectionalSprites[Direction].Hands[1].Value[frame];
        SpriteRenderers.Bandana.sprite = DirectionalSprites[Direction].Bandana[frame];
        SpriteRenderers.Face.sprite = DirectionalSprites[Direction].Face[frame];
        SpriteRenderers.Hat.sprite = DirectionalSprites[Direction].Hat[frame];
    }

    public virtual void HandleInput(Controls Control)
    {
        _transform.Translate(Control.Movement * Time.deltaTime * CharacterConfiguration.MovementSpeed);
        SetSprite(Control.Movement, CharacterConfiguration.Strafes && (Control.BottomLeft == ButtonState.Pressed || Control.BottomLeft == ButtonState.Held));
    }

    void FixedUpdate()
    {
        if(!isAlive)
        {
            if (DeathAnim.Frames.Length > 0)
            {
                _renderer.sprite = DeathAnim.Frames[DeathAnim.Frame++];

                if (DeathAnim.Frame > DeathAnim.Frames.Length)
                {
                    Destroy(gameObject);
                }
                return;
            }

            Destroy(gameObject);
        }
    }
}
