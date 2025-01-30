using System;
using UnityEngine;
using System.Collections.Generic;

//
//  Type of enemy that signifies base stats and visuals
//
public enum EnemyType
{
    TEST = 0,
    SQUARE = 1,
    TRIANGLE = 2,
    CIRCLE = 3,
    HEXAGON = 4

}

//
//  Type of environment that signifies base stats and visuals
//
public enum EnvironmentType
{
    TEST = 0,
    COPPER = 1,
    IRON = 2,
    GOLD = 3

}

//
//  Main script responsible for spawning and managing units other than player
//  such as enemies or interactive environment objects
//
public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    [Header("Enemy data")]
    [SerializeField]
    private GameObject enemyObject;
    [SerializeField]
    private EnemyStatsSO enemyStatsTest;
    [SerializeField]
    private EnemyStatsSO enemySquareStats;
    [SerializeField]
    private EnemyStatsSO enemyTriangleStats;
    [SerializeField]
    private EnemyStatsSO enemyCircleStats;
    [SerializeField]
    private EnemyStatsSO enemyHexagonStats;

    [Header("Environment data")]
    [SerializeField]
    private GameObject environmentObject;
    [SerializeField]
    private EnvironmentStatsSO environmentStatsTest;
    [SerializeField]
    private EnvironmentStatsSO environmentCopperStats;
    [SerializeField]
    private EnvironmentStatsSO environmentIronStats;
    [SerializeField]
    private EnvironmentStatsSO environmentGoldStats;

    [Header("Events")]
    [SerializeField]
    private GameEvent onOreDestroyed;


    [Header("Unit Lists")]
    [SerializeField]
    private GameObject unitListGameobject;
    private List<GameObject> enemyList;
    private List<GameObject> environmentList;



    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Unit Manager in the scene");
        }
        instance = this;
    }

    //
    //  Destroys left over unit game objects (if any left) and creates new lists
    //
    public void ClearUnitLists()
    {
        if (enemyList != null) { foreach (var enemy in enemyList) { Destroy(enemy); } }
        if (environmentList != null) { foreach (var enviro in environmentList) { Destroy(enviro); } }

        enemyList = new List<GameObject>();
        environmentList = new List<GameObject>();
    }

    //
    //  Spawns enemy basic prefab based on player position, attaches components and sets their value based on enemy type
    //
    private void SpawnEnemyFromTemplate(EnemyStatsSO _enemyStatsTemplate, Vector2 _playerPosition)
    {
        float radius = 10f;

        Vector2 randomPositionOnCircle = UnityEngine.Random.insideUnitCircle.normalized;
        randomPositionOnCircle *= radius;

        GameObject spawnedUnit = Instantiate(enemyObject, randomPositionOnCircle + _playerPosition,
            Quaternion.identity, unitListGameobject.transform);
        spawnedUnit.name = _enemyStatsTemplate.name;

        SpriteRenderer enemySpriteRenderer = spawnedUnit.GetComponent<SpriteRenderer>();
        enemySpriteRenderer.sprite = _enemyStatsTemplate.EnemySprite;
        enemySpriteRenderer.color = _enemyStatsTemplate.EnemyColor;

        EnemyAttachable enemyAttachable = spawnedUnit.GetComponent<EnemyAttachable>();
        enemyAttachable.enemyType = _enemyStatsTemplate.EnemyType;
        enemyAttachable.currentHealth = _enemyStatsTemplate.MaxHealth;
        enemyAttachable.maxHealth = _enemyStatsTemplate.MaxHealth;
        enemyAttachable.currentMovementSpeed = _enemyStatsTemplate.MovementSpeed;
        enemyAttachable.damage = _enemyStatsTemplate.MeleeDamage;

        enemyList.Add(spawnedUnit);
    }

    //
    //  Takes enemy type and spawns such enemy based on player position
    //
    public void SpawnEnemy(EnemyType _enemyType, Vector2 _playerPosition)
    {
        switch (_enemyType)
        {
            case EnemyType.SQUARE:
                SpawnEnemyFromTemplate(enemySquareStats, _playerPosition);
                break;
            case EnemyType.TRIANGLE:
                SpawnEnemyFromTemplate(enemyTriangleStats, _playerPosition);
                break;
            case EnemyType.CIRCLE:
                SpawnEnemyFromTemplate(enemyCircleStats, _playerPosition);
                break;
            case EnemyType.HEXAGON:
                SpawnEnemyFromTemplate(enemyHexagonStats, _playerPosition);
                break;
            default:
                SpawnEnemyFromTemplate(enemyStatsTest, _playerPosition);
                break;
        }
    }

    //  
    //  Takes list of scanned colliders and subtracts value of applied player attack damage from enemy health value
    //  
    public void DealDamageToEnemiesInAttackRange(Component sender, object _data)
    {
        if (_data is not (Collider2D[], float))
        {
            Debug.LogError("Invalid data send to dealDamageToEnemiesInAttackRange in UnitManager!");
            return;
        }

        (Collider2D[], float) unpackedData = ((Collider2D[], float))_data;

        foreach (var collider in unpackedData.Item1)
        {
            Debug.Log("Attacking " + collider.gameObject.name);
            collider.gameObject.GetComponent<EnemyAttachable>().currentHealth -= unpackedData.Item2;
            if (collider.GetComponent<EnemyAttachable>().currentHealth <= 0)
            {
                Destroy(collider.gameObject);
            }
        }
    }
    

    //
    //  Spawns enemy basic prefab based on player position, attaches components and sets their value based on enemy type
    //
    private void SpawnEnvironmentFromTemplate(EnvironmentStatsSO _environmentStatsTemplate, Vector2 _playerPosition)
    {
        float radius = UnityEngine.Random.Range(15f, 30f);

        Vector2 randomPositionOnCircle = UnityEngine.Random.insideUnitCircle.normalized;
        randomPositionOnCircle *= radius;

        GameObject spawnedUnit = Instantiate(environmentObject, randomPositionOnCircle + _playerPosition,
            Quaternion.identity, unitListGameobject.transform);
        spawnedUnit.name = _environmentStatsTemplate.name;

        SpriteRenderer environmentSpriteRenderer = spawnedUnit.GetComponent<SpriteRenderer>();
        environmentSpriteRenderer.sprite = _environmentStatsTemplate.EnvironmentSprite;
        environmentSpriteRenderer.color = _environmentStatsTemplate.EnvironmentColor;

        EnvironmentAttachable environmentAttachable = spawnedUnit.GetComponent<EnvironmentAttachable>();
        environmentAttachable.environmentType = _environmentStatsTemplate.EnvironmentType;
        environmentAttachable.currentHealth = _environmentStatsTemplate.MaxHealth;
        environmentAttachable.maxHealth = _environmentStatsTemplate.MaxHealth;

        environmentList.Add(spawnedUnit);
    }

    //  
    //  Takes environment type and spawns such environment based on player position
    //  
    public void SpawnEvironment(EnvironmentType _environmentType, int _quantityToSpawn, Vector2 _playerPosition)
    {
        EnvironmentStatsSO environmentStats;

        switch (_environmentType)
        {
            case EnvironmentType.COPPER: environmentStats = environmentCopperStats; break;
            case EnvironmentType.IRON: environmentStats = environmentIronStats; break;
            case EnvironmentType.GOLD: environmentStats = environmentGoldStats; break;
            default: environmentStats = environmentStatsTest; break;
        }

        for (int i = 0; i < _quantityToSpawn; i++)
        {
            SpawnEnvironmentFromTemplate(environmentStats, _playerPosition);
        }
    }

    //  
    //  Takes list of scanned colliders and subtracts value of applied player mining power from environment health value
    //  
    public void DealDamageToEnvironmentInMiningRange(Component sender, object _data)
    {
        if (_data is not (Collider2D[], float))
        {
            Debug.LogError("Invalid data send to dealDamageToEnvironmentInMiningRange in UnitManager!");
            return;
        }

        (Collider2D[], float) unpackedData = ((Collider2D[], float))_data;

        foreach (var collider in unpackedData.Item1)
        {
            Debug.Log("Mining " + collider.gameObject.name);
            collider.gameObject.GetComponent<EnvironmentAttachable>().currentHealth -= unpackedData.Item2;
            if (collider.GetComponent<EnvironmentAttachable>().currentHealth <= 0)
            {
                int oreGained = 1;
                onOreDestroyed.Raise(this, (collider.GetComponent<EnvironmentAttachable>().environmentType, oreGained));

                Destroy(collider.gameObject);
            }
        }
    }

}
