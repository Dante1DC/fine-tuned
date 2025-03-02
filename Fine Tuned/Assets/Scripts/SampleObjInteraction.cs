using UnityEngine;

public class SampleObjInteraction : MonoBehaviour, IInteractable
{
    private Renderer cubeRenderer;

    void Start()
    {
        // Get the Renderer component of the cube
        cubeRenderer = GetComponent<Renderer>();
    }

    public void Interact()
    {
        // Change the color of the cube to green
        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = Color.green;
        }
    }
}