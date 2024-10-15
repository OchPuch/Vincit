using System;
using UnityEngine;

public class InactiveDoor : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _activeMaterial;

    private Material _startMaterial;
    
    private void Start()
    {
        _collider.enabled = false;
        _startMaterial = _renderer.material;
    }

    public void Reset()
    {
        _collider.enabled = false;
        _renderer.material = _startMaterial;
    }

    public void Activate()
    {
        _collider.enabled = true;
        _renderer.material = _activeMaterial;
    }
}