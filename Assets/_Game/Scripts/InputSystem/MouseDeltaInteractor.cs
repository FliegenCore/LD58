using _Game.Scripts.PlayerInput;
using UnityEditor;
using UnityEngine;


namespace Assets._Game.Scripts.InputSystem
{
    public class MouseDeltaInteractor : MouseInteractor
    {
        private IMouseDeltaUser _mouseDeltaUser;

        public void SetMouseDeltaUser(IMouseDeltaUser mouseDeltaUser)
        {
            _mouseDeltaUser = mouseDeltaUser;
        }

        public override void SelfUpdate()
        {
            float mouseDeltaX = Input.GetAxis("Mouse X");
            float mouseDeltaY = Input.GetAxis("Mouse Y");

            if(_mouseDeltaUser != null)
            {
                _mouseDeltaUser.Direct(new Vector2(mouseDeltaX, mouseDeltaY));
            }
        }

        public override bool TryInteract()
        {
            float mouseDeltaX = Input.GetAxis("Mouse X");
            float mouseDeltaY = Input.GetAxis("Mouse Y");

            return true;
        }


    }
}
