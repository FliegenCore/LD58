using _Game.Scripts.Dialogues;
using _Game.Scripts.Fader;
using _Game.Scripts.PlayerInput;
using Assets._Game.Scripts.MoneySystem;
using Assets._Game.Scripts.NPC.Child;
using Game.GallerySystem;
using Game.NpcSystem;
using Game.PhotocameraSystem;
using Game.Player;
using Game.ServiceLocator;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Game.Scripts.PhotocameraSystem
{
    public class PhotocameraController : MonoBehaviour, IService
    {
        public event Action<int> OnGoodFilmMaked;
        public event Action OnStartMiniGame;
        public event Action OnCameraDisable;

        private PhotocameraView _photocameraView;
        private Camera _camera;
        private ChildsController _childsController;
        private GreenZoneGame _greenZoneGame;
        private AudioSource _cameraSound;
        private MainCanvasView _mainCanvasView;
        private bool _cameraEnabled;
        private List<Child> _markedChilds = new List<Child>();
        private bool _canSwitch = true;
        private bool _canMakePhoto;

        private int _childrensMarked;
        private int _filmsCount;
        private bool _enabled;

        public int ChildrenMarked => _childrensMarked;
        public bool CanSwitch => _canSwitch;
        public bool CanMakePhoto => _canMakePhoto;
        public PhotocameraView PhotocameraView => _photocameraView;
        public GreenZoneGame GreenZoneGame => _greenZoneGame;

        public void Initialize()
        {
            _childsController = G.Get<ChildsController>();
            _mainCanvasView = FindObjectOfType<MainCanvasView>();
            _cameraSound = FindObjectOfType<CameraSound>().GetComponent<AudioSource>();
            G.Get<PlayerController>().OnPlayerSpawned += OnPlayerSpawn;
            //G.Get<DialogueSystem>().OnDialogueStart += OnDialogueStart;
            CreateView();
            _greenZoneGame = _photocameraView.GreenZoneGame;

            _greenZoneGame.OnWin += MakePhoto;
            _greenZoneGame.OnLose += OnLoseMiniGame;
            _greenZoneGame.Disable();
            _filmsCount = 3;
            _photocameraView.FilmCountText.text = $"{_filmsCount}";
            _mainCanvasView.FilmCountText.text = $"{_filmsCount}";
            EnableInput();
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

        public void TryBuyFilm()
        {
            if(G.Get<MoneyController>().CurrentMoneyCount < 50)
            {
                DialogueSystem dialogueSystem = G.Get<DialogueSystem>();
                Speaker speaker = dialogueSystem.CurrentSpeaker;
                dialogueSystem.StartDialogue(speaker.GetDialogue("no_money_no_honey"), speaker);

                return;
            }

            G.Get<MoneyController>().RemoveMoney(50);
            AddFilm();
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

            bool hasRaycasted = false;
            bool hasFullBody = false;
            List<Limb> raycastedLimbs = new List<Limb>();

            foreach(var limb in findedLimbs)
            {
                if(G.Get<InputRoot>().Raycaster.TryRaycastHitLimb(limb))
                {
                    hasRaycasted = true;
                    raycastedLimbs.Add(limb);
                }
            }

            foreach(var limb in raycastedLimbs)
            {
                if(limb.Child.AllLimbsContains(raycastedLimbs))
                {
                    hasFullBody = true;
                }
            }

            if(hasRaycasted)
            {
                if (hasFullBody)
                {
                    _photocameraView.EnableGreenColor();
                }
                else
                {
                    _photocameraView.EnableOrangeColor();
                }
            }
            else
            {
                _photocameraView.EnableRedColor();
            }

            return raycastedLimbs;
        }

        public bool IsObjectInFrame(GameObject obj)
        {
            float distance = Vector3.Distance(obj.transform.position, G.Get<PlayerController>().GetCamera().transform.position);

            if(distance > 7)
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

        public void DisableInput()
        {
            _enabled = false;
        }

        public void EnableInput()
        {
            _enabled = true;
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
            if (!_enabled)
                return;

            if (_filmsCount <= 0)
            {
                G.Get<PlayerController>().StartMindDialogue("no_film");
                return;
            }

            _canSwitch = false;
            G.Get<PlayerController>().DisableMove();

            G.Get<FadeController>().FadeIn(callback: () =>
            {
                G.Get<FadeController>().FadeOut(callback: () =>
                {
                    _canSwitch = true;
                });

                _canMakePhoto = true;
                _cameraEnabled = true;
                _photocameraView.Enable();

                EnableGreenPointOnMarkeredChilds();
            });
        }

        public void DisablePhotocamera()
        {
            if (!_enabled)
                return;

            _greenZoneGame.Disable();
            _canSwitch = false;

            G.Get<FadeController>().FadeIn(duration: 0.25f, callback: () =>
            {
                G.Get<FadeController>().FadeOut(duration: 0.25f, callback: () =>
                {
                    _canSwitch = true;
                });
                OnCameraDisable?.Invoke();
                _canMakePhoto = false;
                _cameraEnabled = false;
                _photocameraView.Disable();
                G.Get<PlayerController>().EnableMove();
                G.Get<PlayerController>().EnableMouseRotation();

                foreach (var child in _markedChilds)
                {
                    child.DisableGreenPoint();
                }
            });
        }

        public void StartMakePhoto()
        {
            if (!_cameraEnabled)
                return;

            if(_filmsCount <= 0)
            {
                return;
            }

            OnStartMiniGame?.Invoke();
            G.Get<PlayerController>().DisableMouseRotation();
            _greenZoneGame.StartMiniGame();
        }

        private void OnLoseMiniGame()
        {
            RemoveFilm();
            DisablePhotocamera();
        }

        public void AddFilm()
        {
            _filmsCount++;
            _mainCanvasView.FilmCountText.text = $"{_filmsCount}";
            _photocameraView.FilmCountText.text = $"{_filmsCount}";
        }

        private void RemoveFilm()
        {
            _filmsCount--;
            _photocameraView.FilmCountText.text = $"{_filmsCount}";
            _mainCanvasView.FilmCountText.text = $"{_filmsCount}";
        }

        private void EnableGreenPointOnMarkeredChilds()
        {
            foreach (var child in _markedChilds)
            {
                child.EnableGreenPoint();
            }
        }

        private void MakePhoto()
        {
            //add effect
            _cameraSound.Play();
            Texture2D tex = G.Get<PhotoMakeService>().MakePhoto();
            Sprite sprite = G.Get<PhotoMakeService>().ConvertToSprite(tex);

            List<Limb> raycastedLibs = FindLimbs();

            if(raycastedLibs != null && raycastedLibs.Count > 0)
            {
                List<Child> childs = new List<Child>();

                foreach(var limb in raycastedLibs.ToArray())
                {
                    if(_markedChilds.Contains(limb.Child))
                    {
                        raycastedLibs.Remove(limb);
                        continue;
                    }

                    if (!childs.Contains(limb.Child))
                        childs.Add(limb.Child);
                }

                foreach(var child in childs.ToArray())
                {
                    if(!_markedChilds.Contains(child))
                    {
                        _markedChilds.Add(child);
                        child.DoOnFilmed();
                    }
                    else
                        childs.Remove(child);
                }

                EnableGreenPointOnMarkeredChilds();

                int price = CalculatePhotoPrice(raycastedLibs);
                
                PhotoData photoData = new PhotoData
                {
                    Price = price,
                    Sprite = sprite,
                    ChildrenCount = childs.Count
                };

                if (price > 0)
                {
                    _childrensMarked += childs.Count;
                    G.Get<GalleryController>().AddPhotoInGallery(photoData);
                    OnGoodFilmMaked?.Invoke(childs.Count);
                }
            }

            RemoveFilm();
            DisablePhotocamera();
        }

        private int CalculatePhotoPrice(List<Limb> raycastedLibs)
        {
            int price = 0;

            foreach(var limb in raycastedLibs)
            {
                if(limb.LimpType == LimpType.Head)
                    price += 30;
                else if(limb.LimpType == LimpType.Body)
                    price += 50;
                else if(limb.LimpType == LimpType.Hand)
                    price += 10;
            }

            return price;
        }

        private void OnDialogueStart()
        {
            HardDisablePhotocamera();
            _greenZoneGame.Disable();
        }

        public void HardDisablePhotocamera()
        {
            _canSwitch = true;
            _canMakePhoto = false;
            _cameraEnabled = false;
            _photocameraView.Disable();

            foreach (var child in _markedChilds)
            {
                child.DisableGreenPoint();
            }
        }

        private void CreateView()
        {
            var asset = Resources.Load<PhotocameraView>("PhotocameraView");
            _photocameraView = Instantiate(asset);
            HardDisablePhotocamera();
        }

        private void OnDestroy()
        {
            G.Get<PlayerController>().OnPlayerSpawned -= OnPlayerSpawn;
            _greenZoneGame.OnWin -= MakePhoto;
            G.Get<DialogueSystem>().OnDialogueStart -= OnDialogueStart;
        }
    }

    public class PhotoData
    {
        public Sprite Sprite;
        public int Price;
        public int ChildrenCount;
        //add limbs for dialogues
    }
}
