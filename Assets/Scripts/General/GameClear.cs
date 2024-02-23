using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class GameClear : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                PlayerPrefs.DeleteAll();
                Time.timeScale = 1;
            }
        }
    }
}
