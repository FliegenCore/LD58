using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Fader
{
    public class FadeView : MonoBehaviour
    {
        [SerializeField] private Image _fadeImage;
        
        public Image FadeImage => _fadeImage;
    }
}