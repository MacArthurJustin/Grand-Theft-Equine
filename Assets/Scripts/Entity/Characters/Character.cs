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

public class Character : MonoBehaviour, IControllable
{
    [Serializable]
    public struct AnimationSet
    {
        public Sprite[] Frames;
    }

    [Serializable]
    public struct Configuration
    {
        public float MovementSpeed;
        public float MaximumHealth;
        public float CurrentHealth;
        public float Alignment;
        public float Reputation;
    }

    Transform _transform;
    SpriteRenderer _renderer;

    /// <summary>
    /// List of Sprites used in Walking animations
    /// </summary>
    public AnimationSet[] DirectionalSprites;

    /// <summary>
    /// Configuration values for the Character
    /// </summary>
    public Configuration CharacterConfiguration;

    /// <summary>
    /// Frame that the animation is currently on
    /// </summary>
    private int _frame = 0;

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
    }

    public bool isAlive
    {
        get
        {
            return CharacterConfiguration.CurrentHealth > 0;
        }
    }

    public void ApplyDamage(float Damage)
    {
        CharacterConfiguration.CurrentHealth -= Damage;
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
                    _frame = 0;
                }
            }

            _renderer.sprite = DirectionalSprites[_direction].Frames[_frame++];

            _frame = _frame % 8;
        }
    }

    public virtual void HandleInput(Controls Control)
    {
        _transform.Translate(Control.Movement * Time.deltaTime * CharacterConfiguration.MovementSpeed);
        SetSprite(Control.Movement, Control.BottomLeft == ButtonState.Pressed || Control.BottomLeft == ButtonState.Held);
    }
}
