using System;
using System.Collections.Generic;
using System.IO;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.TranslateSystem
{
    public class Translator : IService
    {
        private static TextAsset _globalAsset;
        private static int _indexLanguge;
        private static Dictionary<string, int> _languageIndexes = new Dictionary<string, int>();
        private static Dictionary<string, List<string>> _keyValueWords = new Dictionary<string,  List<string>>();
        
        public void Initialize()
        {
            LoadFile();
            LoadCurrentLanguage();
        }

        private void LoadCurrentLanguage()
        {
            SystemLanguage systemLanguage = Application.systemLanguage;
            
            if (systemLanguage == SystemLanguage.Russian)
            {
                _indexLanguge = 0;
            }
            else
            {
                _indexLanguge = 1;
            }
        }

        private void LoadFile()
        {
            _globalAsset = Resources.Load<TextAsset>("XML/Global");
            string[] strings = _globalAsset.text.Split('\n');
            
            //languages--------------
            string languages = strings[0];
            
            string[] languageArray = languages.Split(";");
            
            int startLanguageIndex = 1;

            for (int i = startLanguageIndex; i < languageArray.Length; i++)
            {
                _languageIndexes.Add(languageArray[i], i - 1);
            }

            //languages--------------

            string[] words = new string[strings.Length - 2];
            
            for (int i = 1; i < strings.Length - 1; i++)
            {
                words[i - 1] = strings[i];
            }

            foreach (var word in words)
            {
                string[] wordArray = word.Split(";");
                List<string> wordList = new List<string>();
                
                for (int i = 1; i < wordArray.Length; i++)
                {
                    wordList.Add(wordArray[i]);
                }
                
                _keyValueWords.Add(wordArray[0], wordList);
            }
        }

        public static string Translate(string keyVal)
        {
           string translatedText = _keyValueWords[keyVal][_indexLanguge];
           
           return translatedText;
        }
    }
}