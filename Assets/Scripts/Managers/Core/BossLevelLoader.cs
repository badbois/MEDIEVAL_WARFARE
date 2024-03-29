using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelLoader : LevelLoader
{
    [SerializeField] private Boss _boss;

    public override void init()
    {
        base.init();
        _player_manager.update_dependencies(_main_camera);
        _player_manager.gameObject.SetActive(true);

        _boss.init(_player_manager.transform);
    }
}
