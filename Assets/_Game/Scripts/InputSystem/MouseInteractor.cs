using UnityEngine;

namespace _Game.Scripts.PlayerInput
{
    public abstract class MouseInteractor
    {
        public abstract bool TryInteract();

        public abstract void SelfUpdate();
        
        public bool OnClickUp()
        {
            if (Input.GetMouseButtonUp(0))
            {
                return true;
            }
            
            return false;
        }
    }
}