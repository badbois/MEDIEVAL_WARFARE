using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MenuUIManager : Singleton<MenuUIManager>
{
    [SerializeField]
    private TMP_Dropdown level_selector;
    [SerializeField]
    private TMP_Dropdown difficulty_selector;

    [SerializeField]
    private MenuState _initial_state;

    private MenuState _state;

    private void Start() => set_state(_initial_state);

    public void set_state(MenuState new_state)
    {
        switch(new_state)
        {
            case MenuState.TITLE:
                handle_title();
                break;
            case MenuState.MAIN:
                handle_main();
                break;
            case MenuState.SCORES:
                handle_scores();
                break;
            case MenuState.SHOP:
                handle_shop();
                break;
            case MenuState.GEARS:
                handle_gears();
                break;
            case MenuState.START_GAME:
                handle_start_game();
                break;
            case MenuState.REGISTER:
                handle_register();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(new_state), new_state, null);
        }
    }

    private void handle_title()
    {
        CanvasManager.Instance.set_current_menu(MenuState.TITLE);
        _state = MenuState.TITLE;
    }

    private void handle_main()
    {
        CanvasManager.Instance.set_current_menu(MenuState.MAIN);
        _state = MenuState.MAIN;
    }

    private void handle_scores()
    {
        CanvasManager.Instance.set_current_menu(MenuState.SCORES);
        _state = MenuState.SCORES;
    }

    private void handle_shop()
    {
        CanvasManager.Instance.set_current_menu(MenuState.SHOP);
        _state = MenuState.SHOP;
    }

    private void handle_gears()
    {
        CanvasManager.Instance.set_current_menu(MenuState.GEARS);
        _state = MenuState.GEARS;
    }

    private void handle_start_game()
    {
        string menu_selected = level_selector.options[level_selector.value].text;
        SceneManager.UnloadSceneAsync("main_menu");
        SceneManager.LoadSceneAsync(menu_selected, LoadSceneMode.Additive);
        set_state(MenuState.MAIN);
    }

    private void handle_register()
    {
        CanvasManager.Instance.set_current_menu(MenuState.REGISTER);
        _state = MenuState.REGISTER;
    }
}
