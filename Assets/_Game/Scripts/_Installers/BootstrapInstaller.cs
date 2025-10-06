using _Game.Scripts.Fader;
using _Game.Scripts.SceneManagment;
using _Game.Scripts.TranslateSystem;
using Game.ServiceLocator;
using System.Collections;
using UnityEngine;

namespace _Game.Scripts._Installers
{
    public class BootstrapInstaller : MonoBehaviour
    {
        private void Awake()
        {
            Register();
            InitializeServices();
        }

        private void Register()
        {
            G.Register(new Translator(), ServiceLifetime.Singleton);
            G.InstantiateAndRegisterService<RoutineStarter>(ServiceLifetime.Singleton);
            G.Register(new FadeController(), ServiceLifetime.Singleton);
            G.Register(new SceneController(), ServiceLifetime.Singleton);
        }

        private void InitializeServices()
        {
            G.InitializeServices();
            StartCoroutine(WaitChangeScene());
        }

        private IEnumerator WaitChangeScene()
        {
            WarningView warning = FindObjectOfType<WarningView>();
            string transltateTExt = Translator.Translate("warning");
            warning.WarningText.text = transltateTExt;
            yield return new WaitForSeconds(4f);

            G.Get<SceneController>().ChangeScene(SceneController.CORE_SCENE);
        }
    }
}