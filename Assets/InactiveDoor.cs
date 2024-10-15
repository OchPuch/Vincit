using UnityEngine;

public class InactiveDoor : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _activeMaterial;
    
    private void Start()
    {
        _collider.enabled = false;
    }

    public void Activate()
    {
        _collider.enabled = true;
        _renderer.material = _activeMaterial;
    }
}