using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScroll : MonoBehaviour
{
    public RectTransform[] images; // Array of images to scroll
    public TMP_Text[] texts; // Change from Text[] to TMP_Text[] for TextMeshPro
    public float scrollSpeed = 1f; // Speed of scrolling
    public float targetY = 0f; // Target Y position to stop scrolling

    private void Start()
    {
        // Optionally, you can initialize any variables or settings here
    }

    private void Update()
    {
        ScrollElements();
    }

    private void ScrollElements()
    {
        Debug.Log("Moving...");
        // Calculate the amount to move based on the scroll speed and time
        float moveAmount = scrollSpeed * Time.deltaTime;

        // Scroll each image
        foreach (RectTransform image in images)
        {
            if (image.anchoredPosition.y < targetY)
            {
                image.anchoredPosition += new Vector2(0, moveAmount);
            }
        }

        // Scroll each TextMeshPro text
        foreach (TMP_Text text in texts)
        {
            RectTransform textTransform = text.GetComponent<RectTransform>();
            if (textTransform.anchoredPosition.y < targetY)
            {
                textTransform.anchoredPosition += new Vector2(0, moveAmount);
            }
        }
    }
}
