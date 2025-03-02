using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnimator : MonoBehaviour
{

    private readonly GameObject face;
    private Renderer faceRenderer;
    public Material sadFace;
    public Material happyFace;
    public Material idleFace;


    // Start is called before the first frame update
    void Start()
    {
        faceRenderer = face.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateFace(Material newFace)
    {
        faceRenderer.material = newFace;
    }
}
