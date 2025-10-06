using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.TranslateSystem;
using DG.Tweening;
using Game.Player;
using Game.ServiceLocator;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Dialogues
{
    public class DialogueSystem : MonoBehaviour, IService
    {
        public event Action OnDialogueStart;

        private DialogueView _dialogueView;
        private DialogueTagHandler _tagHandler;

        private Speaker _speaker;
        private Dialogue _currentDialogue;
        private List<DialogueChoice> _currentChoices = new List<DialogueChoice>();
        private Coroutine _writeCoroutine;
        private AudioSource _writeSoundPrefab;
        private int _currentLine;
        private bool _canContinue;
        private bool _canSkip;
        private bool _dialogueEnd;
        private bool _canStart;
        private bool _isDialogueActive;

        public Speaker CurrentSpeaker => _speaker;

        public void Initialize()
        {
            _tagHandler = new DialogueTagHandler();

            var asset = Resources.Load<DialogueView>("DialogueView");
            _dialogueView = Instantiate(asset);
            _dialogueView.gameObject.SetActive(false);
            _writeSoundPrefab = Resources.Load<AudioSource>("WriteSound");

            InitButtons();
        }

        private void InitButtons()
        {
            foreach (var button in _dialogueView.ChoiceButtons)
            {
                button.Init();
                button.OnClick += SelectChoice;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (_canContinue)
                {
                    ContinueDialogue();
                    return;
                }
                SkipDialogue();
            }
        }

        private Sequence _bottomSequence;
        private Sequence _upperSequence;

        public void StartDialogue(Dialogue dialogue, Speaker speaker)
        {
            if (_isDialogueActive)
            {
                Debug.LogWarning("Dialogue is already active! Finishing previous dialogue first.");
                ForceEndDialogue();
            }

            _isDialogueActive = true;
            _speaker = speaker;
            _bottomSequence?.Kill();
            _upperSequence?.Kill();

            if (_writeCoroutine != null)
            {
                StopCoroutine(_writeCoroutine);
                _writeCoroutine = null;
            }

            G.Get<InputRoot>().Disable();

            foreach (var button in _dialogueView.ChoiceButtons)
            {
                button.Disable();
            }

            _dialogueView.gameObject.SetActive(true);
            if (dialogue != null)
                if (dialogue.StartTagsId.Count > 0)
                    UseTags(dialogue.StartTagsId.ToArray());

            _dialogueEnd = false;
            _dialogueView.SpeakerText.text = "";
            _dialogueView.Text.text = "";
            _bottomSequence = DOTween.Sequence();
            _upperSequence = DOTween.Sequence();
            _bottomSequence.Append(_dialogueView.BottomFrame.DOAnchorPosY(100, 0.5f)).OnComplete(() =>
            {
                _canSkip = true;
                _currentDialogue = dialogue;
                _currentLine = 0;
                DialogueProcess(_currentLine);
            });

            _upperSequence.Append(_dialogueView.TopFrame.DOAnchorPosY(-100, 0.5f));
        }

        private void ForceEndDialogue()
        {
            _isDialogueActive = false;
            _canSkip = false;
            _canContinue = false;
            _dialogueEnd = true;

            _bottomSequence?.Kill();
            _upperSequence?.Kill();

            if (_writeCoroutine != null)
            {
                StopCoroutine(_writeCoroutine);
                _writeCoroutine = null;
            }

            _dialogueView.gameObject.SetActive(false);

            G.Get<InputRoot>().Enable();
            G.Get<PlayerController>().DisableCursor();

            foreach (var button in _dialogueView.ChoiceButtons)
            {
                button.Disable();
            }
        }

        private void DialogueProcess(int lineNumber)
        {
            _canSkip = true;
            _canContinue = false;
            _currentChoices.Clear();

            if (_currentDialogue == null || lineNumber >= _currentDialogue.Lines.Count)
            {
                if (_dialogueEnd)
                {
                    return;
                }

                _canSkip = false;

                _dialogueEnd = true;
                if (_dialogueView != null)
                {
                    _bottomSequence = DOTween.Sequence();
                    _upperSequence = DOTween.Sequence();

                    _bottomSequence.Append(_dialogueView.BottomFrame.DOAnchorPosY(-100, 0.5f)).OnComplete(() =>
                    {
                        _canSkip = false;
                        _dialogueView.gameObject.SetActive(false);
                        _isDialogueActive = false; 
                        G.Get<InputRoot>().Enable();
                    });

                    _upperSequence.Append(_dialogueView.TopFrame.DOAnchorPosY(100, 0.5f));
                }

                return;
            }

            var line = _currentDialogue.Lines[lineNumber];
            string transltateSpeaker = Translator.Translate(line.Speaker);

            _dialogueView.SpeakerText.text = transltateSpeaker;

            _currentChoices.AddRange(line.Choices);

            string transltateText = Translator.Translate(line.Text);

            _writeCoroutine = StartCoroutine(WriteDialogue(transltateText, OnWriteComplete));
        }

        private void SelectChoice(string id, string[] tags)
        {
            G.Get<PlayerController>().DisableCursor();

            foreach (var button in _dialogueView.ChoiceButtons)
            {
                button.Disable();
            }

            if (id == string.Empty)
            {
                _canSkip = false;
                _upperSequence = DOTween.Sequence();
                _bottomSequence = DOTween.Sequence();
                _dialogueEnd = true;
                _bottomSequence.Append(_dialogueView.BottomFrame.DOAnchorPosY(-100, 0.5f)).OnComplete(() =>
                {
                    _canSkip = false;
                    _dialogueView.gameObject.SetActive(false);
                    _isDialogueActive = false; 
                    G.Get<InputRoot>().Enable();
                });

                _upperSequence.Append(_dialogueView.TopFrame.DOAnchorPosY(100, 0.5f));
            }

            UseTags(tags);

            if (id == string.Empty)
            {
                return;
            }

            StartDialogue(_speaker.GetDialogue(id), _speaker);
        }

        private void OnWriteComplete()
        {
            if (_currentChoices.Count > 0)
            {
                G.Get<PlayerController>().EnableCursor();
                for (int i = 0; i < _currentChoices.Count; i++)
                {
                    _dialogueView.ChoiceButtons[i].SetDialogueIdAndTags(_currentChoices[i].TargetDialogueId, _currentChoices[i].Tags.ToArray());
                    string choicesTextTranslate = Translator.Translate(_currentChoices[i].Text);
                    _dialogueView.ChoiceButtons[i].SetText(choicesTextTranslate);
                    _dialogueView.ChoiceButtons[i].Enable();
                }
            }
            else
            {
                _canContinue = true;
            }
        }

        private void ContinueDialogue()
        {
            if (!_canContinue)
            {
                return;
            }

            UseTags(_currentDialogue.Lines[_currentLine].Tags.ToArray());
            _currentLine++;
            DialogueProcess(_currentLine);
        }

        private void SkipDialogue()
        {
            if (!_canSkip)
            {
                return;
            }

            if (_writeCoroutine != null)
            {
                StopCoroutine(_writeCoroutine);
                _writeCoroutine = null;
            }
            OnWriteComplete();
            string transltateText = Translator.Translate(_currentDialogue.Lines[_currentLine].Text);
            _dialogueView.Text.text = transltateText;
        }

        private IEnumerator WriteDialogue(string text, Action callback)
        {
            string writedText = string.Empty;
            _dialogueView.Text.text = writedText;
            foreach (var letter in text)
            {
                if (!_isDialogueActive)
                {
                    yield break;
                }

                yield return new WaitForSeconds(0.02f);
                CreateClickSound();
                writedText += letter;
                _dialogueView.Text.text = writedText;
            }

            _canSkip = false;
            _writeCoroutine = null;
            callback?.Invoke();
        }

        private void UseTags(string[] tags)
        {
            _tagHandler.HandleTags(tags);
        }

        private void CreateClickSound()
        {
            float pitch = Random.Range(0.8f, 1.2f);
            AudioSource soundObj = Instantiate(_writeSoundPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            soundObj.pitch = pitch;
            soundObj.Play();

            Destroy(soundObj.gameObject, 0.2f);
        }

        private void OnDestroy()
        {
            foreach (var button in _dialogueView.ChoiceButtons)
            {
                button.OnClick -= SelectChoice;
            }
        }
    }
}