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

        private PlayerView _playerView;

        private InputRoot _inputRoot;

        public void Initialize()
        {
            _inputRoot = G.Get<InputRoot>();

            CreatePlayer();
            SetPlayerViewControl();
        }

        public Camera GetCamera()
        {
            return _playerView.Camera;
        }

        public void SetPlayerViewControl()
        {
            _inputRoot.MoveKeyboardInteractor.SetKeyboardMoveUser(_playerView, G.Get<PhotocameraController>());
            _inputRoot.MouseDeltaInteractor.SetMouseDeltaUser(_playerView);
            _inputRoot.Enable();
            DisableCursor();
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
            var asset = Resources.Load<PlayerView>("PlayerView");
            _playerView = UnityEngine.Object.Instantiate(asset);

            OnPlayerSpawned?.Invoke();
        }
    }
}
