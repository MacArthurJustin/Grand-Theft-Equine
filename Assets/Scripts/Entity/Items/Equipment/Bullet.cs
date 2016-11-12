using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private LineRenderer _lineRenderer;
    public float TimetoLive = 1;
    private float Timer = float.MaxValue;
    public float Distance = 100;

    void Update()
    {
        Timer -= Time.deltaTime;

        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.SetColors(new Color(1, 1, 1, 0), new Color(1, 1, 1, Timer / TimetoLive));

        if(Timer <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    public virtual void Fire(Vector2 Position, Vector2 EndPoint)
    {
        if(_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();
        
        _lineRenderer.SetPosition(0, Position);
        _lineRenderer.SetPosition(1, EndPoint);

        Timer = TimetoLive;
    }
}
