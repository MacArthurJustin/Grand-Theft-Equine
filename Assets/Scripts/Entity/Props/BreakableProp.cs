using UnityEngine;
using System.Collections;
using System;

public class BreakableProp : StageProp, IDamagable
{
    [Serializable]
    public struct BreakableConfiguration
    {
        public float Health;
        public AnimationSet BreakAnimation;
        public bool StopsBullet;
    }

    public BreakableConfiguration Breakable;

    public bool isAlive
    {
        get
        {
            return Breakable.Health > 0;
        }
    }

    public bool StopsBullet
    {
        get
        {
            return Breakable.StopsBullet;
        }
    }

    public void ApplyDamage(float amount)
    {
        Breakable.Health -= amount;
        if (!isAlive)
        {
            _frame = 0;
        }
    }

    protected override void FixedUpdate()
    {
        if (!isAlive)
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
            _renderer.sprite = Breakable.BreakAnimation.Frames[_frame++];

            if(_frame >= Breakable.BreakAnimation.Frames.Length)
            {
                Destroy(gameObject);
            }
            return;
        }

        base.FixedUpdate();
    }
}
