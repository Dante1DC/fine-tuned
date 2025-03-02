using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public Sprite display;
    public int beats;
    public float pitch;
    public KeyCode key;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer && display)
        {
            spriteRenderer.sprite = display;
        }
        transform.position = new Vector2(transform.position.x, pitch);
    }
}
