using Assets._Game.Scripts.InputSystem;
using Game.NpcSystem;
using UnityEngine;

namespace _Game.Scripts.PlayerInput
{
    public class Raycaster
    {
        private Camera _camera;

        public Raycaster(Camera camera)
        {
            _camera = camera;
        }

        public bool TryGetInteractable(out Interactable interactalbe)
        {
            interactalbe = null;

            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, 2))
            {
                if(hit.transform.TryGetComponent(out interactalbe))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryRaycastHitLimb(Limb limb)
        {
            Vector3 direction = limb.transform.position - _camera.transform.position;
            float maxDistance = direction.magnitude + 1f; 

            if (Physics.Raycast(_camera.transform.position, direction, out RaycastHit hit, maxDistance))
            {
                return hit.collider.gameObject == limb.gameObject;
            }

            return false;
        }
    }
}