using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour, IControllable
{
  
    Transform _transform;
    SpriteRenderer _renderer;
    public float Speed = 10;
    public Sprite[] Sprites;
    private int pointer = 0; // 0 - 8
    private int direction = 0; // 0 = up 1 = right 2 = down 3 = left


    void Start()
    {
        _transform = GetComponent<Transform>();

        _renderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Vector2 Movement)
    {
        if (Movement.SqrMagnitude() > float.Epsilon)
        {
            switch(direction)
            {
                case 1: // Right
                    if (Movement.x < 0)
                    {
                        direction = 3;
                        pointer = 0;
                        SetSprite(Movement);
                    }

                    if(Mathf.Abs(Movement.x) < Mathf.Abs(Movement.y))
                    {
                        direction = 0;

                        if(Movement.y < 0)
                        {
                            direction = 2;
                        }

                        pointer = 0;
                        SetSprite(Movement);
                    }

                    _renderer.sprite = Sprites[8 + (pointer++)];
                    break;

                case 2:
                    if (Movement.y > 0)
                    {
                        direction = 0;
                        pointer = 0;
                        SetSprite(Movement);
                    }

                    if (Mathf.Abs(Movement.x) > Mathf.Abs(Movement.y))
                    {
                        direction = 1;

                        if (Movement.y < 0)
                        {
                            direction = 3;
                        }

                        pointer = 0;
                        SetSprite(Movement);
                    }

                    _renderer.sprite = Sprites[16 + (pointer++)];
                    break;

                case 3:
                    if (Movement.x > 0)
                    {
                        direction = 1;
                        pointer = 0;
                        SetSprite(Movement);
                    }

                    if (Mathf.Abs(Movement.x) < Mathf.Abs(Movement.y))
                    {
                        direction = 0;

                        if (Movement.y < 0)
                        {
                            direction = 2;
                        }

                        pointer = 0;
                        SetSprite(Movement);
                    }

                    _renderer.sprite = Sprites[24 + (pointer++)];
                    break;

                default:
                    if (Movement.y < 0)
                    {
                        direction = 2;
                        pointer = 0;
                        SetSprite(Movement);
                    }

                    if (Mathf.Abs(Movement.x) > Mathf.Abs(Movement.y))
                    {
                        direction = 1;

                        if (Movement.y < 0)
                        {
                            direction = 3;
                        }

                        pointer = 0;
                        SetSprite(Movement);
                    }

                    _renderer.sprite = Sprites[(pointer++)];
                    break;
            }
            pointer = pointer % 8;
        }

    }

    public void HandleInput(Controls Control)
    {

        _transform.Translate(Control.Movement * Speed * Time.deltaTime);

        SetSprite(Control.Movement);

    }
}
