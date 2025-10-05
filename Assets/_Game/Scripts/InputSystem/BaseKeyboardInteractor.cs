using _Game.Scripts._Installers;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.TranslateSystem;
using Assets._Game.Scripts.PhotocameraSystem;
using Game.GallerySystem;
using Game.ServiceLocator;
using UnityEngine;

namespace Assets._Game.Scripts.InputSystem
{
    public class BaseKeyboardInteractor : KeyboardInteractor
    {
        private PhotocameraController _photocameraController;
        private IKeyboardAxisUser _currentKeyboardUser;
        private Raycaster _raycaster;
        private MainCanvasView _mainCanvasView;

        private Interactable _findedInteractable;
        private bool _galleryIsOpen;

        public void SetKeyboardMoveUser
            (IKeyboardAxisUser keyboardAxisUser,
            PhotocameraController photocameraController,
            Raycaster raycaster)
        {
            _mainCanvasView = Object.FindObjectOfType<MainCanvasView>();
            _raycaster = raycaster;
            _photocameraController = photocameraController;
            _currentKeyboardUser = keyboardAxisUser;
        }

        public override void SelfUpdate()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if(_currentKeyboardUser != null)
            {
                _currentKeyboardUser.Move(new Vector2(horizontal, vertical));
            }

            if(_photocameraController.CanSwitch)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    if (!_photocameraController.CanMakePhoto)
                        _photocameraController.EnablePhotocamera();
                    else
                        _photocameraController.DisablePhotocamera();
                }
            }
            
            if(_photocameraController.CanMakePhoto)
            {
                if(_photocameraController.GreenZoneGame.Played)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        _photocameraController.GreenZoneGame.TryMakePhoto();
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        _photocameraController.StartMakePhoto();
                    }
                }
            }
            else
            {
                FindInteractable();

                OpenCloseGallery();
            }
        }

        private void FindInteractable()
        {
            if (_raycaster.TryGetInteractable(out _findedInteractable))
            {
                _mainCanvasView.SetText(_findedInteractable.GetName());
                if (Input.GetKeyDown(KeyCode.E))
                    G.Get<RoutineStarter>().StartCoroutine(_findedInteractable.Interact());
            }
            else
            {
                _mainCanvasView.SetText("");

                if (_findedInteractable != null)
                    _findedInteractable = null;
            }
        }

        private void OpenCloseGallery()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!_galleryIsOpen)
                {
                    G.Get<GalleryController>().OpenGallery();
                    _galleryIsOpen = true;
                }
                else
                {
                    G.Get<GalleryController>().CloseGallery();
                    _galleryIsOpen = false;
                }
            }
        }

        public override bool TryInteract()
        {
            return true;
        }
    }
}
