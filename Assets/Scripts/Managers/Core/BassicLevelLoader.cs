using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassicLevelLoader : LevelLoader
{
    [SerializeField] private CameraCrusher _camera_crusher;
    [SerializeField] private EnemySpawner _enemy_spawner;
    [SerializeField] private AudioClip _level_music;

    private RuneManager _rune_manager;

    public override void init()
    {
        // Set player position in parent
        base.init();

        // INIT DON'T DESTROY SCENE COMPONENTS
        StatsManager.Instance.init();

        // INIT PLAYER SCENE COMPONENTS
        _rune_manager = FindObjectOfType<RuneManager>();

        _rune_manager.init();
        _player_manager.init(_main_camera);
        _player_manager.gameObject.SetActive(true);

        // INIT LEVEL SCENE COMPONENTS
        _camera_crusher.init(_player_manager);
        _enemy_spawner.init();

        AudioSystem.Instance.play_music(_level_music);
    }
}
