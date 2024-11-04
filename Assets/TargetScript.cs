using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public int targetNum;
    public LayerMask cursorLayer;
    public bool IsCollidingWithCursor = false;

    public AllVariables allVariables;


    MaterialPropertyBlock _propblock;
    Renderer _renderer;
    string redColorCode = "#FF000049";   // 6 digit Hex for Color + 2 Hex last er FF holo transparency
    string greenColorCode = "#00FF0049";
    string WhiteColorCode = "#FFFFFF49";
    //string LightRedColorCode = "#FFCCCB49";
    Color myColor;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetNum == allVariables.fittsLawExperiment.currentTargetNumber)
        {
            if (IsCollidingWithCursor)
            {

                MychangeColor(greenColorCode);
            }
            else 
            {
                MychangeColor(redColorCode);
            }
        }

        else MychangeColor(WhiteColorCode);

    }

    public void OnTriggerStay(Collider other)
    {
        allVariables.mainInteraction.currentTargetCollided = this.gameObject;
        // Check if the other collider's GameObject is on the cursor layer
        if (IsInLayerMask(other.gameObject, cursorLayer))
        {
            if (targetNum == allVariables.fittsLawExperiment.currentTargetNumber)
            {
                IsCollidingWithCursor = true;

              

            }
            Debug.Log("HITTTTTTT");
        }
        else
        {
            IsCollidingWithCursor = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // Reset the collision flag when the other object exits the trigger
        if (IsInLayerMask(other.gameObject, cursorLayer))
        {
            IsCollidingWithCursor = false;
        }

     

    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }




    public void MychangeColor(string colorCode)
    {
        _renderer = this.GetComponent<Renderer>();  //just change the GameObject ehre
        _propblock = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_propblock);
        myColor = new Color();
        ColorUtility.TryParseHtmlString(colorCode, out myColor);
        _propblock.SetColor("_Color", myColor);
        _renderer.SetPropertyBlock(_propblock);


    }



}
