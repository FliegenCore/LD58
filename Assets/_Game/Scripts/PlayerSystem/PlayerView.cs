using Assets._Game.Scripts.InputSystem;
using UnityEngine;

namespace Assets._Game.Scripts.PlayerSystem
{
    internal class PlayerView : MonoBehaviour, IMouseDeltaUser, IKeyboardAxisUser
    {
        [SerializeField] private float _playerSpeed;
        [SerializeField] private float _horizontalSensivity;
        [SerializeField] private float _verticalSensivity;

        [SerializeField] private Camera _camera;
        [SerializeField] private CharacterController _characterController;

        private float currentXRotation = 0f;

        private bool _enableMouseLook;
        private bool _enableMovement;

        public Camera Camera => _camera;

        public void Direct(Vector2 direction)
        {
            if (!_enableMouseLook)
                return;
            currentXRotation -= direction.y * _verticalSensivity;
            currentXRotation = Mathf.Clamp(currentXRotation, -90f, 90f);

            _camera.transform.localEulerAngles = new Vector3(currentXRotation, 0, 0);

            transform.Rotate(0, direction.x * _horizontalSensivity, 0);
        }

        public void Move(Vector2 direction)
        {
            if (!_enableMovement)
                return;

            direction.Normalize();

            Vector3 cameraForward = _camera.transform.forward;
            Vector3 cameraRight = _camera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 movement = (cameraForward * direction.y + cameraRight * direction.x);

            movement.y = -0.5f;
            _characterController.Move(movement * (_playerSpeed * Time.deltaTime));
        }
        public void EnableMove()
        {
            _enableMovement = true;
        }

        public void EnableMouseLook()
        {
            _enableMouseLook = true;
        }

        public void DisableMove()
        {
            _enableMovement = false;
        }

        public void DisableMouseLook()
        {
            _enableMouseLook = false;
        }
    }
}
