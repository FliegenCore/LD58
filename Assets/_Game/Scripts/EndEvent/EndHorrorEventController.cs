using _Game.Scripts._Installers;
using _Game.Scripts.Dialogues;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.TranslateSystem;
using Assets._Game.Scripts.EndEvent;
using Assets._Game.Scripts.MoneySystem;
using DG.Tweening;
using Game.MissionSystem;
using Game.Player;
using Game.ServiceLocator;
using System.Collections;
using UnityEngine;


namespace Game.HorrorEventSystem
{
    public class EndHorrorEventController : IService
    {
        private HorrorEventView _horrorEventView;

        public void Initialize()
        {
            _horrorEventView = Object.FindObjectOfType<HorrorEventView>();
        }

        public void Enable()
        {
            G.Get<InputRoot>().Disable();
            G.Get<PlayerController>().GetCamera().gameObject.SetActive(false);
            G.Get<MoneyController>().DisableView();
            G.Get<MissionController>().DisableView();
            Object.FindObjectOfType<MainCanvasView>().gameObject.SetActive(false);
            _horrorEventView.HorrorCamera.gameObject.SetActive(true);
            _horrorEventView.ScreamerSound.Play();
            G.Get<RoutineStarter>().StartCoroutine(StartDialogue());
            //disable viwes
        }

        private IEnumerator StartDialogue()
        {
            yield return new WaitForSeconds(2.5f);

            G.Get<DialogueSystem>().StartDialogue(_horrorEventView.Speaker.GetDialogue("why_are_you_doing_this_to_us"), _horrorEventView.Speaker);
        }

        public void ShowEndWindow()
        {
            _horrorEventView.EndCanvas.gameObject.SetActive(true);
            _horrorEventView.EndCanvas.DOFade(1, 1f);
            //think about the consequences before you do anything
        }
    }
}
