using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Dialogues
{
    [Serializable]
    public class Dialogue
    {
        [SerializeField] private string _id;
        [SerializeField] private List<string>_startTagsId;
        [SerializeField] private List<DialogueLine> _lines;
        
        public List<DialogueLine> Lines => _lines;
        public List<string> StartTagsId => _startTagsId;
        public string Id => _id;
    }

    [Serializable]
    public class DialogueLine
    {
        [SerializeField] private string _speaker;
        [SerializeField] [TextArea(3, 5)] private string _text;
        [SerializeField] private List<string> _tags = new List<string>();
        [SerializeField] private List<DialogueChoice> _choices = new List<DialogueChoice>();

        public string Speaker => _speaker;
        public string Text => _text;
        public List<string> Tags => _tags;
        public List<DialogueChoice> Choices => _choices;
    }

    [Serializable]
    public class DialogueChoice
    {
        [SerializeField] [TextArea(3, 5)]  private string _text;
        [SerializeField] private string _targetDialogueId;
        [SerializeField] private List<string> _tags = new List<string>();

        public string Text => _text;
        public string TargetDialogueId => _targetDialogueId;
        public List<string> Tags => _tags;
    }
}