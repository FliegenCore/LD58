using Assets._Game.Scripts.InputSystem;
using Game.Player;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.PlayerInput
{
    public class InputRoot : MonoBehaviour, IService
    {
        private Raycaster _raycaster;
        private MouseInteractor _currentMouseInteractor;
        private KeyboardInteractor _currentKeyboardInteractor;

        private BaseKeyboardInteractor _moveKeyboardInteractor;
        private MouseDeltaInteractor _mouseDeltaInteractor;

        private Camera _camera;
        private bool _enabled;

        public BaseKeyboardInteractor MoveKeyboardInteractor => _moveKeyboardInteractor;
        public MouseDeltaInteractor MouseDeltaInteractor => _mouseDeltaInteractor;
        public Raycaster Raycaster => _raycaster;

        public void Initialize()
        {
            _camera = Camera.main;
            _enabled = false;

            G.Get<PlayerController>().OnPlayerSpawned += PosInit;
        }

        private void PosInit()
        {
            _raycaster = new Raycaster(G.Get<PlayerController>().GetCamera());

            _moveKeyboardInteractor = new BaseKeyboardInteractor();
            _mouseDeltaInteractor = new MouseDeltaInteractor();
        }

        private void Update()
        {
            if (!_enabled)
                return;
            
            if(_currentMouseInteractor != null)
                _currentMouseInteractor.SelfUpdate();

            if(_currentKeyboardInteractor != null)
                _currentKeyboardInteractor.SelfUpdate();

            ChooseMouseInteractor();
            ChooseKeyboardInteractor();
        }

        public void Disable()
        {
            _enabled = false;
        }

        public void Enable()
        {
            _enabled = true;
        }

        private void ChooseMouseInteractor()
        {
            if(_mouseDeltaInteractor.TryInteract())
                _currentMouseInteractor = _mouseDeltaInteractor;
        }

        private void ChooseKeyboardInteractor()
        {
            if (_moveKeyboardInteractor.TryInteract())
                _currentKeyboardInteractor = _moveKeyboardInteractor;
        }

        private void OnDestroy()
        {
            G.Get<PlayerController>().OnPlayerSpawned -= PosInit;
        }
    }
}