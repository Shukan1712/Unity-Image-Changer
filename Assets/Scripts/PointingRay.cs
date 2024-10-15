using System.Collections.Generic;
using UnityEngine;

public class PointingRay : SingletonMonoBehaviour<PointingRay>
{
    [Header("Ray Parameters")]
    public LineRenderer lineRenderer;
    [SerializeField] private float rayDistance;
    [SerializeField] private float rayWidth;
    [SerializeField] private int numberOfPoints;
    [SerializeField] private float timeBetweenPoints;

    [Header("Ray Origin")]
    [SerializeField] public OVRSkeleton pointingHand;
    [SerializeField] public Transform leftEyePosition, rightEyePosition, headPosition, controller;

/*    [Header("Kalman Filters")]
    [SerializeField] private int originHistoryWindow = 4;
    [SerializeField] private int rotationHistoryWindow = 6;
    [SerializeField] private float k1q = 0.0001f, k2q = 0.00001f, k1r = 0.1f, k2r = 0.000001f;
    [SerializeField] private float k1qEyes = 0.0001f, k2qEyes = 0.00001f, k1rEyes = 0.1f, k2rEyes = 0.000001f;*/

    [Header("Teleportation Parameters")]
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask startCubeMask, environmentalMask;
    public bool starting, intercepting, interceptingTarget, interceptingStartCube, interceptingEnvironment;
    [SerializeField] private Color rayColorDefaultState = Color.red, rayColorHoverState = Color.green;

    protected Vector3 originPoint, originRotationVector;

    OneEuroFilterVector3 m_HandFilter = new OneEuroFilterVector3(Vector3.zero);
    OneEuroFilterVector3 m_HandOriFilter = new OneEuroFilterVector3(Vector3.zero);
    OneEuroFilterVector3 m_ControllerFilter = new OneEuroFilterVector3(Vector3.zero);

    bool m_WasHandTrackedLastFrame, m_WasControllerTrackedLastFrame;

    [Header("One Euro Filter")]
    [SerializeField] [Tooltip("Smoothing amount at low speeds.")] float m_FilterMinCutoff = 0.1f;
    [SerializeField] [Tooltip("Filter's responsiveness to speed changes.")] float m_FilterBeta = 0.2f;
    /*KalmanFilterVector3 kalmanV3Origin, kalmanV3Rotation;
    KalmanFilterVector3 kalmanV3OriginEyes, kalmanV3RotationEyes;
    CircularBuffer.CircularBuffer<Vector3> originHistory, rotationHistory;*/

    public void StartingProcedure()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //resetKalmanFilters();
        m_HandFilter = new OneEuroFilterVector3(Vector3.zero);
        m_HandOriFilter = new OneEuroFilterVector3(Vector3.zero);
        starting = true;
        intercepting = false;
        interceptingTarget = false;
        interceptingStartCube = false;
        interceptingEnvironment = false;
        originPoint = Vector3.zero;
        originRotationVector = Vector3.zero;
        SetupRay();
    }

