using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Scriptable Objects/EnemyStatsSO")]
public class EnemyStatsSO : ScriptableObject
{
    [SerializeField]
    private Sprite enemySprite;
    public Sprite EnemySprite { get { return enemySprite; } }
    [SerializeField]
    private Color enemyColor;
    public Color EnemyColor { get { return enemyColor; } }
    [SerializeField]
    private string enemyName;
    public string EnemyName { get { return enemyName; } }
    [SerializeField]
    private EnemyType enemyType;
    public EnemyType EnemyType {  get { return enemyType; } }
    [SerializeField]
    private float maxHealth;
    public float MaxHealth { get { return maxHealth; } }
    [SerializeField]
    private float movementSpeed;
    public float MovementSpeed { get { return movementSpeed; } }
    [SerializeField]
    private float meleeDamage;
    public float MeleeDamage { get { return meleeDamage; } }
    [SerializeField]
    private float meleeAttackSpeed;
    public float MeleeAttackSpeed { get {return meleeAttackSpeed; } }
    [SerializeField]
    private float rangedDamage;
    public float RangedDamage { get { return rangedDamage; } }
    [SerializeField]
    private float rangedAttackSpeed;
    public float RangedAttackSpeed { get { return rangedAttackSpeed; } }
    [SerializeField]
    private float regenerationPerSecond;
    public float RegenerationPerSecond { get { return regenerationPerSecond; } }
    [SerializeField]
    private float knockbackResistance;
    public float KnockbackResistance { get { return knockbackResistance; } }

}
