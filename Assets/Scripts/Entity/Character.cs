using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour, IControllable
{
    RectTransform _transform;

    void Start()
    {
        _transform = GetComponent<RectTransform>();
    }

    public void HandleInput(Controls Control)
    {
        _transform.Translate(Control.Movement);
    }
}
