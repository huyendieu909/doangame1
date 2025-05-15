using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUps : MonoBehaviour
{
    private SpriteRenderer sprite;
    protected virtual void Start () {
        sprite = GetComponent<SpriteRenderer>();
        InvokeRepeating("Blink", 0, 0.1f);
    }
    void Blink()
    {
        sprite.enabled = !sprite.enabled;
    }
}
