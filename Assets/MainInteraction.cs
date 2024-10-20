using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInteraction : MonoBehaviour
{
    public AllVariables allVariables;
    public GestureDetection gestureDetection;
    public int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        
        if(gestureDetection.ConfirmationDetected())
        {
            Debug.Log("button pressed "+count);
            count++;
            if (allVariables.placeTarget.IsCollidingWithCursor)
            {
                CorrectHIT();
                
            }


            else
            { 
                
                //TODO: Error
              //
              //}

            }
        }

    }

    private void CorrectHIT()
    {
       allVariables.placeTarget.UpdateTargetPositionandSize(1.0f, 0.40f);
    }
}
