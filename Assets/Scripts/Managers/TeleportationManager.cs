using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationManager : SingletonMonoBehaviour<TeleportationManager>
{
    [SerializeField] public PointingRay pointingRay;
    [SerializeField] private ConfirmationDetection gestureDetector, voiceDetector, blinkDetector;
    [SerializeField] private DwellDetection dwellDetector;
    [SerializeField] private GameObject startCube;
    [SerializeField] private Vector3 startCubePosition;
    [SerializeField] private AudioSource audioSuccess, audioError;
    [SerializeField] private float teleportationCooldown = 0.5f;
    private bool confirmation, wasIntercepting;
    private Target currentTarget;
    private GameObject player;
    private float taskGenerated, taskStarted, firstTimeIntersected, lastTimeIntersected, teleported;
    private int failed, numberOfIntercepting, didMistake, numberOfMistakes;
    private float deviationFromTheCenter, angularDeviationFromTheCenter;
    private List<Vector3> handPositions, headPositions, leftEyePositions, rightEyePositions;
    private List<Quaternion> handOrientations, headOrientations, leftEyeOrientations, rightEyeOrientations;
    private int current = 0;
    private DatabaseManager db;
    private GameManager gm;

    private float nextActionTime = 0.0f;
    [SerializeField] private float period = 0.5f;

    public void StartingProcedure()
    {
        confirmation = false;
        wasIntercepting = false;
        currentTarget = null;
        current = 0;
        player = GameObject.Find("OVRCameraRig");
        player.transform.position = Vector3.zero;
        Instantiate(startCube, player.transform.position + startCubePosition, Quaternion.identity);
        ResetData();
        db = DatabaseManager.Instance;
        gm = GameManager.Instance;
    }

    private void Update()
    {
        if (CheckpointManager.Instance.current != CheckpointManager.Instance.checkpointQuantity)
        {
            UpdateData();
            RecordPositions();
            currentTarget = CheckpointManager.Instance.currentTarget;
            if(currentTarget != null) currentTarget.Hover(pointingRay.intercepting);
            confirmation = ConfirmationDetected();
            if (confirmation)
            {
                if (pointingRay.interceptingStartCube) StartCoroutine(StartTask());
                else if (!pointingRay.starting && pointingRay.intercepting) StartCoroutine(Teleport());
                else if (!pointingRay.starting) StartCoroutine(Mistake());
            }
            else if(failed == 0 && taskStarted > 0 && (Time.time - taskStarted) > 10)
            {
                failed = 1;
                StartCoroutine(Teleport());
            }
        }
    }

    private void RecordPositions()
    {
        if (taskStarted > 0 && (Time.time - taskStarted > nextActionTime))
        {
            nextActionTime += period;
            headPositions.Add(pointingRay.headPosition.position);
            headOrientations.Add(pointingRay.headPosition.rotation);
            handPositions.Add(pointingRay.getWristTransform().position);
            handOrientations.Add(pointingRay.getWristTransform().rotation);
            leftEyePositions.Add(pointingRay.leftEyePosition.position);
            leftEyeOrientations.Add(pointingRay.leftEyePosition.rotation);
            rightEyePositions.Add(pointingRay.rightEyePosition.position);
            rightEyeOrientations.Add(pointingRay.rightEyePosition.rotation);
        }
    }

    private bool ConfirmationDetected()
    {
        if (GameManager.Instance.confirmationTechnique == GameManager.ConfirmationTechnique.GESTURE)
            return gestureDetector.ConfirmationDetected();
        else if (GameManager.Instance.confirmationTechnique == GameManager.ConfirmationTechnique.VOICE)
            return voiceDetector.ConfirmationDetected();
        else if (GameManager.Instance.confirmationTechnique == GameManager.ConfirmationTechnique.BLINK)
            return blinkDetector.ConfirmationDetected();
        else if (GameManager.Instance.confirmationTechnique == GameManager.ConfirmationTechnique.DWELL)
            return dwellDetector.ConfirmationDetected(pointingRay.lineRenderer);
        else return false;
    }

    private IEnumerator StartTask()
    {
        taskStarted = Time.time;
        Destroy(GameObject.FindGameObjectWithTag("Start").gameObject);
        audioSuccess.Play();
        yield return null;
    }

    private IEnumerator Teleport()
    {
        teleported = Time.time;

        Vector3 pointerOrigin = pointingRay.transform.position;
        Vector3 pointerTip = pointingRay.lineRenderer.GetPosition(pointingRay.lineRenderer.positionCount - 1);
        Vector3 targetPosition = currentTarget.transform.position;
        deviationFromTheCenter = Vector3.Distance(targetPosition, pointerTip);
        angularDeviationFromTheCenter = Vector3.Angle(targetPosition - pointerOrigin, pointerTip - pointerOrigin);

        RecordData();
        pointingRay.lineRenderer.enabled = false;
        player.transform.position = targetPosition;
        //pointingRay.resetKalmanFilters();
        CheckpointManager.Instance.HideCurrentCheckpoint();
        pointingRay.intercepting = false;
        pointingRay.interceptingTarget = false;
        pointingRay.interceptingEnvironment = false;
        pointingRay.starting = false;
        CheckpointManager.Instance.DisplayNextCheckpoint();
        if (CheckpointManager.Instance.current != CheckpointManager.Instance.checkpointQuantity)
            Instantiate(startCube, player.transform.position + startCubePosition, Quaternion.identity);
        audioSuccess.Play();
        yield return new WaitForSeconds(teleportationCooldown);
        pointingRay.lineRenderer.enabled = true;
        ResetData();
    }

    private IEnumerator Mistake()
    {
        didMistake = 1;
        numberOfMistakes++;
        audioError.Play();
        yield return null;
    }

    private void ResetData()
    {
        taskGenerated = Time.time;
        taskStarted = -1;
        wasIntercepting = false;
        numberOfIntercepting = 0;
        failed = 0;
        didMistake = 0;
        numberOfMistakes = 0;
        handPositions = new List<Vector3>();
        headPositions = new List<Vector3>();
        leftEyePositions = new List<Vector3>();
        rightEyePositions = new List<Vector3>();
        handOrientations = new List<Quaternion>();
        headOrientations = new List<Quaternion>();
        leftEyeOrientations = new List<Quaternion>();
        rightEyeOrientations = new List<Quaternion>();
        nextActionTime = 0.0f;
}

    private void UpdateData()
    {
        if (!wasIntercepting && pointingRay.intercepting)
        {
            numberOfIntercepting++;
            if (numberOfIntercepting == 1)
            {
                firstTimeIntersected = Time.time;
            }
            lastTimeIntersected = Time.time;
        }
        wasIntercepting = pointingRay.intercepting;
    }

    private void RecordData()
    {
        float distance, height, angle, size;
        distance = CheckpointManager.Instance.distances[current];
        height = CheckpointManager.Instance.heights[current];
        size = CheckpointManager.Instance.sizes[current];
        angle = CheckpointManager.Instance.angles[current];

        int repeatNumber = CheckpointManager.Instance.repeatNumbers[current];

        current++;

        /*if(db.isRecord)
        {*/
            string pointingTechniqueCombinedName = gm.pointingTechnique.ToString().ToLower() + "_" + gm.pointerType.ToString().ToLower();
            var log = new StudyDataLog
            {
                participantId = gm.participantId,
                pointingTechnique = gm.pointingTechnique.ToString().ToLower(),
                pointerType = gm.pointerType.ToString().ToLower(),
                pointingTechniqueCombinedName = pointingTechniqueCombinedName,
                confirmationTechnique = gm.confirmationTechnique.ToString().ToLower(),
                timestamp = DateTime.Now,
                checkpointIndex = current - 1,
                repeatNumber = repeatNumber,
                distance = distance,
                height = height,
                angle = angle,
                size = size,
                timeToStart = taskStarted - taskGenerated,
                timeToPointFirstTime = firstTimeIntersected - taskStarted,
                timeToPointLastTime = lastTimeIntersected - taskStarted,
                timeToConfirm = teleported - lastTimeIntersected,
                timeToTeleport = teleported - taskStarted,
                deviationFromTheCenter = deviationFromTheCenter,
                angularDeviationFromTheCenter = angularDeviationFromTheCenter,
                failed = failed,
                numberOfOvershooting = numberOfIntercepting - 1,
                didMistake = didMistake,
                numberOfMistakes = numberOfMistakes,
                handPositions = positionsToJson(handPositions),
                headPositions = positionsToJson(headPositions),
                leftEyePositions = positionsToJson(leftEyePositions),
                rightEyePositions = positionsToJson(rightEyePositions),
                handOrientations = orientationsToJson(handOrientations),
                headOrientations = orientationsToJson(headOrientations),
                leftEyeOrientations = orientationsToJson(leftEyeOrientations),
                rightEyeOrientations = orientationsToJson(rightEyeOrientations)
            };
            StartCoroutine(db.LogStudyData(log));
        //}
    }

    private string positionsToJson(List<Vector3> positions) {
        string res = "{ ";
        for (int i = 0; i < positions.Count; i++)
        {
            res += "\"" + i + "\": ";
            res += positions[i].ToString();
            if(i < positions.Count - 1)
            {
                res += ", ";
            } else res += " }";
        }
        return res;
    }

    private string orientationsToJson(List<Quaternion> orientations)
    {
        string res = "{ ";
        for (int i = 0; i < orientations.Count; i++)
        {
            res += "\"" + i + "\": ";
            res += orientations[i].ToString();
            if (i < orientations.Count - 1)
            {
                res += ", ";
            }
            else res += " }";
        }
        return res;
    }
}