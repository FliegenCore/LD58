using UnityEngine;

namespace Assets._Game.Scripts.InputSystem
{
    public interface IKeyboardAxisUser
    {
        void Move(Vector2 direction);
    }
}
