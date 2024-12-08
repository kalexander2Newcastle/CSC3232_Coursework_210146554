using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadHubScene : MonoBehaviour
{
    [SerializeField] private string hubSceneName = "Hub";
    [SerializeField] private string levelSceneName = "Level 1";


    /// <summary>
    /// This method is called to load the Hub scene and Level 1 scene additively and unload the Menu scene.
    /// Attach it to a button's OnClick event.
    /// </summary>
    public void LoadHubAndLevel()
    {
            SceneManager.LoadScene(hubSceneName);
            SceneManager.LoadScene(levelSceneName, LoadSceneMode.Additive);
    }
}

