using _Game.Scripts.Dialogues;
using Game.ServiceLocator;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Game.Scripts.NPC.Van
{
    public class VanView : Speaker
    {
        private void Start()
        {
            StartCoroutine(StartFirstDialogue());
        }

        private IEnumerator StartFirstDialogue()
        {
            yield return null;
            G.Get<DialogueSystem>().StartDialogue(GetDialogue("welcome_dialogue"), this);
        }

        public override IEnumerator Interact()
        {
            yield return null;
            G.Get<DialogueSystem>().StartDialogue(GetDialogue("base_dialogue"), this);
            base.Interact();
        }
    }
}
