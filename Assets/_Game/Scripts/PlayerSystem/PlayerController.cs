using _Game.Scripts.PlayerInput;
using Assets._Game.Scripts.PhotocameraSystem;
using Assets._Game.Scripts.PlayerSystem;
using Game.ServiceLocator;
using System;
using UnityEngine;

namespace Game.Player
{
    public class PlayerController : IService
    {
        public event Action OnPlayerSpawned;

        private PlayerLookAt _playerLookAt;
        private PlayerView _playerView;

        private InputRoot _inputRoot;

        public void Initialize()
        {
            _inputRoot = G.Get<InputRoot>();

            CreatePlayer();
            SetPlayerViewControl();
            //EnableMove();
            //EnableMouseRotation();
        }

        public Camera GetCamera()
        {
            return _playerView.Camera;
        }

        public void SetPlayerViewControl()
        {
            _inputRoot.MoveKeyboardInteractor.SetKeyboardMoveUser(_playerView, G.Get<PhotocameraController>(), _inputRoot.Raycaster);
            _inputRoot.MouseDeltaInteractor.SetMouseDeltaUser(_playerView);
            DisableCursor();
        }

        public void EnableLookAt(Transform transform)
        {
            _playerLookAt.EnableLookAt(transform);
        }

        public void DisableLookAt()
        {
            _playerLookAt.DisableLookAt();
        }

        public void DisableMove()
        {
            _playerView.DisableMove();
        }

        public void DisableMouseRotation()
        {
            _playerView.DisableMouseLook();
        }

        public void EnableMove()
        {
            _playerView.EnableMove();

        }

        public void EnableMouseRotation()
        {
            _playerView.EnableMouseLook();
        }

        public void DisableCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void EnableCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void CreatePlayer()
        {
            PlayerSpawnPoint spawnPoint = UnityEngine.Object.FindObjectOfType<PlayerSpawnPoint>();
            var asset = Resources.Load<PlayerView>("PlayerView");
            _playerView = UnityEngine.Object.Instantiate(asset, spawnPoint.transform.position, spawnPoint.transform.rotation);
            _playerLookAt = _playerView.GetComponent<PlayerLookAt>();
            OnPlayerSpawned?.Invoke();
        }
    }
}
