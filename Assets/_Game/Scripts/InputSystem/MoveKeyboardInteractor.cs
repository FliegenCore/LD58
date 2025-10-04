using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Game.Scripts.InputSystem
{
    public class MoveKeyboardInteractor : KeyboardInteractor
    {
        private IKeyboardAxisUser _currentKeyboardUser;

        public void SetKeyboardMoveUser(IKeyboardAxisUser keyboardAxisUser)
        {
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
        }

        public override bool TryInteract()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
               return true;
            }

            return false;
        }
    }
}
