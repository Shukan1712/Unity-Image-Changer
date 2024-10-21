using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class cursorMovementScript : MonoBehaviour
{

    public AllVariables allVariables;






    [Header("Sphere Movement Parameters")]
    public Transform planeTransform; // Reference to the plane (flat cube)
    public float planeWidth = 5.0f;  // Width of the plane along x-axis
    public float planeHeight = 2.0f; // Height of the plane along y-axis

    [Header("Scaling Factors")]
    public float angleToPositionScaleX = 1f; // Scaling factor for x (left-right movement)
    public float angleToPositionScaleY = 1f; // Scaling factor for y (up-down movement)

    [Header("Position Limits")]
    public float positionLimitX = 5f; // Limit for x position (half of planeWidth)
    public float positionLimitY = 2.0f; // Limit for y position (half of planeHeight)




    public TextMeshPro DebugText;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ("" + allVariables.pointingTechnique == "HAND")
        {
            UpdateSpherePositionBasedOnWristRotation();


        }

        else if ("" + allVariables.pointingTechnique == "HEAD")
        {

            UpdateSpherePositionBasedOnHeadRotation();
        }

        else if ("" + allVariables.pointingTechnique == "CONTROLLER")
        {
            allVariables.drawProjection.DoRaycast();
        
        }

       
    }






    void UpdateSpherePositionBasedOnHeadRotation()
    {
        // Get the current rotation of the GameObject (which matches the wrist rotation)
        Quaternion rotation = allVariables.wristRotationScript.transform.rotation;

        // Convert the rotation to Euler angles
        Vector3 eulerAngles = rotation.eulerAngles;

        // Adjust Euler angles to range from -180 to 180 degrees
        float yaw = eulerAngles.y;   // Left/Right movement (x-axis position)
        float roll = eulerAngles.z;  // Up/Down movement (y-axis position)
        float pitch = eulerAngles.x;





        // Define the angle ranges based on your specifications
        float pitchMinAngle = -40f; // Left
        float pitchMaxAngle = 0f;  // Right

        float rollMinAngle = -35f;  // Down
        float rollMaxAngle = 90f;   // Up

        // Normalize the angles to a 0-1 range
        float normalizedPitch = Mathf.InverseLerp(pitchMinAngle, pitchMaxAngle, pitch);
        float normalizedRoll = Mathf.InverseLerp(rollMinAngle, rollMaxAngle, roll);

        // Map the normalized values to position limits
        float xPosition = Mathf.Lerp(-positionLimitX, positionLimitX, normalizedPitch);
        float yPosition = Mathf.Lerp(-positionLimitY, positionLimitY, normalizedRoll);

        // Optionally invert axes if movement is opposite
        // xPosition *= -1;
        // yPosition *= -1;

        // Set the sphere's position relative to the plane
        transform.position = planeTransform.position + new Vector3(xPosition, yPosition, 0f);



        DebugText.text = "yaw:" + yaw + "\nroll:" + roll + "\nPitch:" + pitch;
    }
























    /// <summary>
    /// ////////////////////////////////////////
    /// </summary>

    void UpdateSpherePositionBasedOnWristRotation()
    {
        // Get the current rotation of the GameObject (which matches the wrist rotation)
        Quaternion rotation = allVariables.wristRotationScript.transform.rotation;

        // Convert the rotation to Euler angles
        Vector3 eulerAngles = rotation.eulerAngles;

        // Adjust Euler angles to range from -180 to 180 degrees
        float yaw = NormalizeAngle(eulerAngles.y);   // Left/Right movement (x-axis position)
        float roll = NormalizeZAngle(eulerAngles.z);  // Up/Down movement (y-axis position)
        float pitch = NormalizePitchAngle(eulerAngles.x);





        // Define the angle ranges based on your specifications
        float pitchMinAngle = -40f; // Left
        float pitchMaxAngle = 5f;  // Right

        float rollMinAngle = -35f;  // Down
        float rollMaxAngle = 90f;   // Up

        // Normalize the angles to a 0-1 range
        float normalizedPitch = Mathf.InverseLerp(pitchMinAngle, pitchMaxAngle, pitch);
        float normalizedRoll = Mathf.InverseLerp(rollMinAngle, rollMaxAngle, roll);

        // Map the normalized values to position limits
        float xPosition = Mathf.Lerp(-positionLimitX, positionLimitX, normalizedPitch);
        float yPosition = Mathf.Lerp(-positionLimitY, positionLimitY, normalizedRoll);

        // Optionally invert axes if movement is opposite
        // xPosition *= -1;
        // yPosition *= -1;

        // Set the sphere's position relative to the plane
        transform.position = planeTransform.position + new Vector3(xPosition, yPosition, 0f);



        DebugText.text = "yaw:"+yaw + "\nroll:"+roll + "\nPitch:" + pitch;
    }


    // Helper method to normalize angles to the range -180 to 180 degrees
    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f)
            return angle - 360f;
        else if (angle < -180f)
            return angle + 360f;
        else
            return angle;
    }


    float NormalizeZAngle(float angle)
    {
        
        
            return   - (angle - 180f);
   
    }


    float NormalizePitchAngle(float angle)
    {
        if (angle > 180)
            return -(360 - angle);
        else
            return angle;
    
    
    }





}
