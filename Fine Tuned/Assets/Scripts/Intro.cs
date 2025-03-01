using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Intro : MonoBehaviour
{
    public Camera playerCamera;
    public Transform cameraStartPosition;
    public Transform cameraEndPosition;
    public GameObject startingRectangle;
    public TMP_Text scrollingText;
    public GameObject titleCard;
    public Button startButton;
    public Button settingsButton;
    public float scrollSpeed = 0.5f;
    public float cameraMoveSpeed = 0.1f;

    private bool isScrolling = true;
    private bool isTitleCardActive = false;

    void Start()
    {
        playerCamera.transform.position = cameraStartPosition.position;
        playerCamera.transform.LookAt(startingRectangle.transform);
        titleCard.SetActive(false);
        StartCoroutine(ScrollText());
    }

    void Update()
    {
        if (isScrolling)
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, cameraEndPosition.position, cameraMoveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator ScrollText()
    {
        string fullText = scrollingText.text;
        scrollingText.text = "";
        foreach (char letter in fullText.ToCharArray())
        {
            scrollingText.text += letter;
            yield return new WaitForSeconds(scrollSpeed);
        }
        isScrolling = false;
        ShowTitleCard();
    }

    private void ShowTitleCard()
    {
        startingRectangle.GetComponent<Renderer>().material.color = Color.white; // Change texture/color
        titleCard.SetActive(true);
        isTitleCardActive = true;
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
    }

    private void StartGame()
    {
        // Spin character 180 degrees
        playerCamera.transform.Rotate(0, 180, 0);
        // Unlock movement controls
        // Example: playerController.enabled = true;
        SceneManager.LoadScene("GameScene"); // Load the main game scene
    }

    private void OpenSettings()
    {
        // Open settings menu
    }
}
