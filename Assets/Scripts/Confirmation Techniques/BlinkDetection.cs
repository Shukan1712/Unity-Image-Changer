using UnityEngine;

public class BlinkDetection : ConfirmationDetection
{
    [SerializeField] private OVRFaceExpressions faceExpressions;
    [SerializeField] private float eyeClosedThreshold = 0.5f, eyeOpenThreshold = 0.1f;
    private bool isBlinked = false;

    public override bool ConfirmationDetected()
    {
        try
        {
            if(faceExpressions[OVRFaceExpressions.FaceExpression.EyesClosedL] < eyeOpenThreshold)
            {
                if (faceExpressions[OVRFaceExpressions.FaceExpression.EyesClosedR] < eyeOpenThreshold)
                {
                    if (isBlinked)
                    {
                        isBlinked = false;
                        return true;
                    }
                } else if (faceExpressions[OVRFaceExpressions.FaceExpression.EyesClosedR] >= eyeClosedThreshold)
                {
                    isBlinked = true;
                }
                return false;
            } else
            {
                isBlinked = false;
                return false;
            }
        } catch
        {
            return false;
        }
    }
}
