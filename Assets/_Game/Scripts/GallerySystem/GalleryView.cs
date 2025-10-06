using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Game.Scripts.GallerySystem
{
    public class GalleryView : MonoBehaviour
    {
        [SerializeField] private Photo[] _photos;
        [SerializeField] private Button _closeButton;

        public Photo[] Photos => _photos;

        public void Initialize()
        {
            foreach(var photo in _photos)
            {
                photo.DisableInteracative();
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}
