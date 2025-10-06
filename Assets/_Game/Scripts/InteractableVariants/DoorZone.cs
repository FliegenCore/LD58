using Assets._Game.Scripts.PlayerSystem;
using Game.Interactables;
using UnityEngine;

namespace Assets._Game.Scripts.InteractableVariants
{
    public class DoorZone : MonoBehaviour
    {
        [SerializeField] private Door _door;

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerView>())
            {
                _door.CanOpen = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerView>())
            {
                _door.CanOpen = false;
            }
        }
    }
}
