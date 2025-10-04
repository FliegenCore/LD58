using Assets._Game.Scripts.NPC.Child;
using UnityEngine;

namespace Game.NpcSystem
{
    public enum LimpType
    {
        Leg,
        Hand,
        Head,
        Body
    }

    public class Limb : MonoBehaviour
    {
        [SerializeField] private LimpType _limbType;
        private Child _child;

        public LimpType LimpType => _limbType;
        public Child Child => _child;

        public void Initialize(Child child)
        {
            _child = child;
        }
    }
}
