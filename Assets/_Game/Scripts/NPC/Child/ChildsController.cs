using Game.NpcSystem;
using Game.ServiceLocator;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Game.Scripts.NPC.Child
{
    public class ChildsController : IService
    {
        private List<Limb> _allLimbs = new List<Limb>();

        public IReadOnlyList<Limb> AllLimbs => _allLimbs;

        public void Initialize()
        {
            FindAllChilds();
        }

        private void FindAllChilds()
        {
            List<Child> childs = UnityEngine.Object.FindObjectsOfType<Child>().ToList();

            foreach (Child child in childs)
            {
                _allLimbs.AddRange(child.Limbs);
            }
        }
    }
}
