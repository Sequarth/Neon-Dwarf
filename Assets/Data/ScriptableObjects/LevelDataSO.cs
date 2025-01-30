using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Objects/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    [SerializeField]
    private List<LevelDataBiome> biomes;

    public int ListCount {  get { return biomes.Count; } }

    [System.Serializable]
    private class LevelDataBiome
    {
        //  TODO - change scriptable object to type, get so from unit manager
        [SerializeField]
        private EnvironmentType environmentToSpawn;
        public EnvironmentType EnvironmentToSpawn { get { return environmentToSpawn; } }
        [SerializeField]
        private Vector2 spawnRange;
        public Vector2 SpawnRange { get { return spawnRange; } }
        [SerializeField]
        private int spawnQuantity;
        public int SpawnQuantity { get { return spawnQuantity; } }
    }

    public EnvironmentType GetEnvironmentType(int _biomeId)
    {
        return biomes[_biomeId].EnvironmentToSpawn;
    }

    public int GetSpawnQuantity(int _biomeID)
    {
        return biomes[_biomeID].SpawnQuantity;
    }

    public Vector2 GetSpawnRange(int _biomeID)
    {
        return biomes[_biomeID].SpawnRange;
    }
}
