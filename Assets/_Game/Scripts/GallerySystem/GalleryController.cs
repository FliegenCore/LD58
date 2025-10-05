using Assets._Game.Scripts.GallerySystem;
using Assets._Game.Scripts.PhotocameraSystem;
using Game.ServiceLocator;
using System;
using UnityEngine;

namespace Game.GallerySystem
{
    public class GalleryController : IService, IDisposable
    {
        public event Action OnPhotoSelled;

        private GalleryView _galleryView;

        public void Initialize()
        {
            CreateGalleryView();

            _galleryView.Initialize();

            foreach(var photo in _galleryView.Photos)
            {
                photo.OnClickPhoto += SellPhoto;
            }
        }

        public void AddPhotoInGallery(PhotoData photoData)
        {
            foreach(var photo in _galleryView.Photos)
            {
                if(!photo.IsBusy)
                {
                    photo.SetInfo(photoData.Sprite, photoData.Price);
                    break;
                }
            }
        }

        private void SellPhoto(Photo photo)
        {
            //add money 
            photo.OnSellPhoto();
            OnPhotoSelled?.Invoke();
        }

        public void OpenForSell()
        {
            foreach (var photo in _galleryView.Photos)
            {
                photo.EnableInteractice();
            }

            OpenGallery();
        }

        public void OpenGallery()
        {
            _galleryView.Enable();
        }

        public void CloseGallery()
        {
            _galleryView.Disable();
        }

        private void CreateGalleryView()
        {
            var asset = Resources.Load<GalleryView>("GalleryView");
            _galleryView = UnityEngine.Object.Instantiate(asset);
            _galleryView.Initialize();
            _galleryView.Disable();
        }

        public void Dispose()
        {
            foreach (var photo in _galleryView.Photos)
            {
                photo.OnClickPhoto -= SellPhoto;
            }
        }
    }
}
