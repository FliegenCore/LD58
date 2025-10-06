using Assets._Game.Scripts.PhotocameraSystem;
using Game.ServiceLocator;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Game.DaySystem
{
    public class DayController : MonoBehaviour, IService
    {
        [SerializeField] private GameObject _directionalLight;
        [SerializeField] private Material[] _materials;
        [SerializeField] private Vector3[] _color;
        [SerializeField] private TMP_Text _tutorialText;

        private int _currentMaterial;

        public void Initialize()
        {
            G.Get<PhotocameraController>().OnGoodFilmMaked += OnChildMarked;
        }

        private void OnChildMarked(int count)
        {
            _currentMaterial += count;

            if (_currentMaterial >= 5)
            {
                _tutorialText.gameObject.SetActive(false);
            }

            RenderSettings.skybox = _materials[_currentMaterial];
            Vector3 color = _color[_currentMaterial];
            
            if(_currentMaterial >= 4)
            {
                _directionalLight.SetActive(false);
            }

            RenderSettings.ambientEquatorColor = new Color(color.x, color.y, color.z);
            DynamicGI.UpdateEnvironment();
        }
    }
}
