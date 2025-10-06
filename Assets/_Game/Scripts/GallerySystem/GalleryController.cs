using Assets._Game.Scripts.GallerySystem;
using Assets._Game.Scripts.PhotocameraSystem;
using Game.Player;
using Game.ServiceLocator;
using System;
using UnityEngine;

namespace Game.GallerySystem
{
    public class GalleryController : IService, IDisposable
    {
        public event Action<int, int> OnPhotoSelled;

        private GalleryView _galleryView;

        public bool GalleryIsOpen;

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
                    photo.SetInfo(photoData.Sprite, photoData.Price, photoData.ChildrenCount);
                    break;
                }
            }
        }

        private void SellPhoto(Photo photo)
        {
            OnPhotoSelled?.Invoke(photo.Price, photo.ChildCount);
            photo.OnSellPhoto();
        }

        public bool HasPhoto()
        {
            foreach(var photo in _galleryView.Photos)
            {
                if(photo.IsBusy)
                {
                    return true;
                }
            }

            return false;
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
            GalleryIsOpen = true;
            _galleryView.Enable();

            G.Get<PhotocameraController>().DisableInput();
            G.Get<PlayerController>().DisableMouseRotation();
            G.Get<PlayerController>().EnableCursor();
            G.Get<PlayerController>().DisableMove();
        }

        public void CloseGallery()
        {
            GalleryIsOpen = false;

            _galleryView.Disable();

            foreach (var photo in _galleryView.Photos)
            {
                photo.DisableInteracative();
            }
            
            G.Get<PhotocameraController>().EnableInput();
            G.Get<PlayerController>().EnableMouseRotation();
            G.Get<PlayerController>().DisableLookAt();
            G.Get<PlayerController>().DisableCursor();
            G.Get<PlayerController>().EnableMove();
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
