using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : Singleton<NavigationManager>
{
    private void Start()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Additive);
    }

    public void set_current_scene()
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync("Title");
        op.completed += (AsyncOperation result) =>
        {
            SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Additive);
        };
    }
}