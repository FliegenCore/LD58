using Game.Player;
using Game.ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Game.Scripts.NPC.Child
{
    internal class ChildLookOnPlayer : MonoBehaviour
    {
        [SerializeField] private Child _child;
        [SerializeField] private Transform _head;

        private bool _enabled;

        private void Awake()
        {
            _child.OnFilmed += EnableLook;
        }

        private void LateUpdate()
        {
            if (!_enabled)
                return;

            Vector3 directionToCamera = G.Get<PlayerController>().GetCamera().transform.position - _head.position;
            _head.rotation = Quaternion.LookRotation(-directionToCamera);
        }

        private void EnableLook()
        {
            _enabled = true;
        }

        private void OnDestroy()
        {
            _child.OnFilmed -= EnableLook;
        }
    }
}
