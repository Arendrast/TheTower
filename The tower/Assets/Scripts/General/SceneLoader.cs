using MyCustomEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace General
{
    public class SceneLoader : MonoBehaviour, IObjectBeindInitialized
    {
        [SerializeField] private UnityEvent _onLoadScene;

        [EnumAction(typeof(SceneStorage.NamesOfScenes))]
        public void LoadScene(int enumIndex)
        {
            _onLoadScene?.Invoke();
            SceneManager.LoadScene(SceneStorage.IndexDictionary[(SceneStorage.NamesOfScenes) enumIndex]);
        }

        public void LoadSpecificScene(int index)
        {
            _onLoadScene?.Invoke();
            SceneManager.LoadScene(index);
        }

        public void ReloadScene()
        {
            _onLoadScene.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Initialize()
        {
            _onLoadScene.AddListener(() => Time.timeScale = 1);
        }
    }
}
