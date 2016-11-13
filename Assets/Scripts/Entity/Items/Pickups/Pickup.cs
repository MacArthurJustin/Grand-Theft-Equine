using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    protected virtual void HandlePickup(PlayableCharacter PC)
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider == null) return;

        PlayableCharacter PC = coll.collider.GetComponent<PlayableCharacter>();

        if(PC != null)
        {
            HandlePickup(PC);

            Destroy(gameObject);
        }
    }
}
