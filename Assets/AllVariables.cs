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
    public SFXplaying sfxPlaying;
   

    public enum PointingTechnique
    {
        
       RAYCASTWRIST, RAYCASTHEAD, RAYCASTFINGER, RAYCASTARM, RAYCASTEYES
    
    };


    public string participantName = "Test";
    public float timer = 0;
    public int errorCount = 0;
    public float currentWidth;
    public float currentAmplitude;
    public float currentDepth; 
    public GameObject currentTarget;
    public GameObject previousTarget;
    public GameObject cursor; 
    public PointingTechnique pointingTechnique;
    public GameObject Target;
    public float effectiveWidth;
    public bool initialTouchDone;
    public float initialTouchTimer;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (initialTouchDone == false)
        { 
            initialTouchTimer += Time.deltaTime;
        }
    }
}
