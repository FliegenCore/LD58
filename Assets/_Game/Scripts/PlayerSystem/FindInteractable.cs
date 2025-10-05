using _Game.Scripts.PlayerInput;
using Game.ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Game.Scripts.PlayerSystem
{
    public class FindInteractable : MonoBehaviour
    {
        private Raycaster _raycaster;

        public void Initialize()
        {
            _raycaster = G.Get<InputRoot>().Raycaster;
        }

        private void Update()
        {
            
        }

        public void Enable()
        {

        }

        public void Disable()
        {

        }
    }
}
