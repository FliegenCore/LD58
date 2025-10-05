using Game.NpcSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Game.Scripts.NPC.Child
{
    public class Child : MonoBehaviour
    {
        [SerializeField] private List<Limb> _limbs;
        [SerializeField] private GameObject _greenPoint;

        public IReadOnlyList<Limb> Limbs => _limbs;

        private void Awake()
        {
            foreach(var limb in _limbs)
            {
                limb.Initialize(this);
            }
        }

        public void EnableGreenPoint()
        {
            _greenPoint.SetActive(true);
        }

        public void DisableGreenPoint()
        {
            _greenPoint.SetActive(false);
        }
    }
}
