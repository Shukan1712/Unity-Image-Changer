using System;

[Serializable]
public class StudyDataLog
{
    public int participantId;
    public string pointingTechnique, pointerType, pointingTechniqueCombinedName, confirmationTechnique;
    public DateTime timestamp;
    public int checkpointIndex, repeatNumber;
    public float distance, height, angle, size;
    public float timeToStart, timeToPointFirstTime, timeToPointLastTime, timeToConfirm, timeToTeleport;
    public float deviationFromTheCenter, angularDeviationFromTheCenter;
    public int failed, numberOfOvershooting, didMistake, numberOfMistakes;
    public string handPositions, headPositions, leftEyePositions, rightEyePositions;
    public string handOrientations, headOrientations, leftEyeOrientations, rightEyeOrientations;
}