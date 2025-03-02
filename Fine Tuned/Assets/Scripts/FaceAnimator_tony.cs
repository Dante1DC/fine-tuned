using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FaceAnimator_tony : MonoBehaviour
{
    private Renderer faceRenderer;
    public Material sadFace;
    public Material happyFace;
    public Material idleFace;

    public GameObject face;

    public AudioClip fourthSong;

    public MusicHandler songHandler;

    // Reference to the Dialogue script
    private Dialogue dialogue;

    public GameObject panelToFill; // Change to a public GameObject variable for the panel

    // Start is called before the first frame update
    void Start()
    {
        // Find the Dialogue component on the same GameObject
        dialogue = GetComponent<Dialogue>();

        // Find the fcae GameObject and its renderer
        // heh heh. lmao, even!

        faceRenderer = face.GetComponent<MeshRenderer>();
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
            case 0: // Sad Face
                UpdateFace(idleFace);
                break;
            case 1: 
                UpdateFace(sadFace);
                break;
            case 2: // Idle Face
                UpdateFace(happyFace);
                break;
            case 3: // Idle Face
                UpdateFace(sadFace);
                break;
            case 4: // Idle Face
                UpdateFace(sadFace);
                break;
            case 5: // Fill panel with black for one second
                //FillPanelWithBlack();
                UpdateFace(happyFace);
                break;
            case 6: 
                UpdateFace(sadFace);
                break;
            case 7: 
                UpdateFace(idleFace);
                break;
            case 8: 
                UpdateFace(happyFace);
                break;
            case 9:
                SceneManager.LoadScene("AltmanOffice");
                songHandler.UpdateSong(fourthSong);
                break;
            default:
                UpdateFace(idleFace); // Default to idle face
                break;
        }
    }
}
