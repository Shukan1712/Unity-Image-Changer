using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllVariables : MonoBehaviour
{
    public wristRotationScript wristRotationScript;
    public cursorMovementScript cursorMovementScript;

    public enum PointingTechnique
    {
        HAND,
        CONTROLLER,
        HEAD,
        EYES
    };

    public PointingTechnique pointingTechnique;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
