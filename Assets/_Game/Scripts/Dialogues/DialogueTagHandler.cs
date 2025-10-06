using _Game.Scripts.PlayerInput;
using Assets._Game.Scripts.PhotocameraSystem;
using Game.GallerySystem;
using Game.HorrorEventSystem;
using Game.MissionSystem;
using Game.NpcSystem;
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
                    G.Get<InputRoot>().Disable();
                    G.Get<PlayerController>().DisableMouseRotation();
                    G.Get<PlayerController>().DisableMove();
                }
                else if (tagComposite == "enablePlayerControl")
                {
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
                    if(G.Get<GalleryController>().HasPhoto())
                    {
                        G.Get<GalleryController>().OpenForSell();
                    }
                    else
                    {
                        DialogueSystem dialogueSystem = G.Get<DialogueSystem>();
                        dialogueSystem.StartDialogue(dialogueSystem.CurrentSpeaker.GetDialogue("no_goods_no_money"),
                            dialogueSystem.CurrentSpeaker);
                    }
                }
                else if (tagComposite == "buyFilm")
                {
                    G.Get<PhotocameraController>().TryBuyFilm();
                }
                else if(tagComposite == "disableCameraFunction")
                {
                    G.Get<PhotocameraController>().DisableInput();
                }
                else if (tagComposite == "enableCameraFunction")
                {
                    G.Get<PhotocameraController>().EnableInput();
                }
                else if (tagComposite == "startFilmTheChildrenMission")
                {
                    G.Get<MissionController>().StartFilmTheChildrenMission();
                }
                else if (tagComposite == "showEndWindow")
                {
                    G.Get<EndHorrorEventController>().ShowEndWindow();
                }
                else if (tagComposite == "showEndScreamer")
                {
                    G.Get<EndHorrorEventController>().Enable();
                }
                else if (tagComposite == "stay")
                {
                    SittedChild child = G.Get<DialogueSystem>().CurrentSpeaker as SittedChild;
                    child.Stay();
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