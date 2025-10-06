using _Game.Scripts.TranslateSystem;
using Assets._Game.Scripts.InputSystem;
using DG.Tweening;
using Game.Player;
using Game.ServiceLocator;
using System.Collections;
using UnityEngine;

namespace Game.Interactables
{
    public class Door : Interactable
    {
        [SerializeField] private AudioSource _openDoor;
        [SerializeField] private AudioSource _closedDoor;
        [SerializeField] private Transform _parent;

        private bool _isOpen;
        public bool CanOpen;

        public override string GetName()
        {
            string translationName = Translator.Translate("door");
            return translationName;
        }

        public override IEnumerator Interact()
        {
            yield return null;

            if(_isOpen)
            {
                yield break;
            }

            if(CanOpen)
            {
                OpenDoor();
            }
            else
            {
                DoCloseAnimation();
            }
        }

        private void DoCloseAnimation()
        {
            _closedDoor.Play();
            transform.DOPunchPosition(new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), 0.25f);
            G.Get<PlayerController>().StartMindDialogue("can_open_inside");
        }

        private void OpenDoor()
        {
            _openDoor.Play();
            enabled = false;
            _parent.DOLocalRotate(new Vector3(0, 120f, 0), 0.25f);
            _isOpen = true;
        }
    }
}
