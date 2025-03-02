using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public Sprite display;
    public int beats;
    public float pitch;
    public KeyCode key;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        if (image != null && display != null)
        {
            image.sprite = display;
            image.SetNativeSize();
        }
    }
}
