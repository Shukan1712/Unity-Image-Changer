using UnityEngine;
using UnityEngine.UI;

public class DwellDetection : ConfirmationDetection
{
    [SerializeField] private Slider TimeoutSlider;
    [SerializeField] private float sightedForTimeout = 0.8f;
    [SerializeField] private float angularThreshold = 1f;
    private float sightedFor = 0;
    private Vector3 holdingAt = new Vector3(0, 0, 1);

    void Start()
    {
        TimeoutSlider.minValue = 0;
        TimeoutSlider.maxValue = sightedForTimeout;
    }

    void Update()
    {
        TimeoutSlider.gameObject.SetActive(GameManager.Instance.confirmationTechnique == GameManager.ConfirmationTechnique.DWELL);
    }

    public bool ConfirmationDetected(LineRenderer lineRenderer)
    {
        if (holdingAt.sqrMagnitude < 0.00001f)
        {
            holdingAt = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);
            return false;
        }

        if (Vector3.Angle(lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0), holdingAt) < angularThreshold)
        {
            sightedFor += Time.deltaTime;
            if (sightedFor > sightedForTimeout)
            {
                holdingAt = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);
                sightedFor = 0;
                return true;
            }
        }
        else
        {
            holdingAt = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);
            sightedFor = 0;
        }

        TimeoutSlider.value = sightedFor;
        
        return false;
    }
}
