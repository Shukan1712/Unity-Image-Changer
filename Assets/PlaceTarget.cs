using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// x max is 2.77
// x min is -1.847
// y max is 2.187
// y min is 0.56

public class PlaceTarget : MonoBehaviour
{
    // Fixed variables
    private const float x_min = -1.847f;
    private const float x_max = 2.77f;
    private const float y_min = 0.56f;
    private const float y_max = 2.187f;
    // Start is called before the first frame update
    public LayerMask cursorLayer;
    public bool IsCollidingWithCursor { get; private set; }

    public AllVariables allVariables;




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public Vector2 GetNextTarget(float diagonalDistance, float targetSize)
    {
        // Use the current transform position as the center
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

        while (true)
        {
            // Generate a random angle in radians between 0 and 2π
            float angle = Random.Range(0f, 2f * Mathf.PI);

            // Compute new position
            float x_new = currentPosition.x + diagonalDistance * Mathf.Cos(angle);
            float y_new = currentPosition.y + diagonalDistance * Mathf.Sin(angle);

            // Check if new position is within bounds, adjusting for target size
            if (x_new - targetSize / 2f >= x_min && x_new + targetSize / 2f <= x_max &&
                y_new - targetSize / 2f >= y_min && y_new + targetSize / 2f <= y_max)
            {
                // Valid position found
                return new Vector2(x_new, y_new);
            }
            // If not valid, loop again to find a new position
        }
    }






    public void OnTriggerStay(Collider other)
    {
        // Check if the other collider's GameObject is on the cursor layer
        if (IsInLayerMask(other.gameObject, cursorLayer))
        {
            IsCollidingWithCursor = true;
            Debug.Log("HITTTTTTT");
        }
        else
        {
            IsCollidingWithCursor = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // Reset the collision flag when the other object exits the trigger
        if (IsInLayerMask(other.gameObject, cursorLayer))
        {
            IsCollidingWithCursor = false;
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }




    public void UpdateTargetPositionandSize(float diagonalDistance, float targetSize )
    {
        // Get the next target position (x and y)
        Vector2 nextPosition = GetNextTarget(diagonalDistance, targetSize);

        // Keep the z-value the same as the current Target position
        float currentZ = transform.position.z;

        // Set the new position of the Target
        transform.position = new Vector3(nextPosition.x, nextPosition.y, 2.424f);
        Debug.Log("Z is " + transform.position.z);


        transform.localScale = new Vector3(targetSize, targetSize, targetSize);


    }








}
