using UnityEngine;
using UnityEngine.UI;

namespace Assets._Game.Scripts.PhotocameraSystem
{
    public class PhotocameraView : MonoBehaviour
    {
        [SerializeField] private Image _photoZone;
        [SerializeField] private Image _prevewImage;

        public Image PhotoZone => _photoZone;
        public Image PrevewImage => _prevewImage;

        public void EnableGreenColor()
        {
            _photoZone.color = new Color(0.01435959f, 1f, 0f, 0.25f);
        }

        public void EnableOrangeColor()
        {
            _photoZone.color = new Color(1f, 0.5339025f, 0f, 0.25f);
        }

        public void EnableRedColor()
        {
            _photoZone.color = new Color(1f, 0f, 0.05694389f, 0.25f);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable() 
        {
            gameObject.SetActive(false);
        }
    }
}
