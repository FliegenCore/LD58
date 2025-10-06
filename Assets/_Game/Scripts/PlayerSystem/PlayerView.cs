using _Game.Scripts.Dialogues;
using Assets._Game.Scripts.InputSystem;
using UnityEngine;

namespace Assets._Game.Scripts.PlayerSystem
{
    public class PlayerView : MonoBehaviour, IMouseDeltaUser, IKeyboardAxisUser
    {
        [SerializeField] private float _playerSpeed;
        [SerializeField] private float _horizontalSensivity;
        [SerializeField] private float _verticalSensivity;

        [SerializeField] private GameObject _screamer;
        [SerializeField] private AudioSource _screamerSound;
        [SerializeField] private AudioSource _stepSound;
        [SerializeField] private Camera _camera;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Speaker _speaker;

        private float _stepInterval = 0.7f;
        private float _minMovementThreshold = 0.1f;

        private float currentXRotation = 0f;
        private float _stepTimer = 0f;
        private Vector3 _lastPosition;
        private bool _isMoving = false;

        private bool _enableMouseLook;
        private bool _enableMovement;

        public AudioSource ScreamerSound => _screamerSound;
        public Speaker Speaker => _speaker;
        public Camera Camera => _camera;

        private void Start()
        {
            _lastPosition = transform.position;
        }

        private void Update()
        {
            UpdateStepSound();
        }

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

        private void UpdateStepSound()
        {
            if (!_enableMovement || !_stepSound)
                return;

            Vector3 currentPosition = transform.position;
            float movementDistance = Vector3.Distance(currentPosition, _lastPosition);
            _isMoving = movementDistance > _minMovementThreshold * Time.deltaTime;

            if (_isMoving && _characterController.isGrounded)
            {
                _stepTimer -= Time.deltaTime;

                if (_stepTimer <= 0f)
                {
                    PlayStepSound();
                    _stepTimer = _stepInterval;
                }
            }
            else
            {
                _stepTimer = 0f;
            }

            _lastPosition = currentPosition;
        }

        private void PlayStepSound()
        {
            if (_stepSound && !_stepSound.isPlaying)
            {
                _stepSound.Play();
            }
        }

        public void EnableScreamer()
        {
            _screamerSound.Play();
            _screamer.SetActive(true);
        }

        public void DisableScreamer()
        {
            _screamer.SetActive(false);
        }

        public void EnableMove()
        {
            _enableMovement = true;
            _lastPosition = transform.position; 
        }

        public void EnableMouseLook()
        {
            _enableMouseLook = true;
        }

        public void DisableMove()
        {
            _enableMovement = false;
            _isMoving = false;
            if (_stepSound && _stepSound.isPlaying)
            {
                _stepSound.Stop();
            }
        }

        public void DisableMouseLook()
        {
            _enableMouseLook = false;
        }
    }
}