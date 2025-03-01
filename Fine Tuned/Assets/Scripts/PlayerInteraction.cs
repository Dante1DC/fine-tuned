using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define the interface for interactable objects
public interface IInteractable
{
    void Interact();
}

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f; // Range within which player can interact with objects
    private Camera playerCamera; // Reference to the player's camera

    void Start()
    {
        // Get the camera that's attached to the player
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        // Check for E key press
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        // Use camera's position and forward direction for the ray
        Ray interactionRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Check if ray hits something within range
        if (Physics.Raycast(interactionRay, out hit, interactionRange))
        {
            // Check if hit object has Interactable tag
            if (hit.collider.CompareTag("Interactable"))
            {
                // Get any interaction component and trigger it
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }
}
