using _Game.Scripts.Dialogues;
using _Game.Scripts.TranslateSystem;
using Assets._Game.Scripts.MissionSystem;
using Assets._Game.Scripts.NPC.Child;
using Game.GallerySystem;
using Game.ServiceLocator;
using System;
using UnityEngine;

namespace Game.MissionSystem
{
    public class MissionController : IService, IDisposable
    {
        private MissionView _missionView;

        private int _filmMakedCount;

        public void Initialize()
        {
            SpawnView();

            _missionView.MissionText.text = "";
            G.Get<GalleryController>().OnPhotoSelled += OnGoodFilmSelled;
        }

        public void StartFilmTheChildrenMission()
        {
            string translatedText = Translator.Translate("film_the_children");
            int childCount = G.Get<ChildsController>().GetCount();
            _missionView.MissionText.text = $"{translatedText}: 0/{childCount}";
        }

        private void OnGoodFilmSelled(int _, int childCountSelled)
        {
            _filmMakedCount += childCountSelled;
            string translatedText = Translator.Translate("film_the_children");
            int childCount = G.Get<ChildsController>().GetCount();
            _missionView.MissionText.text = $"{translatedText}: {_filmMakedCount}/{childCount}";

            if(_filmMakedCount >= childCount)
            {
                OnMissionComplete();
            }
        }

        public void DisableView()
        {
            _missionView.gameObject.SetActive(false);
        }

        private void SpawnView()
        {
            var asset = Resources.Load<MissionView>("MissionView");
            _missionView = UnityEngine.Object.Instantiate(asset);
        }

        private void OnMissionComplete()
        {
            G.Get<GalleryController>().CloseGallery();

            DialogueSystem dialogueSystem = G.Get<DialogueSystem>();
            dialogueSystem.StartDialogue(dialogueSystem.CurrentSpeaker.GetDialogue("great_kids_for_my_collection"), dialogueSystem.CurrentSpeaker);
        }

        public void Dispose()
        {
            G.Get<GalleryController>().OnPhotoSelled -= OnGoodFilmSelled;

        }
    }
}
