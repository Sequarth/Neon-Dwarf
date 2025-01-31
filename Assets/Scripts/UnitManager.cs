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
    //private List<GameObject> enemyList;
    //private List<GameObject> environmentList;
    [SerializeField]
    private int enemyPoolAmount = 10;
    private List<GameObject> enemyPool;
    [SerializeField]
    private int environmentPoolAmount = 10;
    private List<GameObject> environmentPool;



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
    public void DestroyPoolingObjects()
    {
        if (enemyPool != null && enemyPool.Count > 0) { foreach (var enemy in enemyPool) { Destroy(enemy); } }
        if (environmentPool != null && environmentPool.Count > 0) { foreach (var enviro in environmentPool) { Destroy(enviro); } }

        enemyPool = null;
        environmentPool = null;
    }

    //
    //  Initializes pools of objects to use
    //
    public void InitializePoolingBatches()
    {
        if (enemyPool == null) { 
            enemyPool = new List<GameObject>();
            InstantiateEnemyPool(); 
        }
        else { 
            Debug.Log("Enemy Pool already exists - skipping instantiation of enemy objects," +
                " deactivating already existing ones"); 
            DeactivateEnemyPoolObjects();
        }

        if (environmentPool == null) { 
            environmentPool = new List<GameObject>();
            InstantiateEnvironmentPool(); 
        }
        else { 
            Debug.Log("Environment Pool already exists - skipping instantiation of environment objects, " +
                "deactivating already existing ones"); 
            DeactivateEnvironmentPoolObjects();
        }
    }

    //
    //  Instantiates enemy object pool
    //
    private void InstantiateEnemyPool()
    {
        for (int i = 0; i < enemyPoolAmount; i++)
        {
            GameObject spawnedEnemy = Instantiate(enemyObject, Vector3.zero, Quaternion.identity,
                unitListGameobject.transform);
            spawnedEnemy.SetActive(false);
            enemyPool.Add(spawnedEnemy);
        }
    }

    //
    //  Deactivates objects in enemy object pool
    //
    private void DeactivateEnemyPoolObjects()
    {
        foreach (var enemy in enemyPool) { enemy.SetActive(false); }
    }

    //
    //  Grabs available enemy object in the enemy object pool
    //
    private GameObject GetPooledEnemyObject()
    {
        foreach (var enemy in enemyPool) { 
            if (!enemy.activeInHierarchy) { return enemy; }
        }
        return null;
    }

    //
    //  Instantiates environment object pool
    //
    private void InstantiateEnvironmentPool()
    {
        for (int i = 0; i < environmentPoolAmount; i++)
        {
            GameObject spawnedEnvironment = Instantiate(environmentObject, Vector3.zero, Quaternion.identity,
                unitListGameobject.transform);
            spawnedEnvironment.SetActive(false);
            environmentPool.Add(spawnedEnvironment);
        }
    }

    //
    //  Deactivates objects in enemy object pool
    //
    private void DeactivateEnvironmentPoolObjects()
    {
        foreach (var environment in environmentPool) { environment.SetActive(false); }
    }

    //
    //  Grabs available environment object in the environment object pool
    //
    private GameObject GetPooledEnvironmentObject()
    {
        foreach (var environment in environmentPool)
        {
            if (!environment.activeInHierarchy) { return environment; }
        }
        return null;
    }

    //
    //  Spawns enemy basic prefab based on player position, attaches components and sets their value based on enemy type
    //
    private void SpawnEnemyFromTemplate(EnemyStatsSO _enemyStatsTemplate, Vector2 _playerPosition)
    {
        float radius = 10f;

        Vector2 randomPositionOnCircle = UnityEngine.Random.insideUnitCircle.normalized;
        randomPositionOnCircle *= radius;

        //  TODO - Change for object pooling
        //GameObject spawnedUnit = Instantiate(enemyObject, randomPositionOnCircle + _playerPosition,
        //    Quaternion.identity, unitListGameobject.transform);
        GameObject spawnedUnit = GetPooledEnemyObject();
        if (spawnedUnit ==  null)
        {
            Debug.Log("Achieved enemy spawn limit - skipping enemy spawn");
            return;
        }

        spawnedUnit.name = _enemyStatsTemplate.name;
        spawnedUnit.transform.position = _playerPosition + randomPositionOnCircle;

        SpriteRenderer enemySpriteRenderer = spawnedUnit.GetComponent<SpriteRenderer>();
        enemySpriteRenderer.sprite = _enemyStatsTemplate.EnemySprite;
        enemySpriteRenderer.color = _enemyStatsTemplate.EnemyColor;

        EnemyAttachable enemyAttachable = spawnedUnit.GetComponent<EnemyAttachable>();
        enemyAttachable.enemyType = _enemyStatsTemplate.EnemyType;
        enemyAttachable.currentHealth = _enemyStatsTemplate.MaxHealth;
        enemyAttachable.maxHealth = _enemyStatsTemplate.MaxHealth;
        enemyAttachable.currentMovementSpeed = _enemyStatsTemplate.MovementSpeed;
        enemyAttachable.damage = _enemyStatsTemplate.MeleeDamage;

        spawnedUnit.SetActive(true);
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
                collider.gameObject.SetActive(false);
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

        //  TODO - change to pooling
        //GameObject spawnedUnit = Instantiate(environmentObject, randomPositionOnCircle + _playerPosition,
        //    Quaternion.identity, unitListGameobject.transform);
        GameObject spawnedUnit = GetPooledEnvironmentObject();
        if (spawnedUnit == null)
        {
            Debug.Log("Achieved environment spawn limit - skipping environment spawn");
            return;
        }

        spawnedUnit.name = _environmentStatsTemplate.name;
        spawnedUnit.transform.position = _playerPosition + randomPositionOnCircle;

        SpriteRenderer environmentSpriteRenderer = spawnedUnit.GetComponent<SpriteRenderer>();
        environmentSpriteRenderer.sprite = _environmentStatsTemplate.EnvironmentSprite;
        environmentSpriteRenderer.color = _environmentStatsTemplate.EnvironmentColor;

        EnvironmentAttachable environmentAttachable = spawnedUnit.GetComponent<EnvironmentAttachable>();
        environmentAttachable.environmentType = _environmentStatsTemplate.EnvironmentType;
        environmentAttachable.currentHealth = _environmentStatsTemplate.MaxHealth;
        environmentAttachable.maxHealth = _environmentStatsTemplate.MaxHealth;

        spawnedUnit.SetActive(true);
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

                collider.gameObject.SetActive(false);
            }
        }
    }

}
