using TMPro;
using UnityEngine;

namespace _Game.Scripts.Dialogues
{
    public class DialogueView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _speakerText;
        [SerializeField] private ChoiceButton[] _choiceButtons;
        [SerializeField] private RectTransform _topFrame;
        [SerializeField] private RectTransform _bottomFrame;
        
        public RectTransform TopFrame => _topFrame;
        public RectTransform BottomFrame => _bottomFrame;
        public ChoiceButton[] ChoiceButtons => _choiceButtons;
        public TMP_Text Text => _text;
        public TMP_Text SpeakerText => _speakerText;
    }
}