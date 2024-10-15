using System.Collections;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum PointingTechnique
    {
        HAND,
        CONTROLLER,
        HEAD,
        EYES
    };

    public enum PointerType
    {
        LINEAR,
        PARABOLIC
    };

    public enum ConfirmationTechnique
    {
        GESTURE,
        VOICE,
        BLINK,
        DWELL
    };

    public int participantId = 0;
    public PointingTechnique pointingTechnique;
    public PointerType pointerType;
    public ConfirmationTechnique confirmationTechnique;
    public bool restart = true;
    private bool restarting = false;

    /*public void GetData()
    {
        switch (DatabaseManager.Instance.pointingTechnique)
        {
            case "hand":
                pointingTechnique = PointingTechnique.HAND;
                break;
            case "head":
                pointingTechnique = PointingTechnique.HEAD;
                break;
            case "eyes":
                pointingTechnique = PointingTechnique.EYES;
                break;
            default:
                pointingTechnique = PointingTechnique.HAND;
                break;
        }

        switch (DatabaseManager.Instance.pointerType)
        {
            case "linear":
                pointerType = PointerType.LINEAR;
                break;
            case "parabolic":
                pointerType = PointerType.PARABOLIC;
                break;
            default:
                pointerType = PointerType.LINEAR;
                break;
        }

        switch (DatabaseManager.Instance.confirmationTechnique)
        {
            case "gesture":
                confirmationTechnique = ConfirmationTechnique.GESTURE;
                break;
            case "voice":
                confirmationTechnique = ConfirmationTechnique.VOICE;
                break;
            case "blink":
                confirmationTechnique = ConfirmationTechnique.BLINK;
                break;
            case "dwell":
                confirmationTechnique = ConfirmationTechnique.DWELL;
                break;
            default:
                confirmationTechnique = ConfirmationTechnique.GESTURE;
                break;
        }
    }*/

    private IEnumerator RestartSequence()
    {
        CheckpointManager.Instance.StartingProcedure();
        PointingRay.Instance.StartingProcedure();
        TeleportationManager.Instance.StartingProcedure();
        restart = false;
        restarting = false;
        yield return null;
    }

    private void Update()
    {
        if (restart && !restarting)
        {
            restarting = true;
            StartCoroutine(RestartSequence());
        }
    }
}