using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Scriptable Objects/PlayerStatsSO")]
public class PlayerStatsSO : ScriptableObject
{
    [SerializeField]
    private float energy;
    public float GetEnergy { get { return energy; } }
    public float AddEnergy { set { energy += value; } }
    public float SetEnergy { set { energy = value; } }
    [SerializeField]
    private float movementSpeed;
    public float GetMovementSpeed { get { return movementSpeed; } }
    public float SetMovementSpeed { set { movementSpeed = value; } }
    public float AddMovementSpeed { set { movementSpeed += value; } }
    [SerializeField]
    private float miningPower;
    public float GetMiningPower { get { return miningPower; } }
    public float SetMiningPower { set { miningPower = value; } }
    public float AddMiningPower { set { miningPower += value; } }
    [SerializeField]
    private float miningSpeed;
    public float GetMiningSpeed { get {return miningSpeed; } }
    public float SetMiningSpeed { set { miningSpeed = value; } }
    public float AddMiningSpeed { set { miningSpeed += value; } }
    [SerializeField]
    private float miningRange;
    public float GetMiningRange { get { return miningRange; } }
    public float SetMiningRange { set { miningRange = value; } }
    public float AddMiningRange { set { miningRange += value; } }
    [SerializeField]
    private float meleeAttackDamage;
    public float GetMeleeAttackDamage { get { return meleeAttackDamage; } }
    public float SetMeleeAttackDamage { set { meleeAttackDamage = value; } }
    public float AddMeleeAttackDamage { set { meleeAttackDamage += value; } }
    [SerializeField] 
    private float meleeAttackSpeed;
    public float GetMeleeAttackSpeed { get { return meleeAttackSpeed; } }
    public float SetMeleeAttackSpeed { set { meleeAttackSpeed = value; } }
    public float AddMeleeAttackSpeed { set { meleeAttackSpeed += value; } }
    [SerializeField]
    private float attackRange;
    public float GetAttackRange { get { return attackRange; } }
    public float SetAttackRange { set { attackRange = value; } }
    public float AddAttackRange { set { attackRange += value; } }
    [SerializeField]
    private int minedCopper;
    public int GetMinedCopper { get { return minedCopper; } }
    public int SetMinedCopper { set { minedCopper = value; } }
    public int AddMinedCopper { set { minedCopper += value; } }
    [SerializeField]
    private int minedIron;
    public int GetMinedIron { get { return minedIron; } }
    public int SetMinedIron { set { minedIron = value; } }
    public int AddMinedIron { set { minedIron += value; } }
    [SerializeField]
    private int minedGold;
    public int GetMinedGold { get { return minedGold; } }
    public int SetMinedGold { set { minedGold = value; } }
    public int AddMinedGold { set { minedGold += value; } }


    public void ClearMinedOres()
    {
        minedCopper = 0;
        minedIron = 0;
        minedGold = 0;
    }

    public void AddMinedOre(EnvironmentType _type, int _oreQuantity)
    {
        switch (_type)
        {
            case EnvironmentType.COPPER: minedCopper += _oreQuantity; break;
            case EnvironmentType.IRON: minedIron += _oreQuantity; break;
            case EnvironmentType.GOLD: minedGold += _oreQuantity; break;
            default: Debug.Log("Test mine message"); break;
        }
    }
}
