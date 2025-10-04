using Game.Player;
using Game.ServiceLocator;
using System;
using UnityEngine;

namespace Assets._Game.Scripts.PhotocameraSystem
{
    public class PhotoMakeService : IService, IDisposable
    {
        private RenderTexture _renderTexture;
        private Camera _camera;
        private Texture2D _currentPhoto;

        public void Initialize()
        {
            G.Get<PlayerController>().OnPlayerSpawned += Init;
        }

        private void Init()
        {
            _camera = G.Get<PlayerController>().GetCamera();

            CreateRenderTexture();
        }

        private void CreateRenderTexture()
        {
            _renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            _renderTexture.Create();
        }

        public Texture2D MakePhoto()
        {
            if (_camera == null || G.Get<PhotocameraController>().PhotocameraView == null)
            {
                return null;
            }

            RenderTexture currentRT = RenderTexture.active;
            _camera.targetTexture = _renderTexture;
            RenderTexture.active = _renderTexture;

            _camera.Render();

            Texture2D fullScreenTexture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGB24, false);
            fullScreenTexture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            fullScreenTexture.Apply();

            Texture2D croppedPhoto = CropTextureToPhotoZone(fullScreenTexture);

            UnityEngine.Object.Destroy(fullScreenTexture);

            RenderTexture.active = currentRT;
            _camera.targetTexture = null;

            _currentPhoto = croppedPhoto;
            return _currentPhoto;
        }

        private Texture2D CropTextureToPhotoZone(Texture2D fullTexture)
        {
            Rect photoZoneRect = GetPhotoZonePixelRect();

            Texture2D croppedTexture = new Texture2D((int)photoZoneRect.width, (int)photoZoneRect.height, TextureFormat.RGB24, false);

            Color[] pixels = fullTexture.GetPixels(
                (int)photoZoneRect.x,
                (int)photoZoneRect.y,
                (int)photoZoneRect.width,
                (int)photoZoneRect.height
            );

            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            return croppedTexture;
        }

        private Rect GetPhotoZonePixelRect()
        {
            RectTransform photoZoneRT = G.Get<PhotocameraController>().PhotocameraView.PhotoZone.rectTransform;

            Vector3[] corners = new Vector3[4];
            photoZoneRT.GetWorldCorners(corners);

            Vector2 min = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
            Vector2 max = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

            float yMin = Screen.height - max.y;
            float yMax = Screen.height - min.y;

            return new Rect(min.x, yMin, max.x - min.x, yMax - yMin);
        }


        public Sprite ConvertToSprite(Texture2D texture)
        {
            if (texture == null) return null;
            
            return Sprite.Create(texture, 
                new Rect(0, 0, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f));
        }

        public void Dispose()
        {
            G.Get<PlayerController>().OnPlayerSpawned -= Init;
        }
    }

    public class PhotoData
    {
        
    }
}
