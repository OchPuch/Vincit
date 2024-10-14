using UnityEngine;

namespace Player.View
{
    public class SpeedLinesView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem speedLines;

        private void Start()
        {
            speedLines.transform.SetParent(null);
            Kill();
        }
        

        public void UpdatePosition(Vector3 position)
        {
            speedLines.transform.position = position;
        }
        
        public void UpdateDirection(Vector3 direction)
        {
            speedLines.transform.forward = direction;
        }

        public void UpdateUpDirection(Vector3 direction)
        {
            speedLines.transform.up = direction;
        }
        
        public void Activate(Vector3 position,Vector3 direction)
        {
            speedLines.transform.forward = direction;
            speedLines.Play();
        }

        public void Disable()
        {
            speedLines.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        public void Kill()
        {
            speedLines.Stop();
        }

    }
}