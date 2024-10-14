using General;
using TreeEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class PlayerTrigger : GamePlayBehaviour
    {
        [SerializeField] private UnityEvent playerEntered;
        [SerializeField] private UnityEvent playerExited;
        [SerializeField] protected bool DestroyOnEnter = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (other.TryGetComponent(out Player.Player player))
            {
                OnPlayerEnter(player);
                playerEntered.Invoke();
                if (DestroyOnEnter) Destroy(gameObject);    
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            OnPlayerExit(other);
            playerExited.Invoke();
        }
        
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            OnPlayerStay(other);
        }
        
        
        protected virtual void OnPlayerEnter(Player.Player player){}
        protected virtual void OnPlayerStay(Collider player) {}
        protected virtual void OnPlayerExit(Collider player) {}
    }
    
}