using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [Header("Level Data")]
    [SerializeField]
    private List<LevelDataSO> levelData;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Map Manager in the scene");
        }
        instance = this;
    }

    public LevelDataSO GetLevelData(int _level)
    {
        return levelData[_level];
    }
}
