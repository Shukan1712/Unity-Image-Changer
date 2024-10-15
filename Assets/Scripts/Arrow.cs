using UnityEngine;

public class Arrow : MonoBehaviour
{
    public bool IsHovered { get; private set; }
    [SerializeField] private Material OnHoverActiveMaterial, OnIdleMaterial;
    private MeshRenderer meshRenderer;
    public Target target;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        IsHovered = target.IsHovered;
        meshRenderer.material = IsHovered ? OnHoverActiveMaterial : OnIdleMaterial;
    }
}