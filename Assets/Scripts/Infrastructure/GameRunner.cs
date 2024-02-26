using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public static class GameRunner
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            private static void ToInitializeScene()
            {
                var scene = SceneManager.GetActiveScene();
                
                if (scene.buildIndex != 0)
                    SceneManager.LoadScene(0);
            }
    }
}