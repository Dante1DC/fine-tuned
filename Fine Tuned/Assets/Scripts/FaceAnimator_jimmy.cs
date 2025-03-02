using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaceAnimator_jimmy : MonoBehaviour
{
    private Renderer faceRenderer;
    public Material sadFace;
    public Material happyFace;
    public Material idleFace;

    public GameObject face;

    // Reference to the Dialogue script
    private Dialogue dialogue;

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
                UpdateFace(sadFace);
                break;
            case 1: 
                UpdateFace(idleFace);
                break;
            case 2: // Idle Face
                UpdateFace(idleFace);
                break;
            case 3: // Idle Face
                UpdateFace(sadFace);
                break;
            case 4: // Idle Face
                UpdateFace(sadFace);
                break;
            case 5: // Idle Face
                UpdateFace(happyFace);
                break;
            case 6: // Idle Face
                UpdateFace(happyFace);
                break;
            case 7: // empty
                UpdateFace(happyFace);
                SceneManager.LoadScene("Minigame");
                break;
            default:
                UpdateFace(idleFace); // Default to idle face
                break;
        }
    }
}
