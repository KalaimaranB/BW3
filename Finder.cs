using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finder : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private CursorManager cursorManager;
    [SerializeField]
    private FactionManager factionManager;
    [SerializeField]
    private ObjectPoolManager objectPoolManager;

    public static GameController GameManager;
    public static PlayerManager PlayerManager;
    public static LevelManager LevelManager;
    public static CursorManager CursorManager;
    public static FactionManager FactionManager;
    public static ObjectPoolManager ObjectPoolManager;

    private void Awake()
    {
        GameManager = gameController;
        PlayerManager = playerManager;
        LevelManager = levelManager;
        CursorManager = cursorManager;
        FactionManager = factionManager;
        ObjectPoolManager = objectPoolManager;
    }

}
