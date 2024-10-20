using UnityEngine;

public class GestureDetection : ConfirmationDetection
{
    [SerializeField] private OVRHand leftConfirmationHand, rightConfirmationHand;
    private OVRHand confirmationHand;
    private bool isHoldingGesture = false, isPinching = false;

    void Update()
    {
        
            confirmationHand = leftConfirmationHand;
       
    }

    public override bool ConfirmationDetected()
    {
    
            isPinching = confirmationHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            if (!isHoldingGesture)
            {
                isHoldingGesture = isPinching;
                return isPinching;
            }
            else if (!isPinching) isHoldingGesture = false;
         

        return false;
    }
}
