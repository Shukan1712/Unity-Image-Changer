using UnityEngine;

public class GestureDetection : ConfirmationDetection
{
    [SerializeField] private OVRHand leftConfirmationHand, rightConfirmationHand;
    private OVRHand confirmationHand;
    private bool isHoldingGesture = false, isPinching = false;

    void Update()
    {
        if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.HAND)
            confirmationHand = leftConfirmationHand;
        else confirmationHand = rightConfirmationHand;
    }

    public override bool ConfirmationDetected()
    {
        if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.HAND)
        {
            isPinching = confirmationHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            if (!isHoldingGesture)
            {
                isHoldingGesture = isPinching;
                return isPinching;
            }
            else if (!isPinching) isHoldingGesture = false;
        } else
        {
            isPinching = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
            if (!isHoldingGesture)
            {
                isHoldingGesture = isPinching;
                return isPinching;
            }
            else if (!isPinching) isHoldingGesture = false;
        }
        return false;
    }
}
