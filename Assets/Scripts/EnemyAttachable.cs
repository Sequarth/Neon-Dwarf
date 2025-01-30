using System.Collections;
using UnityEngine;

public class EnemyAttachable : MonoBehaviour
{
    [Header("Stats")]
    public EnemyType enemyType;
    public float currentHealth;
    public float maxHealth;
    public float currentMovementSpeed;
    public float damage;

    //  Flags
    private bool attackingPlayer = false;
    private bool alreadyAttackedPlayer = false;

    private float attackPlayerCooldown = 0.25f;

    [Header("Events")]
    [SerializeField]
    private GameEvent onPlayerTouched;

    //
    //  Sets attackingPlayer flag to true after enemy enters PlayerColliderBox
    //  if first time entering - sets alreadyAttackedPlayer to true and starts WaitForAttack coroutine
    //
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.name == "PlayerColliderBox")
        {
            //Debug.Log("Entered PlayerColliderBox");
            if (alreadyAttackedPlayer == false)
            {
                alreadyAttackedPlayer = true;
                StartCoroutine(WaitForAttack());
            }
            attackingPlayer = true;
        }

    }

    //
    //  Sets attackingPlayer flag to false after exiting PlayerColliderBox
    //
    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.name == "PlayerColliderBox")
        {
            //Debug.Log("Exited PlayerColliderBox");
            attackingPlayer = false;
        }
    }

    //
    //  Draws enemy hitbox
    //
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }

    //
    //  Stops WaitForAttack coroutine
    //
    private void OnDisable()
    {
        StopCoroutine(WaitForAttack());
    }

    //
    //  Coroutine that calls game event to deal damage to the player with cooldown between attacks
    //
    private IEnumerator WaitForAttack()
    {
        while (true)
        {
            onPlayerTouched.Raise(this, damage);
            yield return new WaitForSeconds(attackPlayerCooldown);
            //Debug.Log("WaitingAAA");
            yield return new WaitUntil(() => attackingPlayer == true);
        }
    }
}
