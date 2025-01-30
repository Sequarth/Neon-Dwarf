using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentStatsSO", menuName = "Scriptable Objects/EnvironmentStatsSO")]
public class EnvironmentStatsSO : ScriptableObject
{
    [SerializeField]
    private string environmentName;
    public string EnvironmentName { get { return environmentName; } }
    [SerializeField]
    private EnvironmentType environmentType;
    public EnvironmentType EnvironmentType { get { return environmentType; } }
    [SerializeField]
    private Color environmentColor;
    public Color EnvironmentColor { get { return environmentColor; } }
    [SerializeField]
    private Sprite environmentSprite;
    public Sprite EnvironmentSprite { get { return environmentSprite; } }
    [SerializeField]
    private float maxHealth;
    public float MaxHealth { get { return maxHealth; } }
    [SerializeField]
    private float miningPowerNeeded;
    public float MiningPowerNeeded { get { return miningPowerNeeded; } }
}
