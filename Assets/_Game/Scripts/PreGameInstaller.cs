using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
    public class PreGameInstaller
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            if(SceneManager.GetActiveScene().buildIndex != 0)
                SceneManager.LoadScene(0);
        }
    }
}