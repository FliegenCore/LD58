using _Game.Scripts.PlayerInput;
using Assets._Game.Scripts.NPC.Child;
using Game.NpcSystem;
using Game.Player;
using Game.ServiceLocator;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Game.Scripts.PhotocameraSystem
{
    public class PhotocameraController : MonoBehaviour, IService
    {
        private PhotocameraView _photocameraView;
        private Camera _camera;
        private ChildsController _childsController;
        private bool _cameraEnabled;

        public PhotocameraView PhotocameraView => _photocameraView;

        public void Initialize()
        {
            _childsController = G.Get<ChildsController>();
            G.Get<PlayerController>().OnPlayerSpawned += OnPlayerSpawn;
            CreateView();
        }

        private void OnPlayerSpawn()
        {
            _camera = G.Get<PlayerController>().GetCamera();
        }

        private void Update()
        {
            if(_cameraEnabled)
            {
                FindLimbs();
            }
        }

        private List<Limb> FindLimbs()
        {
            Rect frameBounds = GetFrameViewportBounds();

            if (_childsController.AllLimbs.Count <= 0)
            {
                return null;
            }

            List<Limb> findedLimbs = new List<Limb>();

            foreach(var limb in _childsController.AllLimbs)
            {
                Vector3 viewportPoint = _camera.WorldToViewportPoint(limb.transform.position);

                if (IsObjectInFrame(limb.gameObject))
                {
                    findedLimbs.Add(limb);
                }
            }

            var limbs = HandleFindedLimbs(findedLimbs);

            return limbs;
        }

        private List<Limb> HandleFindedLimbs(List<Limb> findedLimbs)
        {
            if (findedLimbs.Count <= 0)
            {
                _photocameraView.EnableRedColor();

                return null;
            }

            bool hasHead = false;

            List<Limb> raycastedLimbs = new List<Limb>();

            foreach(var limb in findedLimbs)
            {
                if(G.Get<InputRoot>().Raycaster.TryRaycastHitLimb(limb))
                {
                    if(limb.LimpType == LimpType.Head)
                    {
                        hasHead = true;
                    }
                    raycastedLimbs.Add(limb);
                }
            }

            if(hasHead)
            {
                _photocameraView.EnableGreenColor();
            }
            else
            {
                _photocameraView.EnableOrangeColor();
            }

            return raycastedLimbs;
        }

        public bool IsObjectInFrame(GameObject obj)
        {
            float distance = Vector3.Distance(obj.transform.position, G.Get<PlayerController>().GetCamera().transform.position);

            if(distance > 8)
            {
                return false;
            }

            Vector3 viewportPoint = _camera.WorldToViewportPoint(obj.transform.position);
            Rect frameBounds = GetFrameViewportBounds();

            bool inFrame = viewportPoint.x >= frameBounds.x &&
            viewportPoint.x <= frameBounds.x + frameBounds.width &&
            viewportPoint.y >= frameBounds.y &&
            viewportPoint.y <= frameBounds.y + frameBounds.height &&
            viewportPoint.z > 0;

            return inFrame;
        }

        private Rect GetFrameViewportBounds()
        {
            if (_photocameraView?.PhotoZone?.rectTransform == null)
            {
                Debug.LogError("PhotoZone rectTransform is null!");
                return new Rect(0, 0, 0, 0);
            }

            Canvas canvas = _photocameraView.PhotoZone.rectTransform.GetComponentInParent<Canvas>();

            if (canvas == null)
            {
                return new Rect(0, 0, 0, 0);
            }

            Vector3[] corners = new Vector3[4];
            _photocameraView.PhotoZone.rectTransform.GetWorldCorners(corners);

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                Vector2 min = corners[0];
                Vector2 max = corners[2];

                Vector2 viewportMin = new Vector2(min.x / Screen.width, min.y / Screen.height);
                Vector2 viewportMax = new Vector2(max.x / Screen.width, max.y / Screen.height);

                return new Rect(viewportMin.x, viewportMin.y,
                               viewportMax.x - viewportMin.x,
                               viewportMax.y - viewportMin.y);
            }

            return new Rect(0, 0, 0, 0);
        }

        public void EnablePhotocamera()
        {
            _cameraEnabled = true;
            _photocameraView.Enable();

        }

        public void DisablePhotocamera()
        {
            _cameraEnabled = false;
            _photocameraView.Disable();
        }

        public void StartMakePhoto()
        {
            if (!_cameraEnabled)
                return;

            MakePhoto();
            //start mini game
        }

        private void MakePhoto()
        {
            Texture2D tex = G.Get<PhotoMakeService>().MakePhoto();
            Sprite sprite = G.Get<PhotoMakeService>().ConvertToSprite(tex);
            _photocameraView.PrevewImage.sprite = sprite;

            List<Limb> raycastedLibs = FindLimbs();

            

            if(raycastedLibs != null && raycastedLibs.Count > 0)
            {
                //save in galery
            }
        }

        private void CreateView()
        {
            var asset = Resources.Load<PhotocameraView>("PhotocameraView");
            _photocameraView = Instantiate(asset);
            DisablePhotocamera();
        }

        private void OnDestroy()
        {
            G.Get<PlayerController>().OnPlayerSpawned -= OnPlayerSpawn;
        }
    }

    public class PhotoData
    {

    }
}
