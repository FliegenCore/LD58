using Assets._Game.Scripts.PhotocameraSystem;
using Game.NpcSystem;
using Game.Player;
using Game.ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Game.Scripts.NPC.Child
{
    public class ChildsController : IService, IDisposable
    {
        private List<Limb> _allLimbs = new List<Limb>();
        private List<Child> _children = new List<Child>();

        private int _childrenMarked;

        public IReadOnlyList<Limb> AllLimbs => _allLimbs;

        public void Initialize()
        {
            FindAllChilds();
            G.Get<PhotocameraController>().OnGoodFilmMaked += OnChildrenFilmMaked;
        }

        private void OnChildrenFilmMaked(int count)
        {
            _childrenMarked += count;

            if(_childrenMarked >= GetCount())
            {
                foreach(var child in _children)
                {
                    child.gameObject.SetActive(false);
                }

                G.Get<PlayerController>().PlayerView.ScreamerSound.Play();
            }
        }

        public int GetCount()
        {
            return _children.Count;
        }

        private void FindAllChilds()
        {
            _children = UnityEngine.Object.FindObjectsOfType<Child>().ToList();

            foreach (Child child in _children)
            {
                _allLimbs.AddRange(child.Limbs);
            }
        }

        public void Dispose()
        {
            G.Get<PhotocameraController>().OnGoodFilmMaked += OnChildrenFilmMaked;
        }
    }
}
