using _Game.Scripts.TranslateSystem;
using Assets._Game.Scripts.InputSystem;
using Game.ServiceLocator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Dialogues
{
    public class Speaker: Interactable
    {
        [SerializeField] private Transform _lookAtPoint;
        [SerializeField] private string _keyName;
        [SerializeField] private List<Dialogue> _dialogues;
        
        public Transform LookAtPoint => _lookAtPoint;
        public List<Dialogue> Dialogues => _dialogues;
        private Dialogue _currentDialogue;

        public override string GetName()
        {
            string text = Translator.Translate(_keyName);

            return text;
        }

        public override IEnumerator Interact()
        {
            yield return null;
            G.Get<DialogueSystem>().StartDialogue(_currentDialogue, this);
        }

        public void SetDialogue(Dialogue dialogue)
        {
            _currentDialogue = dialogue;
        }
        
        public Dialogue GetDialogue(string id)
        {
            foreach (var dialogue in _dialogues)
            {
                if(dialogue.Id == id)
                    return dialogue;
            }
            
            return null;
        }
    }
}