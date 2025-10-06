using _Game.Scripts.Dialogues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Game.Scripts.EndEvent
{
    public class HorrorEventView : MonoBehaviour
    {
        [SerializeField] private Speaker _speaker;
        [SerializeField] private Camera _camera;
        [SerializeField] private AudioSource _screamerSound;
        [SerializeField] private CanvasGroup _endCanvas;


        public CanvasGroup EndCanvas => _endCanvas;
        public Speaker Speaker => _speaker;
        public AudioSource ScreamerSound => _screamerSound;
        public Camera HorrorCamera => _camera;
    }
}
