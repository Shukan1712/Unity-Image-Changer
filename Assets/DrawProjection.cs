using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawProjection : MonoBehaviour
{
    //LineRenderer lineRenderer;
    public GameObject ControllerToFollow;
    public GameObject Cursor;
    public int numPoints = 5;
    public float timeBetweenPoints = 0.1f;
    public OVRSkeleton pointingHand;
    public OVRSkeleton LeftHand;
    public AllVariables allVariables;

    public LayerMask CollidableLayes;
    // public DavidCustomGrabbingScript CGS;


    private float lastClickTime = 0f;
    private float clickCooldown = 0.5f; // seconds



    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    public void Update()
    {

        Cursor.transform.position = GetIndexFingerTransform().position;
        
        CheckFingerRaycast();
        

    }

    void CheckFingerRaycast()
    {
        if (Time.time - lastClickTime < clickCooldown) return;

        Transform fingerTip = GetIndexFingerTransform();
        if (fingerTip == null) return;

        Ray ray = new Ray(fingerTip.position, fingerTip.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f, CollidableLayes))
        {
            if (hit.transform.name == "NextButton")
            {
                FindObjectOfType<ExperimentController>().ShowNextMaterial();
                lastClickTime = Time.time;
            }
            else if (hit.transform.name == "PrevButton")
            {
                FindObjectOfType<ExperimentController>().ShowPreviousMaterial();
                lastClickTime = Time.time;
            }
        }
    }


    public Transform GetIndexFingerTransform()
    {
        foreach (var b in pointingHand.Bones)
        {
            if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip) return b.Transform;
        }
        return null;
    }


    public Transform GetIndexFingerTransform(OVRSkeleton handSkeleton)
    {
        foreach (var bone in handSkeleton.Bones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
            {
                return bone.Transform;
            }
        }
        return null;
    }

    public Transform GetArmTransform()
    {
        foreach (var b in LeftHand.Bones)
        {
            if (b.Id == OVRSkeleton.BoneId.Hand_ForearmStub) return b.Transform;
        }
        return null;
    }


    //public void DoRaycast()
    //{
    //    Vector3 startingPoint = Vector3.zero;
    //    Vector3 fwd = Vector3.zero;


    //    if ("" + allVariables.pointingTechnique == "RAYCASTFINGER")
    //    {
    //        Transform indexFinger = GetIndexFingerTransform();
    //        startingPoint = indexFinger.position;
    //        fwd = indexFinger.TransformDirection(Vector3.right) * 10;
    //    }
    //    else if ("" + allVariables.pointingTechnique == "RAYCASTARM")
    //    {
    //        Transform arm = GetArmTransform();
    //        startingPoint = arm.position;
    //        fwd = arm.TransformDirection(Vector3.right) * 10;
    //    }

    //    else if ("" + allVariables.pointingTechnique == "RAYCASTEYES")
    //    {
    //        startingPoint = ControllerToFollow.transform.position;
    //        fwd = ControllerToFollow.transform.TransformDirection(Vector3.forward) * 10;

    //    }

    //    else
    //    {
             

    //         startingPoint = ControllerToFollow.transform.position;
    //        if ("" + allVariables.pointingTechnique == "RAYCASTWRIST")
    //        {
    //            fwd = ControllerToFollow.transform.TransformDirection(Vector3.left) * 10;
    //        }

    //        if ("" + allVariables.pointingTechnique == "RAYCASTHEAD")
    //        {
    //            fwd = ControllerToFollow.transform.TransformDirection(Vector3.forward) * 10;
    //        }
    //    }

    //    RaycastHit hit;
    //    if (Physics.Raycast(startingPoint, fwd, out hit, 100000f, CollidableLayes))
    //    {


    //        Cursor.transform.position = hit.point;
    //    }



    //}
}













