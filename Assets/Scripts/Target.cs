using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class Target : MonoBehaviour
{
    public bool IsHovered { get; private set; }
    [SerializeField] private Material OnHoverActiveMaterial, OnIdleMaterial;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Hover(bool state)
    {
        IsHovered = state;
    }

    void Update()
    {
        meshRenderer.material = IsHovered ? OnHoverActiveMaterial : OnIdleMaterial;
    }
}