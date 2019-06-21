using UnityEngine;
using BaseAssets.Tools;

namespace BaseAssets.Debugger
{
    public static class DebugManager
    {
        // FORMATION PLANNER ===========================================================================================================================
        public static void TroopFormationEmpty(GameObject _context)
        {
            if (!Application.isPlaying || ToolManagerDataHolder.ShowErrorLog)
                UnityEngine.Debug.Log("<color=red>Troop Count is 0!</color> Please add elements to Troop Formation list on the Inspector!", _context);
        }

        public static void ToolConfiguration(FormationPlanner.FormationType _formationType, int _troopCount, GameObject _context)
        {
            UnityEngine.Debug.Log($"<color=yellow>{_context.name}</color> currently using - <color=white>{_formationType}</color> - formation type and - <color=white>{_troopCount}</color> - unit(s) will spawn!", _context);
        }
        // FORMATION PLANNER ===========================================================================================================================

        // SPAWN MANAGER ===============================================================================================================================
        public static void SpawnContainerEmpty(GameObject _context)
        {
            if (!Application.isPlaying || ToolManagerDataHolder.ShowErrorLog)
                UnityEngine.Debug.Log($"<color=yellow>{_context.name}</color>'s <color=white>TroopContainer</color> variable was <color=red>empty</color>, a new one was created!", _context);
        }

        public static void WaypointContainerEmpty(GameObject _context)
        {
            if (!Application.isPlaying || ToolManagerDataHolder.ShowInfoLog)
                UnityEngine.Debug.Log($"<color=yellow>{_context.name}</color>'s <color=white>EndlessWaypointContainer</color> variable was <color=red>empty</color>, a new one was created!", _context);
        }
        public static void SpawnLocationsEmpty(GameObject _context)
        {
            if (!Application.isPlaying || ToolManagerDataHolder.ShowInfoLog)
                UnityEngine.Debug.Log("<color=red>Spawn Location is empty!</color> Please add Spawn Locations to SpawnLocations list on the Inspector!", _context);
        }

        public static void NoFormationAtAll(GameObject _context)
        {
            if (!Application.isPlaying || ToolManagerDataHolder.ShowErrorLog)
                UnityEngine.Debug.Log("<color=red>Formation list is empty!</color> Please add at least one formation to the list if you are <color=red>NOT</color> using endless!", _context);
        }

        public static void MissingFormation(GameObject _context)
        {
            if (!Application.isPlaying || ToolManagerDataHolder.ShowErrorLog)
                UnityEngine.Debug.Log("<color=red>You are missing an element!</color> Please don't leave the list elements empty! ", _context);
        }

        public static void NoFormationToSwitchTo(GameObject _context)
        {
            if (!Application.isPlaying || ToolManagerDataHolder.ShowInfoLog)
                UnityEngine.Debug.Log("<color=red>No formation to switch to!</color> Add additional formations to the list!", _context);
        }

        public static void NoTroopPrefabSet(GameObject _context)
        {
            if (!Application.isPlaying || ToolManagerDataHolder.ShowErrorLog)
                UnityEngine.Debug.Log("<color=red>Troop prefab is not set!</color> Please choose what troop to spawn!", _context);
        }

        public static void NoTroopParentSet(GameObject _context)
        {
            if (!Application.isPlaying || ToolManagerDataHolder.ShowInfoLog)
                UnityEngine.Debug.Log("<color=red>Parent transform is not set!</color> Please choose under what parent to spawn troop(s)!", _context);
        }

        public static void EndlessSpawnError(GameObject _context)
        {
            if (!Application.isPlaying || ToolManagerDataHolder.ShowInfoLog)
                UnityEngine.Debug.Log("Please make sure <color=white>SpawnType</color> is  <color=white>SpawnEndless</color> AND <color=white>SpawnLocationType</color> is  <color=white>RandomArea</color>", _context);
        }
        // SPAWN MANAGER ===============================================================================================================================
    }
}