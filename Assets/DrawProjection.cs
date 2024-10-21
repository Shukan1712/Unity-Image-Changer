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

    public AllVariables allVariables;

    public LayerMask CollidableLayes;
   // public DavidCustomGrabbingScript CGS;




    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
 


    }




    public void DoRaycast()
    {
        Vector3 fwd = ControllerToFollow.transform.TransformDirection(Vector3.left) * 10; 

        Vector3 startingPoint = ControllerToFollow.transform.position;
        if ("" + allVariables.pointingTechnique == "RAYCASTWRIST")
        {
             fwd = ControllerToFollow.transform.TransformDirection(Vector3.left) * 10;
        }

         if("" + allVariables.pointingTechnique == "RAYCASTHEAD")
        {
             fwd = ControllerToFollow.transform.TransformDirection(Vector3.forward) * 10;
        }


        RaycastHit hit;
        if (Physics.Raycast(startingPoint, fwd, out hit, 100000f, CollidableLayes))
        {


            Cursor.transform.position = hit.point;
        }



    }
}
