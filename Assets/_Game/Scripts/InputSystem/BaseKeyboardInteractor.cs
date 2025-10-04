using Assets._Game.Scripts.PhotocameraSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Game.Scripts.InputSystem
{
    public class BaseKeyboardInteractor : KeyboardInteractor
    {
        private PhotocameraController _photocameraController;
        private IKeyboardAxisUser _currentKeyboardUser;

        private bool _cameraIsEnabled;

        public void SetKeyboardMoveUser(IKeyboardAxisUser keyboardAxisUser, PhotocameraController photocameraController)
        {
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

            if(Input.GetMouseButtonDown(1))
            {
                if(!_cameraIsEnabled)
                {
                    _cameraIsEnabled = true;
                    _photocameraController.EnablePhotocamera();
                }
                else
                {
                    _cameraIsEnabled = false;
                    _photocameraController.DisablePhotocamera();
                }
            }
            if(_cameraIsEnabled)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    _photocameraController.StartMakePhoto();
                }
            }
        }

        public override bool TryInteract()
        {
            return true;
        }
    }
}
