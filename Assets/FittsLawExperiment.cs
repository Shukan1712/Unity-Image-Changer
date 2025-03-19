using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FittsLawExperiment : MonoBehaviour
{
    // Array to hold the target GameObjects
    public GameObject[] targets = new GameObject[9];

    // Index to keep track of the current target in the selection sequence
    public int targetIndex = 0;

    // The order in which targets will be selected
    private int[] targetOrder = new int[] { 0, 4, 8, 3, 7, 2, 6, 1, 5 ,0};

    public bool SequenceComplete  = false;
    

    public int currentTargetNumber;

    public AllVariables allvariables; 



    /// <summary>
    /// Arranges 9 targets around a circle centered at the given position.
    /// </summary>
    /// <param name="amplitude">The diameter of the circle.</param>
    /// <param name="width">The size (width) of each target.</param>
    /// <param name="position">The center position of the circle.</param>
    public void ArrangeTargets(float amplitude, float width, Vector3 position)
    {
        float radius = amplitude / 2f;
        allvariables.currentAmplitude = amplitude;
        allvariables.currentWidth = width;
        allvariables.currentDepth = position.z;
        


        for (int i = 0; i < 9; i++)
        {
            float angleDeg = i * 40f; // 360 degrees divided by 9 targets
            float angleRad = angleDeg * Mathf.Deg2Rad;

            float x = position.x + radius * Mathf.Cos(angleRad);
            float y = position.y + radius * Mathf.Sin(angleRad);
            float z = position.z; // Z value remains the same

            // Set the position of each target
            targets[i].transform.position = new Vector3(x, y, z);

            // Set the size of each target
            targets[i].transform.localScale = new Vector3(width, width, 0.001f);
        }
    }

    /// <summary>
    /// Selects the next target in the predefined sequence.
    /// </summary>
    /// <param name="currentTargetNumber">The variable to store the next target's index.</param>
    /// <returns>False if all 8 targets have been selected; otherwise, true.</returns>
    public bool GetNextTarget()
    {
        if (targetIndex >= targetOrder.Length)
        {
            currentTargetNumber = -1; // No more targets
            return false;
        }
        else
        {
            currentTargetNumber = targetOrder[targetIndex];
            allvariables.currentTarget = targets[currentTargetNumber];

            targetIndex++;
            return true;
        }
    }




    public void ResetTargetSequence()
    {
        targetIndex = 0;
        SequenceComplete = false;
    }

    /// <summary>
    /// Selects the next target in the predefined sequence.
    /// </summary>
    /// <param name="currentTargetNumber">The variable to store the next target's index.</param>
    /// <returns>False if all 8 targets have been selected; otherwise, true.</returns>
    public bool GetNextTarget(out int currentTargetNumber)
    {
        if (targetIndex >= targetOrder.Length)
        {
            currentTargetNumber = -1; // No more targets
            SequenceComplete = true;
            return false;
        }
        else
        {
            currentTargetNumber = targetOrder[targetIndex];
            targetIndex++;
            return true;
        }
    }







}
