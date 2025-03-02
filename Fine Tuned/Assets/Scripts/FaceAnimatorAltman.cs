using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaceAnimatorAltman : MonoBehaviour
{
    private Renderer faceRenderer;
    public Material sadFace;
    public Material happyFace;
    public Material idleFace;

    public GameObject face;

    // Reference to the Dialogue script
    private Dialogue dialogue;

    public GameObject canvas;

    public AudioClip audioClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Find the Dialogue component on the same GameObject
        dialogue = GetComponent<Dialogue>();

        // Find the fcae GameObject and its renderer
        // heh heh. lmao, even!

        faceRenderer = face.GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFaceBasedOnDialogue();
    }

    void UpdateFace(Material newFace)
    {
        var new_materials = faceRenderer.materials;
        new_materials[0] = newFace;
        faceRenderer.materials = new_materials;
        
    }

    void UpdateFaceBasedOnDialogue()
    {
        // Assuming dialogue.currentOptionIndex gives the current dialogue option index
        
        int currentOptionIndex = dialogue.GetCurrentOptionIndex();
        // Update the face based on the current dialogue option
        switch (currentOptionIndex)
        {
            case 8: // empty
                SceneManager.LoadScene("Minigame");
                break;
            default:
                UpdateFace(idleFace); // Default to idle face
                break;
        }
    }
}
