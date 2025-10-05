using _Game.Scripts.PlayerInput;
using Game.GallerySystem;
using Game.Player;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.Dialogues
{
    public class DialogueTagHandler
    {

        
        public DialogueTagHandler()
        {
            
        }
        
        public void HandleTags(string[] tags)
        {
            foreach (var tag in tags)
            {
                string[] split = tag.Split('_');
                string tagComposite = split[0];
                string tagObject = string.Empty;
                if(split.Length > 1)
                    tagObject = split[1];

                if(tagComposite == "lookAtSpeaker")
                {
                    LookAtSpeaker();
                }
                else if(tagComposite == "disablePlayerControl")
                {
                    G.Get<PlayerController>().DisableMouseRotation();
                    G.Get<PlayerController>().DisableMove();
                }
                else if (tagComposite == "enablePlayerControl")
                {
                    Debug.Log("Enable player control");
                    G.Get<PlayerController>().EnableMove();
                    G.Get<PlayerController>().EnableMouseRotation();
                    G.Get<InputRoot>().Enable();
                }
                else if (tagComposite == "stopLookAtSpeaker")
                {
                    G.Get<PlayerController>().DisableLookAt();
                }
                else if (tagComposite == "Sell")
                {
                    G.Get<GalleryController>().OpenForSell();
                }
            }
        }

        private void LookAtSpeaker()
        {
            Transform point = G.Get<DialogueSystem>().CurrentSpeaker.LookAtPoint;
            G.Get<PlayerController>().EnableLookAt(point);
        }
    }
}