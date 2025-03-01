using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour, IInteractable
{
    // Expected dialogue file format (JSON):
    // {
    //   "dialogueLines": [
    //     "Line 1 of dialogue",
    //     "Line 2 of dialogue",
    //     "etc..."
    //   ],
    //   "dialogueName": "Character Name"
    // }
    [SerializeField] private TextAsset dialogueFile;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private string dialogueFileName;
    [SerializeField] private float typingSpeed = 0.05f; // Adjust this value to change typing speed
    [SerializeField] private Transform headTransform; // Reference to the "head" part of the object

    private string[] dialogueLines;
    private string speakerName;
    private int currentLine = 0;
    private bool isInDialogue = false;
    private Coroutine typingCoroutine;
    private bool isTextFullyDisplayed = false;
    private Transform cameraTransform;

    void Start()
    {
        if (dialoguePanel) dialoguePanel.SetActive(false);
        if (namePanel) namePanel.SetActive(false);

        if (!string.IsNullOrEmpty(dialogueFileName))
        {
            Debug.Log("Attempting to load file: " + dialogueFileName);
            TextAsset dialogueFile = Resources.Load<TextAsset>(dialogueFileName);
            if (dialogueFile != null)
            {
                DialogueData data = JsonUtility.FromJson<DialogueData>(dialogueFile.text);
                dialogueLines = data.dialogueLines;
                speakerName = data.dialogueName;
            }
            else
            {
                Debug.LogWarning("Dialogue file not found: " + dialogueFileName);
            }
        }

        // Find the main camera's transform
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (isInDialogue && headTransform != null && cameraTransform != null)
        {
            // Make the head look at the camera
            headTransform.LookAt(cameraTransform);
        }
    }

    public void Interact()
    {
        if (!isInDialogue)
        {
            // Start dialogue
            Debug.Log("Starting dialogue");
            StartDialogue();
        }
        else
        {
            if (isTextFullyDisplayed)
            {
                // Advance to next line
                Debug.Log("Advancing to next line");
                NextLine();
            }
            else
            {
                // Complete the current line immediately
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                dialogueText.text = dialogueLines[currentLine];
                isTextFullyDisplayed = true;
            }
        }
    }

    private void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogWarning("No dialogue lines found!");
            return;
        }

        isInDialogue = true;
        currentLine = 0;
        
        if (dialoguePanel) dialoguePanel.SetActive(true);
        if (namePanel) namePanel.SetActive(true);
        
        if (nameText && !string.IsNullOrEmpty(speakerName))
        {
            nameText.text = speakerName;
        }
        
        DisplayCurrentLine();
    }

    private void NextLine()
    {
        currentLine++;
        if (currentLine >= dialogueLines.Length)
        {
            // End of dialogue
            EndDialogue();
        }
        else
        {
            DisplayCurrentLine();
        }
    }

    private void DisplayCurrentLine()
    {
        if (dialogueText != null && currentLine < dialogueLines.Length)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeText(dialogueLines[currentLine]));
        }
    }

    private IEnumerator TypeText(string line)
    {
        dialogueText.text = "";
        isTextFullyDisplayed = false;

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTextFullyDisplayed = true;
    }

    private void EndDialogue()
    {
        isInDialogue = false;
        if (dialoguePanel) dialoguePanel.SetActive(false);
        if (namePanel) namePanel.SetActive(false);

        // Optionally, reset the head's rotation here if needed
        // headTransform.rotation = Quaternion.identity; // Reset to original rotation
    }
}

[System.Serializable]
public class DialogueData
{
    public string[] dialogueLines;
    public string dialogueName;
}
