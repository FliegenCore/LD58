using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Dialogues
{
    public class ChoiceButton : MonoBehaviour
    {
        public Action<string, string[]> OnClick;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        private string _id;
        private string[] _tags;

        public string Id => _id;

        public void Init()
        {
            _button.onClick.AddListener(Click);
        }
        
        public void SetDialogueIdAndTags(string id, string[] tags)
        {
            _tags = tags;
            _id = id;
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private void Click()
        {
            OnClick?.Invoke(_id, _tags);
        }
    }
}