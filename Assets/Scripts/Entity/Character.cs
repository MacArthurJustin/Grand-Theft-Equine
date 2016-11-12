using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour, IControllable
{
  public float speed;
    Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    public void HandleInput(Controls Control)
    {
        _transform.Translate(Control.Movement * speed * Time.deltaTime);
    }
}
