﻿using UnityEngine;
using System;

public class StageProp : MonoBehaviour {
    [Serializable]
    public struct Configuration
    {
        public AnimationSet Animation;
        public bool Animated;
    }

    protected SpriteRenderer _renderer;
    public Configuration PropConfiguration;
    protected int _frame = 0;

    protected virtual void FixedUpdate()
    {
        if (PropConfiguration.Animated) {
            if(_renderer == null)
            {
                _renderer = GetComponent<SpriteRenderer>();
            }

            _renderer.sprite = PropConfiguration.Animation.Frames[_frame++];

            _frame = _frame % PropConfiguration.Animation.Frames.Length;
        }
    }
}
