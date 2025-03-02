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
    public float scrollSpeed = 1f;
    public float cameraMoveSpeed = 1f;
    public Transform player; // Reference to the player object

    private bool isScrolling = true;
    private bool isTitleCardActive = false;

    // ui images (title card background)
    public GameObject Back; 
    public GameObject Middle; 
    public GameObject Front; 

    void Start()
    {
        startingRectangle.GetComponent<Renderer>().material.color = Color.black;
        Debug.Log("Time Scale: " + Time.timeScale);
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        
        playerCamera.transform.position = cameraStartPosition.position;
        playerCamera.transform.LookAt(startingRectangle.transform);

        titleCard.SetActive(false);
        Back.SetActive(false);
        Middle.SetActive(false);
        Front.SetActive(false);

        StartCoroutine(ScrollText());

        if (isScrolling = true){
            startButton.gameObject.SetActive(false);
            settingsButton.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isScrolling)
        {
            // Move the player (and thus the camera) from start to end position
            player.position = Vector3.Lerp(player.position, cameraEndPosition.position, (cameraMoveSpeed/10) * Time.deltaTime);
        }
    }

    private IEnumerator ScrollText()
    {
        //Debug.Log("Starting text scroll...");
        string fullText = scrollingText.text;
        scrollingText.text = "";
        foreach (char letter in fullText.ToCharArray())
        {
            scrollingText.text += letter;
            //Debug.Log($"Adding letter: {letter}");
            Debug.Log("Time Scale: " + Time.timeScale);
            yield return new WaitForSeconds(scrollSpeed / 15);
        }
        isScrolling = false;
        ShowTitleCard();
    }

    private void ShowTitleCard()
    {
         if (isTitleCardActive) return; // Prevent multiple activations

        Debug.Log("Showing title card...");
        isTitleCardActive = true; // Mark it as active
        StartCoroutine(FadeOutTextAndChangeColor());
    }

    private IEnumerator FadeOutTextAndChangeColor()
    {
        yield return new WaitForSeconds(1f); // Wait for one second

        float duration = 2f; // Duration of the fade effect
        float elapsedTime = 0f;

        Color initialTextColor = scrollingText.color;
        Color initialRectangleColor = startingRectangle.GetComponent<Renderer>().material.color;
        Color targetRectangleColor = Color.white;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Fade out text
            scrollingText.color = new Color(initialTextColor.r, initialTextColor.g, initialTextColor.b, Mathf.Lerp(1f, 0f, t));

            // Change rectangle color from black to white
            startingRectangle.GetComponent<Renderer>().material.color = Color.Lerp(initialRectangleColor, targetRectangleColor, t);

            yield return null;
        }

        // Ensure final values are set
        scrollingText.color = new Color(initialTextColor.r, initialTextColor.g, initialTextColor.b, 0f);
        startingRectangle.GetComponent<Renderer>().material.color = targetRectangleColor;

        titleCard.SetActive(true);
        Back.SetActive(true);
        Middle.SetActive(true);
        Front.SetActive(true);
        isTitleCardActive = true;
        isScrolling = false;

        if (isScrolling is false){
            startButton.gameObject.SetActive(true);
            settingsButton.gameObject.SetActive(true);
        }

        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
    }

    private void StartGame()
    {
        Debug.Log("Starting game...");
        player.transform.Rotate(0, 180, 0);
        Vector3 currentPosition = player.transform.position;
        player.transform.position = new Vector3(currentPosition.x, currentPosition.y + 2, currentPosition.z);
        playerCamera.transform.rotation = Quaternion.Euler(0, playerCamera.transform.rotation.eulerAngles.y, playerCamera.transform.rotation.eulerAngles.z);
        SceneManager.LoadScene("GameScene");
    }

    private void OpenSettings()
    {
        Debug.Log("Opening settings...");
        // Open settings menu
    }
}
