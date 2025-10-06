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
        public event Action OnFilmed;

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

        public bool AllLimbsContains(List<Limb> limbs)
        {
            int count = 0;

            foreach(var limb in limbs)
            {
                if(_limbs.Contains(limb))
                {
                    count++;
                }
            }

            

            if (count >= _limbs.Count)
                return true;

            return false;
        }

        public void DoOnFilmed()
        {
            OnFilmed?.Invoke();
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
