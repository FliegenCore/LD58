using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Game.Scripts.PlayerSystem
{
    public class PlayerLookAt: MonoBehaviour
    {
        private Camera _camera;

        private Transform _target;
        private bool _enable;

        private void Awake()
        {
            _camera = GetComponent<PlayerView>().Camera;
        }

        private void Update()
        {
            if (!_enable)
                return;

            Look();
        }

        private void Look()
        {
            if (_target == null)
                return;

            _camera.transform.LookAt(_target);
        }

        public void EnableLookAt(Transform target)
        {
            _target = target;
            _enable = true;
        }

        public void DisableLookAt()
        {
            _enable = false;
        }

    }
}
