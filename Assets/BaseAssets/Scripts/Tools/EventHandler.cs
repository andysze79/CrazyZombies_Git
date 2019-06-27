using BaseAssets.Tools;
using BaseAssets.AI;

namespace BaseAssets.Events
{
    public static class EventHandler
    {
        public static OnAllTroopsSpawned onAllTroopsSpawned;
        public delegate void OnAllTroopsSpawned(SpawnManager _spawnManager);

        public static OnTroopSpawn onTroopSpawn;    
        public delegate void OnTroopSpawn(AIDataHolder _data);

        public static OnTroopDie onTroopDie;
        public delegate void OnTroopDie(AIDataHolder _data);

        public static OnFormationSwitch onFormationSwitch;
        public delegate void OnFormationSwitch(SpawnManager _spawnManager, FormationPlanner _formationPlanner);
    }    
}
