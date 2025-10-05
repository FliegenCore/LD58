using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Game.PhotocameraSystem
{
    public class GreenZoneGame : MonoBehaviour
    {
        public event Action OnWin;
        public event Action OnLose;

        [SerializeField] private RectTransform _rightPoint;
        [SerializeField] private RectTransform _leftPoint;
        [SerializeField] private RectTransform _arrow;
        [SerializeField] private RectTransform _greenZone;

        private RectTransform _target;

        private float _min;
        private float _max;

        private bool _played;

        public bool Played => _played;

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            _played = false;
            gameObject.SetActive(false);
        }

        public void StartMiniGame()
        {
            Enable();
            _target = _rightPoint;
            float x = UnityEngine.Random.Range(-300f, 300f);
            _arrow.anchoredPosition = new Vector2(0, _arrow.anchoredPosition.y);
            _greenZone.anchoredPosition = new Vector2(x, _greenZone.anchoredPosition.y);

            float min = _greenZone.anchoredPosition.x - _greenZone.rect.width;
            float max = _greenZone.anchoredPosition.x + _greenZone.rect.width;

            StartCoroutine(StartMoveArrow(min, max));
        }

        private IEnumerator StartMoveArrow(float min, float max)
        {
            _min = min;
            _max = max;
            _played = true;

            int direction = 1;

            while (_played)
            {
                float newX = _arrow.anchoredPosition.x + (600 * direction * Time.deltaTime);
                _arrow.anchoredPosition = new Vector2(newX, _arrow.anchoredPosition.y);

                if (_arrow.anchoredPosition.x >= _rightPoint.anchoredPosition.x)
                {
                    direction = -1;
                }
                else if (_arrow.anchoredPosition.x <= _leftPoint.anchoredPosition.x)
                {
                    direction = 1;
                }

                yield return null;
            }
        }

        public void TryMakePhoto()
        {
            if (_arrow.anchoredPosition.x > _min && _arrow.anchoredPosition.x < _max)
                Win();
            else
                Lose();
        }

        private void Win()
        {
            _played = false;
            OnWin?.Invoke();
            Disable();
        }

        private void Lose()
        {
            _played = false;
            OnLose?.Invoke();
            Disable();
        }
    }
}
