using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    [SerializeField]
    private PlayerStatsSO playerStats;
    [SerializeField]
    private PlayerStatsSO playerStatsTemplate;
    private GameObject playerObject;
    [SerializeField]
    private GameObject playerObjectPrefab;
    private Rigidbody2D playerRigidBody;
    private Vector2 playerMovementInput;

    [Header("Events")]
    [SerializeField]
    private GameEvent onPlayerAttack;
    [SerializeField]
    private GameEvent onPlayerMine;



    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Player Manager in the scene");
        }
        instance = this;

    }

    private void Start()
    {
        StartCoroutine(WaitForMeleeAttack());
        StartCoroutine(WaitForMiningAttack());
        if (!playerStats)
        {
            Debug.LogWarning("PlayerStats Scriptable Object not attached");
        }
    }

    private void OnDisable()
    {
        // is it necessary?
        StopCoroutine(WaitForMeleeAttack());
        StopCoroutine(WaitForMiningAttack());
    }

    //  TEMP
    //  Spawns player object. Sets player current stats to base stats from template.
    //  TODO - set starting stats through metod in playerStats scriptable object
    public void LoadPlayer()
    {
        Debug.Log("Loading Player");

        if (!playerObjectPrefab)
        {
            Debug.LogWarning("No Player Object Prefab attached to Player Manager");
        }

        playerObject = Instantiate(playerObjectPrefab, transform.parent);

        playerRigidBody = playerObject.GetComponent<Rigidbody2D>();

        playerStats.SetEnergy = playerStatsTemplate.GetEnergy;
        playerStats.SetMiningRange = playerStatsTemplate.GetMiningRange;
        playerStats.SetMiningPower = playerStatsTemplate.GetMiningPower;
        playerStats.SetMiningSpeed = playerStatsTemplate.GetMiningSpeed;
        playerStats.ClearMinedOres();
    }

    //
    //  Updates player movement input in Update
    //
    public void PlayerMovementInputUpdate()
    {
        playerMovementInput = PlayerMovement.movementUpdate();
    }

    //
    //  Updates player movement in Fixed Update (TODO other - physics on colliding with objects etc.)
    //
    public void PlayerMovementFixedUpdate()
    {
        PlayerMovement.movementFixedUpdate(playerRigidBody, playerMovementInput, playerStats.GetMovementSpeed);
    }

    //
    //  Returns colliders of layer "Enemy" in Attack Range
    //
    public Collider2D[] ReturnEnemiesInAttackRange()
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerObject.transform.position,
            playerStats.GetAttackRange, mask);

        int i = 0;
        while (i < hitColliders.Length)
        {
            //Debug.Log("Hit: " + hitColliders[i].gameObject.name + i);
            i++;
        }
        return hitColliders;
    }

    //
    //  Checks if there are colliders of layer "Enemy" in Attack Range
    //
    public bool CheckForEnemiesInAttackRange()
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerObject.transform.position,
            playerStats.GetAttackRange, mask);
        if (hitColliders.Length == 0)
        {
            return false;
        }
        return true;
    }

    //
    //  Returns colliders of layer "Environment" in Mining Range
    //
    public Collider2D[] ReturnEnvironmentInMiningRange()
    {
        LayerMask mask = LayerMask.GetMask("Environment");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerObject.transform.position,
            playerStats.GetMiningRange, mask);

        int i = 0;
        while (i < hitColliders.Length)
        {
            //Debug.Log("Mining hit: " + hitColliders[i].gameObject.name + i);
            i++;
        }
        return hitColliders;
    }

    //  ONGAMEEVENT/TEMP
    //  Adds damage to player current health
    //  TODO - include condition if health drops to 0 (game over, store mined ores etc.)
    public void DamagePlayer(Component _sender, object _data)
    {
        if (_data is float)
        {
            float damageTaken = (float)_data;
            playerStats.AddEnergy = -damageTaken;
            Debug.Log("Damage Taken " + damageTaken);
        }
    }
    
    //  ONGAMEEVENT
    //  Adds ore quantity to player current stats (TODO other - quantities later go through mults etc. on run end)
    //
    public void AddOreQuantityToCurrentStats(Component _sender, object _data)
    {
        if (_data is not (EnvironmentType, int))
        {
            Debug.LogError("Invalid data send to AddOreQuantityToCurrentStats in PlayerManager!");
            return;
        }

        (EnvironmentType, int) unpackedData = ((EnvironmentType, int))_data;
        playerStats.AddMinedOre(unpackedData.Item1, unpackedData.Item2);
        Debug.Log("Added " + unpackedData.Item2 + " " + unpackedData.Item1);
    }

    //
    //  Draws Attack Range as Red and Mining Range as Blue
    //
    private void OnDrawGizmos()
    {
        if (playerObject)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerObject.transform.position, playerStats.GetAttackRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(playerObject.transform.position, playerStats.GetMiningRange);
        }
    }

    //
    //  Player performs attack based on Attack Speed
    //
    private IEnumerator WaitForMeleeAttack()
    {
        while (true)
        {
            onPlayerAttack.Raise(this, (ReturnEnemiesInAttackRange(), playerStats.GetMeleeAttackDamage));
            yield return new WaitForSeconds(playerStats.GetMeleeAttackSpeed);
        }
    }

    //
    //  Player performs mining based on Mining Speed
    //
    private IEnumerator WaitForMiningAttack()
    {
        while (true)
        {
            onPlayerMine.Raise(this, (ReturnEnvironmentInMiningRange(), playerStats.GetMiningPower));
            yield return new WaitForSeconds(playerStats.GetMiningSpeed);
        }
    }

    //
    //  Returns player position
    //
    public Vector2 GetPlayerPosition()
    {
        return playerObject.transform.position;
    }
}
