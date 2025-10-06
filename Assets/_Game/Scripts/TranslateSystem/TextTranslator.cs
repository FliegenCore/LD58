using _Game.Scripts.TranslateSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets._Game.Scripts.TranslateSystem
{
    internal class TextTranslator : MonoBehaviour
    {
        private TMP_Text _text;

        private void Start()
        {
            _text = GetComponent<TMP_Text>();
            Translate();
        }

        private void Translate()
        {
            string translatedText = Translator.Translate(_text.text);

            _text.text = translatedText;
        }

    }
}
