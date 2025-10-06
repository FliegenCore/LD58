using Game.PhotocameraSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Game.Scripts.PhotocameraSystem
{
    public class PhotocameraView : MonoBehaviour
    {
        [SerializeField] private Image _photoZone;
        [SerializeField] private GreenZoneGame _greenZoneGame;
        [SerializeField] private TMP_Text _filmCountText;

        public GreenZoneGame GreenZoneGame => _greenZoneGame;
        public Image PhotoZone => _photoZone;
        public TMP_Text FilmCountText => _filmCountText;

        public void EnableGreenColor()
        {
            _photoZone.color = new Color(0.01435959f, 1f, 0f, 1f);
        }

        public void EnableOrangeColor()
        {
            _photoZone.color = new Color(1f, 0.5339025f, 0f, 1f);
        }

        public void EnableRedColor()
        {
            _photoZone.color = new Color(1f, 0f, 0.05694389f, 1f);
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
