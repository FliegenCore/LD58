using System;
using DG.Tweening;
using Game.ServiceLocator;
using Object = UnityEngine.Object;

namespace _Game.Scripts.Fader
{
    public class FadeController : IService
    {
        private FadeView _fadeView;

        private Sequence _inSequence;
        private Sequence _outSequence;
        
        public void Initialize()
        {
            _fadeView = Object.FindObjectOfType<FadeView>();
            
            Object.DontDestroyOnLoad(_fadeView.gameObject);
        }

        public void FadeIn(float duration = 0.5f, Action callback = null)
        {
            _fadeView.gameObject.SetActive(true);

            if (_outSequence != null)
                DOTween.Kill(_outSequence);

            _inSequence = DOTween.Sequence();

            _inSequence.Append(_fadeView.FadeImage.DOFade(1, duration))
                .OnComplete(() => callback?.Invoke());
        }

        public void FadeOut(float duration = 0.5f, Action callback = null)
        {
            if(_inSequence != null)
                DOTween.Kill(_inSequence);

            _outSequence = DOTween.Sequence();

            _outSequence.Append(_fadeView.FadeImage.DOFade(0, duration))
                .OnComplete(() =>
                {
                    callback?.Invoke();
                    _fadeView.gameObject.SetActive(false);
                });
        }

        
    }
}