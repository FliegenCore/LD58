using System.Collections;
using _Game.Scripts.Fader;
using Game.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.SceneManagment
{
    public class SceneController : IService
    {
        public const string CORE_SCENE = "CoreScene";
        public const string BOOTSTRAP_SCENE = "BootstrapScene";

        private FadeController _fadeController;
        
        public void Initialize()
        {
            _fadeController = G.Get<FadeController>();
        }
        
        public void ChangeScene(string sceneName)
        {
            _fadeController.FadeIn(2,callback: () =>
            {
                G.UnregisterAllDestroyed();
                
                SceneManager.LoadScene(sceneName);
                
                _fadeController.FadeOut(1f);
            });
        }
    }
}