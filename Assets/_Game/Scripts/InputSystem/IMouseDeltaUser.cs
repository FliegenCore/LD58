using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Game.Scripts.InputSystem
{
    public interface IMouseDeltaUser
    {
        void Direct(Vector2 direction);
    }
}
