using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wristRotationScript : MonoBehaviour
{
    public AllVariables allVariables;

    [Header("Ray Origin")]
    [SerializeField] public OVRSkeleton pointingHand;





    private Transform wristTransform;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCubePositionandRotationBasedonWrist();
    }


    public Transform getWristTransform()
    {
        foreach (var b in pointingHand.Bones)
        {
            if (b.Id == OVRSkeleton.BoneId.Hand_WristRoot) return b.Transform;
        }
        return null;
    }




    void UpdateCubePositionandRotationBasedonWrist()
    {
        wristTransform = getWristTransform();

        if (wristTransform != null)
        {

            // Get the wrist's rotation
            Quaternion wristRotation = wristTransform.rotation;

            // Optionally, adjust the rotation if needed (e.g., apply an offset)

            // Apply the wrist rotation to the current GameObject
            transform.rotation = wristRotation;

            // If you also want to set the position based on the wrist
            transform.position = wristTransform.position;
        }
    }

}
