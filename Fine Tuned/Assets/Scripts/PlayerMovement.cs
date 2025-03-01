using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This script is used to move the player around the map using the arrow keys
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Lock rotation and vertical movement
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // Get rotation input from horizontal axis (left/right arrows)
        float rotationInput = horizontalInput;
        transform.Rotate(Vector3.up * rotationInput * moveSpeed * 50f * Time.deltaTime);
        
        // Only use vertical input (up/down arrows) for forward/back movement
        horizontalInput = 0f;

        // Only move in X and Z directions
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
