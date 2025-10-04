using System;
using DG.Tweening;
using Game.ServiceLocator;
using Object = UnityEngine.Object;

namespace _Game.Scripts.Fader
{
    public class FadeController : IService
    {
        private FadeView _fadeView;
        
        public void Initialize()
        {
            _fadeView = Object.FindObjectOfType<FadeView>();
            
            Object.DontDestroyOnLoad(_fadeView.gameObject);
        }

        public void FadeIn(float duration = 0.5f, Action callback = null)
        {
            _fadeView.gameObject.SetActive(true);
            _fadeView.FadeImage.DOFade(1, duration)
                .OnComplete(() => callback?.Invoke());
        }

        public void FadeOut(float duration = 0.5f, Action callback = null)
        {
            _fadeView.FadeImage.DOFade(0, duration)
                .OnComplete(() =>
                {
                    callback?.Invoke();
                    _fadeView.gameObject.SetActive(false);
                });
        }

        
    }
}