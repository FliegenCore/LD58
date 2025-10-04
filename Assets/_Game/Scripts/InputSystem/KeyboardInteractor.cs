using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Game.Scripts.InputSystem
{
    public abstract class KeyboardInteractor
    {
        public abstract void SelfUpdate();
        public abstract bool TryInteract();
    }
}
