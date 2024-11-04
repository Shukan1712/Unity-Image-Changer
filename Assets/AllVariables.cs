using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllVariables : MonoBehaviour
{
 
    public DrawProjection drawProjectionWrist;
    public DrawProjection drawProjectionHead;
    public DrawProjection drawProjectionArm;
    public DrawProjection drawProjectionFinger;
    public DrawProjection drawProjectionEyes;




    public wristRotationScript wristRotationScript;
    public cursorMovementScript cursorMovementScript;
    public PlaceTarget placeTarget;
    public MainInteraction mainInteraction;
    public FittsLawExperiment fittsLawExperiment;
    public ExperimentController experimentController;
    public FirebaseStuff firebaseStuff;

   

    public enum PointingTechnique
    {
        HAND,
        CONTROLLER,
        HEAD, RAYCASTWRIST, RAYCASTHEAD, RAYCASTFINGER, RAYCASTARM, RAYCASTEYES,
        EYES
    };


    public string participantName = "Test";
    public float timer = 0;
    public int errorCount = 0;

    public PointingTechnique pointingTechnique;
    public GameObject Target;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

    }
}
