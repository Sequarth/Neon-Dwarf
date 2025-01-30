using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private PlayerManager playerManager;
    private UnitManager unitManager;
    private MapManager mapManager;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Game Manager in the scene");
        }
        instance = this;
    }
    void Start()
    {
        unitManager = UnitManager.instance;
        playerManager = PlayerManager.instance;
        mapManager = MapManager.instance;

        playerManager.LoadPlayer();
        unitManager.ClearUnitLists();

        //test
        //unitManager.SpawnEnemy(EnemyType.TEST, playerManager.GetPlayerPosition());
        unitManager.SpawnEnemy(EnemyType.SQUARE, playerManager.GetPlayerPosition());
        unitManager.SpawnEnemy(EnemyType.TRIANGLE, playerManager.GetPlayerPosition());
        unitManager.SpawnEnemy(EnemyType.CIRCLE, playerManager.GetPlayerPosition());
        unitManager.SpawnEnemy(EnemyType.HEXAGON, playerManager.GetPlayerPosition());

        StartCoroutine(SpawnEnvironment(3));
    }

    void Update()
    {
        playerManager.PlayerMovementInputUpdate();

    }

    private void FixedUpdate()
    {
        playerManager.PlayerMovementFixedUpdate();

    }

    private IEnumerator SpawnEnvironment(int _level)
    {
        LevelDataSO levelData = mapManager.GetLevelData(_level);
        Vector2 playerPosition;

        while (true)
        {
            playerPosition = playerManager.GetPlayerPosition();
            //Debug.Log("Distance from spawn point - " + Vector2.Distance(playerPosition, Vector2.zero));
            for (int id = 0; id < levelData.ListCount; id++)
            {
                if (Vector2.Distance(playerPosition, Vector2.zero) >= levelData.GetSpawnRange(id).x &&
                    Vector2.Distance(playerPosition, Vector2.zero) < levelData.GetSpawnRange(id).y)
                {
                    unitManager.SpawnEvironment(levelData.GetEnvironmentType(id), 
                        levelData.GetSpawnQuantity(id), playerPosition);
                }
            }
            yield return new WaitForSeconds(5f);
        }
    }
}
