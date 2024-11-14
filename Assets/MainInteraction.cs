using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInteraction : MonoBehaviour
{
    public AllVariables allVariables;
    public GestureDetection gestureDetection;
    public int count = 0;

    public GameObject currentTargetCollided;

    private string hitTarget;
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
            if (currentTargetCollided.GetComponent<TargetScript>().IsCollidingWithCursor)
            {
                currentTargetCollided.GetComponent<TargetScript>().IsCollidingWithCursor = false;
                effectiveWidth = Vector3.Distance(allVariables.cursor.transform.position, allVariables.currentTarget.transform.position);
                hitTarget = ""+ allVariables.fittsLawExperiment.currentTargetNumber; 
                allVariables.fittsLawExperiment.currentTargetNumber = -1;
                //currentTargetCollided.GetComponent<TargetScript>().MychangeColor("#FFFFFF49"); //turn it back to white after selection
                CorrectHIT();
                
            }


            else
            {
                allVariables.errorCount =  allVariables.errorCount + 1;

                //TODO: Error
                //
                //}

            }
        }

    }

    private void CorrectHIT()
    {
        //allVariables.placeTarget.UpdateTargetPositionandSize(1.0f, 0.40f);

        if (allVariables.fittsLawExperiment.targetIndex != 1)
        {
            logData();
        }
        else
        {
            allVariables.timer = 0;
            allVariables.errorCount = 0;
        }

        allVariables.experimentController.OnTargetSelected();
      

    }


    public float effectiveWidth;


    private void logData()
    {
        System.DateTime epochStart = new System.DateTime(2024, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        double cur_time = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
        long key = Convert.ToInt64(cur_time);

        //string platform = "Physical";
        string TargetNumber = hitTarget;
        string width = "" + allVariables.currentWidth;
        string amplitude = "" + allVariables.currentAmplitude;
        string movementTime = "" + allVariables.timer;
       // string window_size = "" + "no";

        string misclick = "" + allVariables.errorCount;
        string technique = "" + allVariables.pointingTechnique;

        string participant = allVariables.participantName;

        // float effectiveDistance = Vector3.Distance(allVariables.cursor, allVariables.);
        string EndpointDeviation = ""+effectiveWidth;



        //Vector3 OriginRotationVector = new Vector3(0, 0, 1);
        //Vector3 raydirection = Optifwd;
        //float angleInDegrees = Vector3.SignedAngle(OriginRotationVector, raydirection, Vector3.up);
        //string ExtraHandrotation = "" + Math.Round(angleInDegrees, 2);

        //Vector3 newraydirection = HeadPoint.transform.TransformDirection(Vector3.forward); ;
        //float HeadDegrees = Vector3.SignedAngle(OriginRotationVector, newraydirection, Vector3.up);

        //string ExtraHeadrotation = "" + Math.Round(HeadDegrees, 2);
        //string ExtraAngularWidth = "" + WidthDict[WidthArr[DbVemsWidth]];
        //string ExtraAngularAmplitude = "" + AmplitudeDict[distArr[DbVemsDistance]];
        ////string logStr =platform +", "+DBparticipantID + ", " + width + ", " + amplitude + ", " + movementTime + ", " + continuousError + ", " + misclick + ", "+ ExtraHeadrotation+ ", "+ ExtraHandrotation;
        //string DBIntearctionDistance = "" + DBplayerDistanceMultiplier;


        DateTime theTime = DateTime.Now;
        string datetime = theTime.ToString("yyyy-MM-dd\\THH:mm:ss\\Z");
        string logStr = datetime + "," + allVariables.fittsLawExperiment.targetIndex + "," + participant + "," + technique + "," + amplitude + "," + width + ","+ TargetNumber + "," + EndpointDeviation + ","+ misclick + "," + movementTime;




      
        StartCoroutine(allVariables.firebaseStuff.LogDetails( logStr, "" + key));
        allVariables.timer = 0;
        allVariables.errorCount = 0;



    }









    }
