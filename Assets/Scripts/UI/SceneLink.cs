using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneLink : MonoBehaviour
{
    [SerializeField]
    public MenuState target;

    private Button link_button;

    private void Start()
    {
        link_button = GetComponent<Button>();
        link_button.onClick.AddListener(handle_click);
    }

    // Update is called once per frame
    private void handle_click()
    {
        NavigationManager.Instance.set_current_scene();
    }
}