/*    public void resetKalmanFilters()
    {
        kalmanV3Origin = new KalmanFilterVector3(k1q, k1r);
        kalmanV3Rotation = new KalmanFilterVector3(k2q, k2r);
        
        kalmanV3OriginEyes = new KalmanFilterVector3(k1qEyes, k1rEyes);
        kalmanV3RotationEyes = new KalmanFilterVector3(k2qEyes, k2rEyes);

        originHistory = new CircularBuffer.CircularBuffer<Vector3>(originHistoryWindow);
        rotationHistory = new CircularBuffer.CircularBuffer<Vector3>(rotationHistoryWindow);
    }*/

    void SetupRay()
    {
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        lineRenderer.startColor = rayColorDefaultState;
        lineRenderer.endColor = rayColorDefaultState;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y,
            transform.position.z + rayDistance));
    }

    void Update()
    {
        m_WasHandTrackedLastFrame = false;

        starting = GameObject.FindGameObjectWithTag("Start") != null;
        UpdateOrigin();
        UpdateRay();
        if (!intercepting && !interceptingStartCube) lineRenderer.startColor = lineRenderer.endColor = rayColorDefaultState;
    }

    private void UpdateOrigin()
    {
        if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.HAND)
            transform.position = getStableIndexFingerPosition();
        else if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.CONTROLLER)
            transform.position = getStableControllerPosition();
        else if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.HEAD)
            transform.position = headPosition.position;
        else if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.EYES && 
            GameManager.Instance.confirmationTechnique == GameManager.ConfirmationTechnique.BLINK)
            transform.position = getStableLeftEyePosition();
        else if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.EYES)
            transform.position = getStableEyesPosition();
    }

    private void UpdateRay()
    {
        Vector3 rayDirection = Vector3.zero;

        if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.HAND)
            rayDirection = getStableIndexFingerOrientation();
        else if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.CONTROLLER)
            rayDirection = getStableControllerOrientation();
        else if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.HEAD)
            rayDirection = headPosition.TransformDirection(Vector3.forward);
        else if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.EYES &&
            GameManager.Instance.confirmationTechnique == GameManager.ConfirmationTechnique.BLINK)
            rayDirection = getStableLeftEyeOrientation();
        else if (GameManager.Instance.pointingTechnique == GameManager.PointingTechnique.EYES)
            rayDirection = getStableEyesOrientation();

        if (GameManager.Instance.pointerType == GameManager.PointerType.LINEAR)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + rayDirection * rayDistance);

            interceptingStartCube = Physics.Raycast(transform.position, rayDirection, out RaycastHit startHit, rayDistance, startCubeMask); 

            if(!starting)
            {
                intercepting = false;
                interceptingTarget = Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, rayDistance, targetMask);
                interceptingEnvironment = Physics.Raycast(transform.position, rayDirection, out RaycastHit envHit, rayDistance, environmentalMask);

                if (interceptingTarget && interceptingEnvironment)
                {
                    lineRenderer.SetPosition(1, hit.distance < envHit.distance ? hit.point : envHit.point);
                    if (hit.distance < envHit.distance)
                    {
                        intercepting = true;
                        lineRenderer.startColor = lineRenderer.endColor = rayColorHoverState;
                    }
                }
                else if (interceptingEnvironment) lineRenderer.SetPosition(1, envHit.point);
                else if (interceptingTarget)
                {
                    intercepting = true;
                    lineRenderer.SetPosition(1, hit.point);
                    lineRenderer.startColor = lineRenderer.endColor = rayColorHoverState;
                }
            } else if(interceptingStartCube)
            {
                lineRenderer.SetPosition(1, startHit.point);
                lineRenderer.startColor = lineRenderer.endColor = rayColorHoverState;
            }
        }
        else if (GameManager.Instance.pointerType == GameManager.PointerType.PARABOLIC)
        {
            lineRenderer.positionCount = numberOfPoints;
            List<Vector3> points = new List<Vector3>();

            for (float t = 0; t < numberOfPoints * timeBetweenPoints; t += timeBetweenPoints)
            {
                Vector3 rayVelocity = rayDirection * Mathf.Sqrt(rayDistance * Mathf.Abs(Physics.gravity.y));
                Vector3 newPoint = transform.position + t * rayVelocity;
                newPoint.y = transform.position.y + rayVelocity.y * t + Physics.gravity.y / 2f * t * t;
                points.Add(newPoint);

                if (points.Count > 1)
                {
                    Vector3 oldPoint = points[points.Count - 2];
                    Vector3 direction = newPoint - oldPoint;

                    interceptingStartCube = Physics.Raycast(oldPoint, direction, out RaycastHit startHit, direction.magnitude, startCubeMask);
                    
                    if (!starting)
                    {
                        intercepting = false;
                        interceptingTarget = Physics.Raycast(oldPoint, direction, out RaycastHit hit, direction.magnitude, targetMask);
                        interceptingEnvironment = Physics.Raycast(oldPoint, direction, out RaycastHit envHit, direction.magnitude, environmentalMask);

                        if (interceptingTarget)
                        {
                            if (!interceptingEnvironment || hit.distance < envHit.distance)
                            {
                                intercepting = true;
                                points[points.Count - 1] = hit.point;
                                lineRenderer.startColor = lineRenderer.endColor = rayColorHoverState;
                            } else
                            {
                                points[points.Count - 1] = envHit.point;
                            }
                        } else if(interceptingEnvironment)
                        {
                            points[points.Count - 1] = envHit.point;
                        }

                        if (interceptingTarget || interceptingEnvironment)
                        {
                            lineRenderer.positionCount = points.Count;
                            break;
                        }
                    } else if(interceptingStartCube)
                    {
                        lineRenderer.startColor = lineRenderer.endColor = rayColorHoverState;
                        lineRenderer.positionCount = points.Count;
                        break;
                    }
                }
            }
            lineRenderer.SetPositions(points.ToArray());
        }
    }

    private Vector3 getStableIndexFingerPosition()
    {
        Transform currentWristTransform = getWristTransform();

        if (currentWristTransform != null)
        {
            originPoint = currentWristTransform.position;

            if (!m_WasHandTrackedLastFrame)
                m_HandFilter.Initialize(originPoint);
            else
                originPoint = m_HandFilter.Filter(originPoint, Time.deltaTime, m_FilterMinCutoff, m_FilterBeta);
        }

        m_WasHandTrackedLastFrame = currentWristTransform != null;

        return originPoint;
    }

    private Vector3 getStableIndexFingerOrientation()
    {
        Transform currentWristTransform = getWristTransform();

        if (currentWristTransform != null)
        {
            originRotationVector = currentWristTransform.TransformDirection(Vector3.right);
            if (!m_WasHandTrackedLastFrame)
                m_HandOriFilter.Initialize(originRotationVector);
            else
                originRotationVector = m_HandOriFilter.Filter(originRotationVector, Time.deltaTime, m_FilterMinCutoff, m_FilterBeta);
        }

        m_WasHandTrackedLastFrame = currentWristTransform != null;

        return originRotationVector;
    }

    private Vector3 getStableControllerPosition()
    {
        Transform currentControllerTransform = controller;
        if (currentControllerTransform != null)
        {
            if (!m_WasControllerTrackedLastFrame)
            {
                m_HandFilter.Initialize(currentControllerTransform.position);
                originPoint = currentControllerTransform.position;
            }
            else
            {
                originPoint = m_HandFilter.Filter(currentControllerTransform.position, Time.deltaTime, m_FilterMinCutoff, m_FilterBeta);
            }
        }

        m_WasControllerTrackedLastFrame = currentControllerTransform != null;

        return originPoint;
    }

    private Vector3 getStableControllerOrientation()
    {
        Transform currentControllerTransform = controller;
        if (currentControllerTransform != null)
        {
            originRotationVector = currentControllerTransform.TransformDirection(Vector3.forward);
            /*rotationHistory.PushBack(originRotationVector);
            originRotationVector = kalmanV3Rotation.Update(rotationHistory.Back(), k2q, k2r);*/
        }
        return originRotationVector;
    }

    public Transform getWristTransform()
    {
        foreach (var b in pointingHand.Bones)
        {
            if (b.Id == OVRSkeleton.BoneId.Hand_WristRoot) return b.Transform;
        }
        return null;
    }

    private Vector3 getStableEyesPosition()
    {
        originPoint = new Vector3((leftEyePosition.position.x + rightEyePosition.position.x) / 2,
            (leftEyePosition.position.y + rightEyePosition.position.y) / 2,
            (leftEyePosition.position.z + rightEyePosition.position.z) / 2);
        /*originHistory.PushBack(originPoint);
        originPoint = kalmanV3OriginEyes.Update(originHistory.Back(), k1qEyes, k1rEyes);*/
        return originPoint;
    }

    private Vector3 getStableLeftEyePosition()
    {
        originPoint = leftEyePosition.position;
        /*originHistory.PushBack(originPoint);
        originPoint = kalmanV3OriginEyes.Update(originHistory.Back(), k1qEyes, k1rEyes);*/
        return originPoint;
    }

    private Vector3 getStableEyesOrientation()
    {
        originRotationVector = (leftEyePosition.TransformDirection(Vector3.forward) + rightEyePosition.TransformDirection(Vector3.forward)) / 2;
        /*rotationHistory.PushBack(originRotationVector);
        originRotationVector = kalmanV3RotationEyes.Update(rotationHistory.Back(), k2qEyes, k2rEyes);*/
        return originRotationVector;
    }

    private Vector3 getStableLeftEyeOrientation()
    {
        originRotationVector = leftEyePosition.TransformDirection(Vector3.forward);
        /*rotationHistory.PushBack(originRotationVector);
        originRotationVector = kalmanV3RotationEyes.Update(rotationHistory.Back(), k2qEyes, k2rEyes);*/
        return originRotationVector;
    }
}