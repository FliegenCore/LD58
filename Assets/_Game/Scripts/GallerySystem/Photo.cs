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
        [SerializeField] private int _childCount;
        private bool _isBusy;


        public int ChildCount => _childCount;
        public int Price => _price;
        public bool IsBusy => _isBusy;

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }
        
        private void OnClick()
        {
            OnClickPhoto?.Invoke(this);
        }

        public void SetInfo(Sprite sprite, int price, int childCount)
        {
            _childCount = childCount;
            _isBusy = true;
            _image.sprite = sprite;
            _price = price;
            _priceText.text = $"${_price}";
        }

        public void EnableInteractice()
        {
            _button.enabled = true;
            _priceText.text = $"${_price}";
        }

        public void DisableInteracative()
        {
            _button.enabled = false;
            _priceText.text = $"${_price}";
        }

        public void OnSellPhoto()
        {
            DisableInteracative();
            _childCount = 0;
            _image.sprite = null;
            _isBusy = false;
            _priceText.text = $"${0}";
        }
    }
}
