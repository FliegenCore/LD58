using _Game.Scripts.Dialogues;
using UnityEngine;

namespace Game.NpcSystem
{
    public class SittedChild : Speaker
    {
        [SerializeField] private Transform _stayPoint;
        [SerializeField] private Animator _animator;

        private void Awake()
        {
           _currentDialogue = GetDialogue("dad_didnt_come_to_my_birthday_again");
        }

        public void Stay()
        {
            transform.position = _stayPoint.position;
            transform.rotation = _stayPoint.rotation;
            _animator.Play("Laughing");
        }
    }
}
