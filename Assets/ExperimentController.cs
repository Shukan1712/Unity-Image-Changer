using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ExperimentController : MonoBehaviour
{


    // SHukan chned here on 1st april

    public Material[] materials;            // Assign these in Inspector
    public Renderer screenRenderer;         // Assign your screen's MeshRenderer
    private int currentMaterialIndex = 0;




    public FittsLawExperiment fittsLawExperiment;
    public AllVariables allVariables;
    [SerializeField] private Transform wristTransform;
    [SerializeField] private Transform fingerTipTransform;
    [SerializeField] private OVRSkeleton rightHandSkeleton;
    [SerializeField] private OVRSkeleton leftHandSkeleton;



    public int setcount = 0;

    public DrawProjection drawProjectionFinger;

    public bool completed = false;
    public bool restart = false;
    public bool work = false;

    // Arrays for amplitudes and widths
    //public float[] amplitudeArray = { 0.05f, 0.05f, 0.03f };
    //public float[] widthArray = { 0.01f,0.02f, 0.005f  };

    public float[] amplitudeArray = { 0.04f, 0.06f };
    public float[] widthArray = { 0.005f,0.010f  };

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
            //StartCoroutine(RunExperiment());
            
        }

        if (work == true)
        {

            

            if (allVariables.remoteState == 1) // front palm small
            {
                // this sets x , y , z 
                extraX = 0.071f;
                extraY = 0.030f;
                extraZ = 0.005f;

                // this sets scale( Size) of the screen 
                screen.transform.localScale = new Vector3(0.050f*1.5f, 0.060f, 0.000001f);


            }
            else if (allVariables.remoteState == 2) // front palm big 
            {
                //extraX = 0.071f;
                //extraY = 0.055f;
                //extraZ = 0.071f;

                extraX = 0.071f;
                extraY = -0.035f;
                extraZ = 0.005f;


                // this sets scale( Size) of the screen 

                screen.transform.localScale = new Vector3(0.050f * 1.5f, 0.060f, 0.000001f);


            }
            else if (allVariables.remoteState == 3) //Hand small
            {
                extraX = -0.041f;
                extraY = 0.035f;
                extraZ = -0.001f;

                // this sets scale( Size) of the screen 
                screen.transform.localScale = new Vector3(0.050f * 1.5f, 0.060f, 0.000001f);


            }
            else if (allVariables.remoteState == 4) // Hand Big
            {

                extraX = -0.031f;
                extraY = -0.045f;
                extraZ = -0.001f;

                // this sets scale( Size)
                screen.transform.localScale = new Vector3(0.065f * 1.5f, 0.065f, 0.000001f);


            }
            else if (allVariables.remoteState == 5) 
            {

                extraX = -0.07f;
                extraY = 0.00f;
                extraZ = -0.010f;

                // this sets scale( Size)
                screen.transform.localScale = new Vector3(0.065f * 1.5f, 0.065f, 0.000001f);


            }
            else if (allVariables.remoteState == 6)
            {

                extraX = 0.071f;
                extraY = -0.035f;
                extraZ = 0.165f;


                // this sets scale( Size) of the screen 

                screen.transform.localScale = new Vector3(0.050f * 1.5f, 0.060f, 0.000001f);


            }
            else if (allVariables.remoteState == 7)
            {

                extraX = 0.071f;
                extraY = -0.035f;
                extraZ = 0.325f;


                // this sets scale( Size) of the screen 

                screen.transform.localScale = new Vector3(0.050f * 1.5f, 0.060f, 0.000001f);


            }






            Vector3 newposition = drawProjectionFinger.GetArmTransform().position;
            print(extraX);
             //   screen.transform.position = new Vector3(newposition.x + extraX, newposition.y + extraY, newposition.z + extraZ); //sukhan change 



            // Shukan Changed here
            Vector3 offset = new Vector3(extraX, extraY, extraZ);
            screen.transform.position = wristTransform.TransformPoint(offset);
            // till here



            if(allVariables.remoteState == 5)
            {
                Transform leftIndexTipTransform = drawProjectionFinger.GetIndexFingerTransform(leftHandSkeleton);
                // Apply the offset relative to the finger tip's local space
                screen.transform.position = leftIndexTipTransform.TransformPoint(offset);
                screen.transform.rotation = fingerTipTransform.rotation * Quaternion.Euler(90, 180, 0);
            }
            else
            {
                // For other states, use the wrist transform offset as before
                screen.transform.position = wristTransform.TransformPoint(offset);
            }

            Vector3 newPosition = wristTransform.position;
            Quaternion newRotation = wristTransform.rotation;




            if (allVariables.remoteState == 1|| allVariables.remoteState == 2|| allVariables.remoteState == 3 || allVariables.remoteState == 4 )
            {
                screen.transform.rotation = wristTransform.rotation * Quaternion.Euler(-90, 90, 90);
            }
            else if (allVariables.remoteState == 5)
            {
                screen.transform.rotation = wristTransform.rotation * Quaternion.Euler(90, 180, 0);
                //screen.transform.rotation = fingerTipTransform.rotation * Quaternion.Euler(90, 180, 0);

            }
            else if (allVariables.remoteState == 6)
            {
                screen.transform.rotation = wristTransform.rotation * Quaternion.Euler(90, 90, 0);
            }
            else if (allVariables.remoteState == 7)
            {
                screen.transform.rotation = wristTransform.rotation * Quaternion.Euler(90, 90, 0);
            }




        }



    }


    // Shukan made this methods
    public void ShowNextMaterial()
    {
        currentMaterialIndex = (currentMaterialIndex + 1) % materials.Length;
        screenRenderer.material = materials[currentMaterialIndex];
    }

    public void ShowPreviousMaterial()
    {
        currentMaterialIndex = (currentMaterialIndex - 1 + materials.Length) % materials.Length;
        screenRenderer.material = materials[currentMaterialIndex];
    }


    // till here


    void Start()
    {

       
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


    public IEnumerator RunExperiment()
    {
        completed = false;
        // Randomly shuffle the center points
        //ShuffleList(centerPoints);
        Shuffle(amplitudeArray);
        Shuffle(widthArray);
        widthArray=  AddFloatAtStart(widthArray,0.0077f);
        yield return new WaitForSeconds(2f);

        foreach (Vector3 centerPosition in centerPoints)
        {

            foreach (float amplitude in amplitudeArray)
            {


                foreach (float width in widthArray)
                {


                    for (int i = 0; i < 2; i++)
                    {
                        Debug.Log($"Iteration: Amplitude={amplitude}, Width={width}, i={i}");

                        // Iterate over all combinations of amplitudes and widths







                        work = false;
                        // we need to detach all children from   the current gameobject   (if any)
                        // Detach all children (if any)
                        fittsLawExperiment.DetachAllChildren();
                        yield return new WaitForSeconds(0.3f);

                        //yield return new WaitForSeconds(1f);
                        screen.transform.rotation = Quaternion.Euler(0, 0, 0);
                        //screen.transform.position = new Vector3(0.47f, 0,0);
                        screen.transform.position = new Vector3(0f, 0f, 0f);
                        yield return new WaitForSeconds(0.5f);






                        if (allVariables.remoteState == 1 || allVariables.remoteState == 3) // remoteState 1 & 3 is small
                        {
                            // sets how much big the target will be...
                            fittsLawExperiment.ArrangeTargets(amplitude, width, centerPosition);
                            Debug.Log("Here Once 1");


                        }
                        else if (allVariables.remoteState == 2 || allVariables.remoteState == 4) // remoteState 2 & 4 is big
                        {
                            // sets how much big the target will be...
                            fittsLawExperiment.ArrangeTargets(amplitude * 2, width * 2, centerPosition);
                            Debug.Log("NOT Here Once 1");
                        }
                        else if (allVariables.remoteState == 5 || allVariables.remoteState == 6) // remoteState 5 & 6 is??
                        {
                            // sets how much big the target will be...
                            fittsLawExperiment.ArrangeTargets(amplitude * 2, width * 2, centerPosition);
                            Debug.Log("NOT Here Once 2");

                        }

                        else { fittsLawExperiment.ArrangeTargets(amplitude, width, centerPosition); Debug.Log("NOT Here Once 3"); }


                            // fittsLawExperiment.ArrangeTargets(amplitude / 2, width / 2, centerPosition);

                            yield return new WaitForSeconds(0.2f);
                        fittsLawExperiment.AttachTargetsAsChildren();
                        yield return new WaitForSeconds(0.2f);



                        if (allVariables.remoteState == 1)
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
                        else screen.transform.rotation = Quaternion.Euler(0, 0, 0);

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


      void Shuffle( float[] array)
    {
        System.Random rnd = new System.Random();
        int n = array.Length;

        // Fisher-Yates shuffle
        for (int i = n - 1; i > 0; i--)
        {
            // Pick a random index from 0 to i
            int j = rnd.Next(i + 1);

            // Swap array[i] with array[j]
            float temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }




    public  float[] AddFloatAtStart(float[] originalArray, float newValue)
    {
        float[] newArray = new float[originalArray.Length + 1];

        // Place the new float at index 0
        newArray[0] = newValue;

        // Copy all elements of the original array starting from index 1
        Array.Copy(originalArray, 0, newArray, 1, originalArray.Length);

        return newArray;
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
