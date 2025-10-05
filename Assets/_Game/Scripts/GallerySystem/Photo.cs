using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Game.Scripts.GallerySystem
{
    public class Photo : MonoBehaviour
    {
        public event Action<Photo> OnClickPhoto;

        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _button;
        [SerializeField] private int _price;
        private bool _isBusy;

        public int Price => _price;
        public bool IsBusy => _isBusy;

        public void SetInfo(Sprite sprite, int price)
        {
            _isBusy = true;
            _image.sprite = sprite;
            _price = price;
        }

        public void EnableInteractice()
        {
            _button.enabled = true;
            _priceText.text = $"${_price}";
        }

        public void DisableInteracative()
        {
            _button.enabled = false;
            _priceText.text = "";
        }

        public void OnSellPhoto()
        {
            DisableInteracative();
            _image.sprite = null;
            _isBusy = false;
        }
    }
}
