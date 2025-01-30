using UnityEngine;

public class EnvironmentAttachable : MonoBehaviour
{
    [Header("Stats")]
    public EnvironmentType environmentType;
    public float maxHealth;
    public float currentHealth;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
