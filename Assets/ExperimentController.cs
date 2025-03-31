using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ExperimentController : MonoBehaviour
{
   public FittsLawExperiment fittsLawExperiment;
    public AllVariables allVariables;
    [SerializeField] private Transform wristTransform;


    public int setcount = 0;

    public DrawProjection drawProjectionFinger;

    public bool completed = false;
    public bool restart = false;
    public bool work = false;

    // Arrays for amplitudes and widths
    //public float[] amplitudeArray = { 0.05f, 0.05f, 0.03f };
    //public float[] widthArray = { 0.01f,0.02f, 0.005f  };

    public float[] amplitudeArray = { 0.05f, 0.07f };
    public float[] widthArray = { 0.01f, 0.015f  };

    public GameObject screen;
    // List of center points
    private List<Vector3> centerPoints = new List<Vector3>
    {
    
         new Vector3(0f, 0f, 0f),
     

    };

   

    public float amplitude = 4f;
    public float width = 0.2f;
   // public Vector3 centerPosition = new Vector3(0.47f, 2.36f, 2f);

    private int currentTargetNumber;

    private void Update()
    {
        float extraX = 0f;
        float extraY = 0f;
        float extraZ = 0f;


        if (restart == true)
        {
            restart = false;
            StartCoroutine(RunExperiment());
            
        }

        if (work == true)
        {

            

            if (allVariables.remoteState == 1) // front palm small
            {
                // this sets x , y , z 
                extraX = 0.071f;
                extraY = 0.040f;
                extraZ = 0.075f;

                // this sets scale( Size) of the screen 
                screen.transform.localScale = new Vector3(0.09f, 0.09f, 0.000001f);


            }
            else if (allVariables.remoteState == 2) // front palm big 
            {
                extraX = 0.071f;
                extraY = 0.055f;
                extraZ = 0.071f;


                // this sets scale( Size) of the screen 
                screen.transform.localScale = new Vector3(0.09f *2, 0.09f * 2, 0.000001f * 2);

            }
            else if (allVariables.remoteState == 3) //Hand small
            {
                extraX = -0.001f;
                extraY = 0.025f;
                extraZ = -0.051f;

                // this sets scale( Size)
                screen.transform.localScale = new Vector3(0.09f, 0.09f, 0.000001f);
               // screen.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);


            }
            else if (allVariables.remoteState == 4) // Hand Big
            {

                extraX = -0.001f;
                extraY = 0.025f;
                extraZ = -0.051f;

                // this sets scale( Size)
                screen.transform.localScale = new Vector3(0.09f * 2, 0.09f * 2, 0.000001f * 2);


            }
            else if (allVariables.remoteState == 5) 
            {

                extraX = 0.041f;
                extraY = 0.100f;
                extraZ = 0.075f;

                // this sets scale( Size)
                screen.transform.localScale = new Vector3(0.1f, 0.1f, 0.00001f);


            }
            else if (allVariables.remoteState == 6)
            {

                extraX = -0.071f;
                extraY = 0.100f;
                extraZ = -0.071f;

                // this sets scale( Size)
                screen.transform.localScale = new Vector3(0.1f, 0.1f, 0.00001f);


            }







            Vector3 newposition = drawProjectionFinger.GetArmTransform().position;
            print(extraX);
                screen.transform.position = new Vector3(newposition.x + extraX, newposition.y + extraY, newposition.z + extraZ); //sukhan change 


            Vector3 newPosition = wristTransform.position;
            Quaternion newRotation = wristTransform.rotation;


            if (allVariables.remoteState == 1|| allVariables.remoteState == 2|| allVariables.remoteState == 3 || allVariables.remoteState == 4)
            {
                screen.transform.rotation = wristTransform.rotation * Quaternion.Euler(90, 90, 0);
            }
            else if (allVariables.remoteState == 5)
            {
                screen.transform.rotation = wristTransform.rotation * Quaternion.Euler(0, 90, 0);
            }
            else if (allVariables.remoteState == 6)
            {
                screen.transform.rotation = wristTransform.rotation * Quaternion.Euler(90, 90, 0);
            }




        }



    }



    void Start()
    {

        StartCoroutine(RunExperiment());
        //resetScenario();

        // Arrange the targets around the circle
        //fittsLawExperiment.ArrangeTargets(amplitude, width, centerPosition);

        // Initialize the first target selection
        //if (fittsLawExperiment.GetNextTarget())
        //{
        //    // Activate or highlight the current target
        //    ActivateTarget(currentTargetNumber);
        //}
    }


    IEnumerator RunExperiment()
    {
        completed = false;
        // Randomly shuffle the center points
        ShuffleList(centerPoints);

        foreach (Vector3 centerPosition in centerPoints)
        {
            // Iterate over all combinations of amplitudes and widths
            foreach (float amplitude in amplitudeArray)
            {
                foreach (float width in widthArray)
                {
                    work = false;
                    // we need to detach all children from   the current gameobject   (if any)
                    // Detach all children (if any)
                    fittsLawExperiment.DetachAllChildren();
                    yield return new WaitForSeconds(0.3f);
                    
                    //yield return new WaitForSeconds(1f);
                    screen.transform.rotation = Quaternion.Euler(0, 0, 0);
                    //screen.transform.position = new Vector3(0.47f, 0,0);
                    screen.transform.position = new Vector3(0f, 0f, 0f);
                    yield return new WaitForSeconds(0.1f);






                    if (allVariables.remoteState == 1 || allVariables.remoteState == 3) // remoteState 1 & 3 is small
                    {
                        // sets how much big the target will be...
                        fittsLawExperiment.ArrangeTargets(amplitude, width, centerPosition);


                    }
                    else if (allVariables.remoteState == 2 || allVariables.remoteState == 4) // remoteState 2 & 4 is big
                    {
                        // sets how much big the target will be...
                        fittsLawExperiment.ArrangeTargets(amplitude * 2, width * 2, centerPosition);
                    }
                    else if (allVariables.remoteState == 5 || allVariables.remoteState == 6) // remoteState 5 & 6 is??
                    {
                        // sets how much big the target will be...
                        fittsLawExperiment.ArrangeTargets(amplitude , width , centerPosition);
                       

                    }

                        // fittsLawExperiment.ArrangeTargets(amplitude / 2, width / 2, centerPosition);

                        yield return new WaitForSeconds(0.2f);
                    fittsLawExperiment.AttachTargetsAsChildren();
                    yield return new WaitForSeconds(0.2f);



                    if (allVariables.remoteState==1)
                    {
                        screen.transform.rotation = Quaternion.Euler(0, 0, 0); // sukhan change here 
                    }
                    else if (allVariables.remoteState == 2)
                    {
                        screen.transform.rotation = Quaternion.Euler(0, 0, 0); // sukhan change here 
                    }
                    else if (allVariables.remoteState == 3)
                    {
                        screen.transform.rotation = Quaternion.Euler(00, 0, 0); // sukhan change here 
                    }

                  
                    yield return new WaitForSeconds(0.1f);
                    //yield return new WaitForSeconds(1f);
                    work = true;


                   






                    // Reset the target selection sequence
                    fittsLawExperiment.ResetTargetSequence();

                    // Initialize the first target selection
                    if (fittsLawExperiment.GetNextTarget())
                    {
                        // Activate or highlight the current target
                        ActivateTarget(currentTargetNumber);
                    }

                    // Wait for the target selection sequence to complete
                    while (!fittsLawExperiment.SequenceComplete)
                    {
                        yield return null;
                    }

                    // Optionally, collect and record data here
                    // ...

                    // Reset the scenario if needed
                    //ResetScenario();
                }
            }
        }

        allVariables.sfxPlaying.PlayFinished();

        Debug.Log("Experiment Complete");
    }

    void ActivateTarget(int targetNumber)
    {
        // Implement your logic to activate or highlight the target

       // fittsLawExperiment.targets[targetNumber].GetComponent<TargetScript>().MychangeColor("#FF000049");  //redcolor

        //does nothing   GetNextTarget() does everything. 

        Debug.Log("Current Target: " + targetNumber);
    }

    // Call this method when the user selects the current target
    public void OnTargetSelected()
    {
        if (fittsLawExperiment.GetNextTarget())
        {
            // Activate or highlight the next target
            ActivateTarget(currentTargetNumber);
        }
        else
        {
            // Sequence is complete
           fittsLawExperiment. SequenceComplete = true;
        }
    }





    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        System.Random rnd = new System.Random();

        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);

            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
