using DG.Tweening;
using Game.GallerySystem;
using Game.ServiceLocator;
using System;
using UnityEngine;

namespace Assets._Game.Scripts.MoneySystem
{
    public class MoneyController : IService, IDisposable
    {
        private MoneyView _moneyView;
        private AudioSource _moneySound;
        private int _currentMoneyCount;

        public int CurrentMoneyCount => _currentMoneyCount;

        public void Initialize()
        {
            _moneyView = UnityEngine.Object.FindObjectOfType<MoneyView>();
            _moneySound = UnityEngine.Object.FindObjectOfType<MoneySound>().GetComponent<AudioSource>();
            _moneyView.MoneyText.text = $"{_currentMoneyCount}$";
            G.Get<GalleryController>().OnPhotoSelled += AddMoney;
        }

        public void DisableView()
        {
            _moneyView.gameObject.SetActive(false);
        }

        private void AddMoney(int count, int _)
        {
            _moneySound.Play();
            _currentMoneyCount += count;
            _moneyView.MoneyText.text = $"{_currentMoneyCount}$";
        }

        public void RemoveMoney(int count)
        {
            _moneySound.Play();
            _currentMoneyCount -= count;
            _moneyView.MoneyText.text = $"{_currentMoneyCount}$";
        }

        public void Dispose()
        {
            G.Get<GalleryController>().OnPhotoSelled -= AddMoney;
        }
    }
}
