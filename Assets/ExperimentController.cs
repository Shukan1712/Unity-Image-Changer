using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ExperimentController : MonoBehaviour
{
   public FittsLawExperiment fittsLawExperiment;
    public AllVariables allVariables;


    public DrawProjection drawProjectionFinger;

    public bool completed = false;
    public bool restart = false;
    public bool work = false;

    // Arrays for amplitudes and widths
    public float[] amplitudeArray = { 0.1f, 0.2f, 0.05f };
    public float[] widthArray = { 0.01f,0.02f, 0.005f  };
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
        if (restart == true)
        {
            restart = false;
            StartCoroutine(RunExperiment());
            
        }

        if (work == true)
        {

            Vector3 newposition = drawProjectionFinger.GetArmTransform().position;


            screen.transform.position = new Vector3(newposition.x, newposition.y+0.035f, newposition.z); //sukhan change 





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
                    fittsLawExperiment.ArrangeTargets(amplitude, width, centerPosition);
                    yield return new WaitForSeconds(0.2f);
                    fittsLawExperiment.AttachTargetsAsChildren();
                    yield return new WaitForSeconds(0.2f);
                    screen.transform.rotation = Quaternion.Euler(90, 0, 0); // sukhan change here 
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
