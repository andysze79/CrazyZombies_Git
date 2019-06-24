using System.Collections.Generic;
using UnityEngine.Events;
using UnityEditor.Events;
using UnityEngine;
using UnityEditor;
using System.IO;
using BaseAssets.Tools;
using BaseAssets.AI;

public class ToolManagerEditor : EditorWindow 
{
    // Editor Variables
    private bool filter = false;
    private bool filterLock = false;
    private bool foldoutPosition = false;
    private string spawnManagerName = null;
    private string formationPlannerName = null;
    private System.Int32 tab = 0;
    private Vector2 scrollPositionGeneralTab = Vector2.zero;
    private Vector2 scrollPositionTroopPrefabCreatorTab = Vector2.zero;
    private Vector2 scrollPositionSpawnManager = Vector2.zero;
    private Vector2 scrollPositionFormationPlanner = Vector2.zero;
    private List<FormationPlanner> editorFormationPlanners = new List<FormationPlanner>();

    // Troop Prefab Creator
    private bool CreateFinalPrefabs = false;
    private int RedLayer = 0;
    private int BlueLayer = 0;
    private string Name = null;
    private GameObject BaseMesh = null;
    private LayerMask RedSearchLayermask = 0;
    private LayerMask BlueSearchLayermask = 0;
    private enum BaseTypeEnum { BaseMelee, BaseRanged, BaseRunner }
    private BaseTypeEnum BaseType = BaseTypeEnum.BaseMelee;
    private AnimatorOverrideController AnimatorController = null;
    private AIDataHolder.TroopType TroopType = AIDataHolder.TroopType.Infantry;

    // Paths
    private string BaseMeleePath = "Assets/BaseAssets/Troops/Tier 2 - Base Type/BaseMelee.prefab";
    private string BaseRangedPath = "Assets/BaseAssets/Troops/Tier 2 - Base Type/BaseRanged.prefab";
    private string BaseRunnerPath = "Assets/BaseAssets/Troops/Tier 2 - Base Type/BaseRunner.prefab";
    private string BaseClassPath = "Assets/BaseAssets/Troops/Tier 5 - Base Class/IgnoreByGit";
    private string BlueTemporaryMaterialPath = "Assets/BaseAssets/ToolMaterials/TPCMaterials/TemporaryBlueTroopMaterial.mat";
    private string RedTemporaryMaterialPath = "Assets/BaseAssets/ToolMaterials/TPCMaterials/TemporaryRedTroopMaterial.mat";
    private string BlueTroopPath = "Assets/BaseAssets/Troops/Tier 6 - Final Prefabs/IgnoreByGit";
    private string RedTroopPath = "Assets/BaseAssets/Troops/Tier 6 - Final Prefabs/IgnoreByGit";

    private string TroopPrefabPath = "Assets/BaseAssets/Troops/Tier 6 - Final Prefabs";

    [MenuItem("Project Tools/Tool Manager %t")]
    private static void ShowWindow() 
    {
        ToolManagerEditor window = GetWindow<ToolManagerEditor>(false, "Tool Manager", true);
    }

    private void OnGUI()
    {
        tab = GUILayout.Toolbar(tab, new string[] { "General", "Troop Prefab Creator", "Troop Prefabs" });
        switch (tab)
        {
            case 0:
                // ------------------------------------------------------------------------------------------------------------------------------------------------------------
                GUILayout.Space(10);
                // ------------------------------------------------------------------------------------------------------------------------------------------------------------

                scrollPositionGeneralTab = GUILayout.BeginScrollView(scrollPositionGeneralTab);

                // ============================================================================================================================================================
                    GUILayout.Label("GLOBAL TOOL SETTINGS", EditorStyles.boldLabel);
                    GUILayout.BeginVertical();
                        EditorGUIUtility.labelWidth = 200f;
                        ToolManagerDataHolder.GizmoInPlayMode = EditorGUILayout.Toggle("Gizmos In Play Mode:", ToolManagerDataHolder.GizmoInPlayMode);
                        ToolManagerDataHolder.GizmoInEditMode = EditorGUILayout.Toggle("Gizmos In Edit Mode:", ToolManagerDataHolder.GizmoInEditMode);
                        ToolManagerDataHolder.ShowInfoLog = EditorGUILayout.Toggle("Show Info Log In Play Mode:", ToolManagerDataHolder.ShowInfoLog);
                        ToolManagerDataHolder.ShowErrorLog = EditorGUILayout.Toggle("Show Error Log In Play Mode:", ToolManagerDataHolder.ShowErrorLog);
                        ToolManagerDataHolder.ProjectileDebugRay = EditorGUILayout.Toggle("Show Projectile Damage Ray:", ToolManagerDataHolder.ProjectileDebugRay);
                    GUILayout.EndVertical();
                // ============================================================================================================================================================

                // ------------------------------------------------------------------------------------------------------------------------------------------------------------
                    GUILayout.Space(30);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                // ------------------------------------------------------------------------------------------------------------------------------------------------------------

                // ============================================================================================================================================================
                    GUILayout.Label("CREATE TOOL", EditorStyles.boldLabel);
                    GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                            EditorGUIUtility.labelWidth = 160f;
                            spawnManagerName = EditorGUILayout.TextField("Name: SpawnManager_", spawnManagerName) as string;
                            if (GUILayout.Button("Create Spawn Manager As Prefab"))
                            {
                                GameObject sm = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/BaseAssets/ToolPrefabs/SpawnManager.prefab");
                                GameObject smRoot = (GameObject)PrefabUtility.InstantiatePrefab(sm);
                                smRoot.name = $"SpawnManager_{spawnManagerName}";

                                SpawnManager smt = smRoot.GetComponent<SpawnManager>();
                                smt.spawnLocations.Add(smt.gameObject);

                                if(Selection.activeGameObject != null)
                                {
                                    smRoot.transform.SetParent(Selection.activeGameObject.transform);
                                    smRoot.transform.position = Vector3.zero;
                                }
                                else
                                    smRoot.transform.position = Vector3.zero;

                                Undo.RegisterCreatedObjectUndo(smRoot, "Created SpawnManager");
                            }
                            if (GUILayout.Button("Create Spawn Manager As GameObject"))
                            {
                                GameObject sm = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/BaseAssets/ToolPrefabs/SpawnManager.prefab");
                                GameObject smRoot = (GameObject)Instantiate(sm);
                                smRoot.name = $"SpawnManager_{spawnManagerName}";

                                SpawnManager smt = smRoot.GetComponent<SpawnManager>();
                                smt.spawnLocations.Add(smt.gameObject);

                                if (Selection.activeGameObject != null)
                                {
                                    smRoot.transform.SetParent(Selection.activeGameObject.transform);
                                    smRoot.transform.position = Vector3.zero;
                                }
                                else
                                    smRoot.transform.position = Vector3.zero;

                                Undo.RegisterCreatedObjectUndo(smRoot, "Created SpawnManager");
                            }
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical();
                            EditorGUIUtility.labelWidth = 160f;
                            formationPlannerName = EditorGUILayout.TextField("Name: FormationPlanner_", formationPlannerName) as string;
                            if (GUILayout.Button("Create Formation Planner As Prefab"))
                            {
                                GameObject fp = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/BaseAssets/ToolPrefabs/FormationPlanner.prefab");
                                GameObject fpRoot = (GameObject)PrefabUtility.InstantiatePrefab(fp);
                                fpRoot.name = $"FormationPlanner_{formationPlannerName}";

                                FormationPlanner fpt = fpRoot.GetComponent<FormationPlanner>();

                                for (int i = 0; i < 4; i++)
                                {
                                    fpt.troopFormation.Add(new Troops());
                                    fpt.troopFormation[fpt.troopFormation.Count - 1].rowCount = 4;
                                }

                                if (Selection.activeGameObject != null)
                                {
                                    fpRoot.transform.SetParent(Selection.activeGameObject.transform);
                                    fpRoot.transform.position = Vector3.zero;
                                }
                                else
                                {
                                    fpRoot.transform.position = Vector3.zero;
                                }

                                Undo.RegisterCreatedObjectUndo(fpRoot, "Created FormationPlanner");
                            }
                            if (GUILayout.Button("Create Formation Planner As GameObject"))
                            {
                                GameObject fp = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/BaseAssets/ToolPrefabs/FormationPlanner.prefab");
                                GameObject fpRoot = (GameObject)Instantiate(fp);
                                fpRoot.name = $"FormationPlanner_{formationPlannerName}";

                                FormationPlanner fpt = fpRoot.GetComponent<FormationPlanner>();

                                for (int i = 0; i < 4; i++)
                                {
                                    fpt.troopFormation.Add(new Troops());
                                    fpt.troopFormation[fpt.troopFormation.Count - 1].rowCount = 4;
                                }

                                if (Selection.activeGameObject != null)
                                {
                                    fpRoot.transform.SetParent(Selection.activeGameObject.transform);
                                    fpRoot.transform.position = Vector3.zero;
                                }
                                else
                                {
                                    fpRoot.transform.position = Vector3.zero;
                                }

                                Undo.RegisterCreatedObjectUndo(fpRoot, "Created FormationPlanner");
                            }
                        GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                // ============================================================================================================================================================

                // ------------------------------------------------------------------------------------------------------------------------------------------------------------
                    GUILayout.Space(30);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                // ------------------------------------------------------------------------------------------------------------------------------------------------------------

                // ============================================================================================================================================================
                    GUILayout.Label("TOOL TRACKER", EditorStyles.boldLabel);

                    EditorGUIUtility.labelWidth = 200f;
                    filter = EditorGUILayout.Toggle("Filter:", filter);
                    filterLock = EditorGUILayout.Toggle("Filter Lock:", filterLock);

                    if (!filterLock)
                        editorFormationPlanners.Clear();
                    else
                        filter = true;

                    SpawnManager[] SpawnManagers = (SpawnManager[])Object.FindObjectsOfType(typeof(SpawnManager));
                    FormationPlanner[] FormationPlanners = (FormationPlanner[])Object.FindObjectsOfType(typeof(FormationPlanner));
                    System.Array.Sort(SpawnManagers, (x, y) => System.String.Compare(x.name, y.name));
                    System.Array.Sort(FormationPlanners, (x, y) => System.String.Compare(x.name, y.name));

                    GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                            scrollPositionSpawnManager = GUILayout.BeginScrollView(scrollPositionSpawnManager);
                                for (int i = 0; i < SpawnManagers.Length; i++)
                                {
                                    if (GUILayout.Button(SpawnManagers[i].name))
                                    {
                                        Selection.activeGameObject = SpawnManagers[i].transform.gameObject;
                                    }
                                }
                            GUILayout.EndScrollView();
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical();
                            scrollPositionFormationPlanner = GUILayout.BeginScrollView(scrollPositionFormationPlanner);
                                for (int i = 0; i < FormationPlanners.Length; i++)
                                {
                                    if (filterLock)
                                    {
                                        break;
                                    }

                                    if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<SpawnManager>() && Selection.activeGameObject.GetComponent<SpawnManager>().formations.Count > 0)
                                    {
                                        if (Selection.activeGameObject.GetComponent<SpawnManager>().formations.Contains(FormationPlanners[i]))
                                        {
                                            editorFormationPlanners.Add(FormationPlanners[i]);
                                        }
                                    }

                                    if(filter)
                                    {
                                        if(Selection.activeGameObject.GetComponent<SpawnManager>() && Selection.activeGameObject.GetComponent<SpawnManager>().formations.Count > 0)
                                        {
                                            if(Selection.activeGameObject.GetComponent<SpawnManager>().formations.Contains(FormationPlanners[i]))
                                            {
                                                if (GUILayout.Button(FormationPlanners[i].name))
                                                {
                                                    Selection.activeGameObject = FormationPlanners[i].transform.gameObject;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if(Selection.activeGameObject.GetComponent<SpawnManager>())
                                            {
                                                GUILayout.Label("This spawner doesn't have any Formation Planners!", EditorStyles.boldLabel);
                                            }
                                            else
                                            {
                                                GUILayout.Label("No Spawner is selected!", EditorStyles.boldLabel);
                                            }
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (GUILayout.Button(FormationPlanners[i].name))
                                        {
                                            Selection.activeGameObject = FormationPlanners[i].transform.gameObject;
                                        }
                                    }
                                }

                                for (int i = 0; i < editorFormationPlanners.Count; i++)
                                {
                                    if(filterLock)
                                    {
                                        if (GUILayout.Button(editorFormationPlanners[i].name))
                                        {
                                            Selection.activeGameObject = editorFormationPlanners[i].transform.gameObject;
                                        }
                                    }
                                }
                            GUILayout.EndScrollView();
                        GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                GUILayout.EndScrollView();
                // ============================================================================================================================================================
                break;

            case 1:
                // ------------------------------------------------------------------------------------------------------------------------------------------------------------
                GUILayout.Space(10);
                // ------------------------------------------------------------------------------------------------------------------------------------------------------------

                scrollPositionTroopPrefabCreatorTab = GUILayout.BeginScrollView(scrollPositionTroopPrefabCreatorTab);

                // ============================================================================================================================================================
                    GUILayout.Label("BASE PREFAB SETTINGS", EditorStyles.boldLabel);
                    EditorGUIUtility.labelWidth = 200f;
                    Name = EditorGUILayout.TextField("Base Name:", Name) as string;
                    BaseMesh = EditorGUILayout.ObjectField("Base Mesh:", BaseMesh, typeof(GameObject), false) as GameObject;
                    AnimatorController = EditorGUILayout.ObjectField("Animator Controller:", AnimatorController, typeof(AnimatorOverrideController), false) as AnimatorOverrideController;
                    BaseType = (BaseTypeEnum)EditorGUILayout.EnumPopup("Base Type:", BaseType);
                    TroopType = (AIDataHolder.TroopType)EditorGUILayout.EnumPopup("Troop Type:", TroopType);
                // ============================================================================================================================================================

                // ------------------------------------------------------------------------------------------------------------------------------------------------------------
                    GUILayout.Space(30);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                // ------------------------------------------------------------------------------------------------------------------------------------------------------------

                // ============================================================================================================================================================
                    GUILayout.Label("TEAM PREFAB SETTINGS", EditorStyles.boldLabel);
                    EditorGUIUtility.labelWidth = 200f;
                    CreateFinalPrefabs = EditorGUILayout.Toggle("Create Team Prefabs:", CreateFinalPrefabs);
                    BlueLayer = EditorGUILayout.LayerField("Blue Team Layer:", BlueLayer);
                    RedLayer = EditorGUILayout.LayerField("Red Team Layer:", RedLayer);
                    LayerMask tempMaskBlue = EditorGUILayout.MaskField("Blue Team Search Layermask:", UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(BlueSearchLayermask), UnityEditorInternal.InternalEditorUtility.layers);
                    BlueSearchLayermask = UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMaskBlue);
                    LayerMask tempMaskRed = EditorGUILayout.MaskField("Red Team Search Layermask:", UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(RedSearchLayermask), UnityEditorInternal.InternalEditorUtility.layers);
                    RedSearchLayermask = UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMaskRed);
                // ============================================================================================================================================================

                // ------------------------------------------------------------------------------------------------------------------------------------------------------------
                    GUILayout.Space(30);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                // ------------------------------------------------------------------------------------------------------------------------------------------------------------

                // ============================================================================================================================================================
                //     foldoutPosition = EditorGUILayout.Foldout(foldoutPosition, "SHOW PATH SETTINGS");
                //         if(foldoutPosition)
                //         {
                //             GUILayout.BeginVertical();
                //                 GUILayout.BeginHorizontal();
                //                     DrawBrowseFileButton("Browse Base Melee Path", ref BaseMeleePath);
                //                 GUILayout.EndHorizontal();
                //                 GUILayout.BeginHorizontal();
                //                     DrawBrowseFileButton("Browse Base Ranged Path", ref BaseRangedPath);
                //                 GUILayout.EndHorizontal();
                //                 GUILayout.BeginHorizontal();
                //                     DrawBrowseFileButton("Browse Blue Temporary Material Path", ref BlueTemporaryMaterialPath);
                //                 GUILayout.EndHorizontal();
                //                 GUILayout.BeginHorizontal();
                //                     DrawBrowseFileButton("Browse Red Temporary Material Path", ref RedTemporaryMaterialPath);
                //                 GUILayout.EndHorizontal();
                //                 GUILayout.BeginHorizontal();
                //                     DrawBrowseFolderButton("Browse Base Class Output", ref BaseClassPath);
                //                 GUILayout.EndHorizontal();
                //                 GUILayout.BeginHorizontal();
                //                     DrawBrowseFolderButton("Browse Blue Troop Output", ref BlueTroopPath);
                //                 GUILayout.EndHorizontal();
                //                 GUILayout.BeginHorizontal();
                //                     DrawBrowseFolderButton("Browse Red Troop Output", ref RedTroopPath);
                //                 GUILayout.EndHorizontal();
                //                 GUILayout.BeginHorizontal();
                //                     if (GUILayout.Button("Save Paths", GUILayout.Width(122)))
                //                     {
                //                         PlayerPrefs.SetString("Base Melee Path", BaseMeleePath);
                //                         PlayerPrefs.SetString("Base Ranged Path", BaseRangedPath);
                //                         PlayerPrefs.SetString("Blue Temporary Material Path", BlueTemporaryMaterialPath);
                //                         PlayerPrefs.SetString("Red Temporary Material Path", RedTemporaryMaterialPath);
                //                         PlayerPrefs.SetString("Base Class Path", BaseClassPath);
                //                         PlayerPrefs.SetString("Blue Troop Path", BlueTroopPath);
                //                         PlayerPrefs.SetString("Red Troop Path", RedTroopPath);
                //                     }

                // // ------------------------------------------------------------------------------------------------------------------------------------------------------------
                //                     GUILayout.Space(2);
                // // ------------------------------------------------------------------------------------------------------------------------------------------------------------

                //                     if (GUILayout.Button("Load Paths", GUILayout.Width(122)))
                //                     {
                //                         if (PlayerPrefs.HasKey("Base Melee Path"))
                //                             BaseMeleePath = PlayerPrefs.GetString("Base Melee Path");
                //                         if (PlayerPrefs.HasKey("Base Ranged Path"))
                //                             BaseRangedPath = PlayerPrefs.GetString("Base Ranged Path");
                //                         if (PlayerPrefs.HasKey("Blue Temporary Material Path"))
                //                             BlueTemporaryMaterialPath = PlayerPrefs.GetString("Blue Temporary Material Path");
                //                         if (PlayerPrefs.HasKey("Red Temporary Material Path"))
                //                             RedTemporaryMaterialPath = PlayerPrefs.GetString("Red Temporary Material Path");
                //                         if (PlayerPrefs.HasKey("Base Class Path"))
                //                             BaseClassPath = PlayerPrefs.GetString("Base Class Path");
                //                         if (PlayerPrefs.HasKey("Blue Troop Path"))
                //                             BlueTroopPath = PlayerPrefs.GetString("Blue Troop Path");
                //                         if (PlayerPrefs.HasKey("Red Troop Path"))
                //                             RedTroopPath = PlayerPrefs.GetString("Red Troop Path");
                //                     }
                //                 GUILayout.EndHorizontal();
                //             GUILayout.EndVertical();
                //         }
                // ============================================================================================================================================================
                GUILayout.EndScrollView();
                break;

                case 2:
                // ============================================================================================================================================================
                if (GUILayout.Button("Browse Troop Prefab Folder", GUILayout.Width(250)))
                {
                    string temp = TroopPrefabPath;
                    TroopPrefabPath = EditorUtility.OpenFolderPanel("Browse Troop Prefab Folder", "", "");
                    if (System.String.IsNullOrEmpty(TroopPrefabPath)) TroopPrefabPath = temp;
                }

                FileInfo[] directoryInfo = new DirectoryInfo(TroopPrefabPath).GetFiles();

                for (int i = 0; i < directoryInfo.Length; i++)
                {
                    if(!directoryInfo[i].Name.Contains("meta"))
                    {
                        if (GUILayout.Button(directoryInfo[i].Name))
                        {
                            Selection.activeGameObject = (GameObject)AssetDatabase.LoadMainAssetAtPath($"{TroopPrefabPath}/{directoryInfo[i].Name}");
                            AssetPreview.GetAssetPreview(Selection.activeGameObject);
                        }
                    }
                }
                // ============================================================================================================================================================
                break;
        }

        DrawResetButton();
    }

    private void DrawResetButton()
    {
        GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.BeginHorizontal();
            
                GUI.backgroundColor = Color.green;
                if (tab == 1 && GUILayout.Button("Create Prefab!", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    CreatePrefabs();
                }

                GUILayout.FlexibleSpace();
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Reset", GUILayout.Width(100), GUILayout.Height(30)))
                    {
                        ToolManagerDataHolder.GizmoInPlayMode = true;

                        spawnManagerName = "";
                        formationPlannerName = "";

                        Name = EditorGUILayout.TextField("Name", "") as string;
                        BaseMesh = EditorGUILayout.ObjectField("BaseMesh", null, typeof(GameObject), false) as GameObject;
                        AnimatorController = EditorGUILayout.ObjectField("AnimatorController", null, typeof(AnimatorOverrideController), false) as AnimatorOverrideController;
                        BaseType = (BaseTypeEnum)EditorGUILayout.EnumPopup("BaseType", BaseTypeEnum.BaseMelee);
                        TroopType = (AIDataHolder.TroopType)EditorGUILayout.EnumPopup("TroopType", AIDataHolder.TroopType.Infantry);
                        BlueLayer = EditorGUILayout.LayerField("BlueLayer", 0);
                        RedLayer = EditorGUILayout.LayerField("RedLayer", 0);
                        LayerMask tempMaskBlue = EditorGUILayout.MaskField("BlueSearchLayermask", UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(BlueSearchLayermask), UnityEditorInternal.InternalEditorUtility.layers);
                        BlueSearchLayermask = UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(0);
                        LayerMask tempMaskRed = EditorGUILayout.MaskField("RedSearchLayermask", UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(RedSearchLayermask), UnityEditorInternal.InternalEditorUtility.layers);
                        RedSearchLayermask = UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(0);
                        CreateFinalPrefabs = EditorGUILayout.Toggle("CreateFinalPrefabs", false);

                        BaseMeleePath = "Assets/BaseAssets/Troops/Tier 2 - Base Type/BaseMelee.prefab";
                        BaseRangedPath = "Assets/BaseAssets/Troops/Tier 2 - Base Type/BaseRanged.prefab";
                        BaseRunnerPath = "Assets/BaseAssets/Troops/Tier 2 - Base Type/BaseRunner.prefab";
                        BaseClassPath = $"Assets/BaseAssets/Troops/Tier 5 - Base Class/IgnoreByGit";
                        BlueTemporaryMaterialPath = "Assets/BaseAssets/ToolMaterials/TPCMaterials/TemporaryBlueTroopMaterial.mat";
                        RedTemporaryMaterialPath = "Assets/BaseAssets/ToolMaterials/TPCMaterials/TemporaryRedTroopMaterial.mat";
                        BlueTroopPath = $"Assets/BaseAssets/Troops/Tier 6 - Final Prefabs/IgnoreByGit";
                        RedTroopPath = $"Assets/BaseAssets/Troops/Tier 6 - Final Prefabs/IgnoreByGit";

                        TroopPrefabPath = "Assets/BaseAssets/Troops/Tier 6 - Final Prefabs";
                    }
            EditorGUILayout.EndHorizontal();
    }

    private void DrawBrowseFileButton(string _text, ref string _path)
    {
        if (GUILayout.Button(_text, GUILayout.Width(250)))
        {
            string temp = _path;
            _path = EditorUtility.OpenFilePanel(_text, "", "prefab");
            if (System.String.IsNullOrEmpty(_path)) _path = temp;
        }
        _path = EditorGUILayout.TextField(_path) as string;
    }

    private void DrawBrowseFolderButton(string _text, ref string _path)
    {
        if (GUILayout.Button(_text, GUILayout.Width(250)))
        {
            string temp = _path;
            _path = EditorUtility.OpenFolderPanel("Browse Red Troop Output", "", "");
            if (System.String.IsNullOrEmpty(_path)) _path = temp;
        }
        _path = EditorGUILayout.TextField(_path) as string;
    }

    // TROOP PREFAB CREATOR ====================================================================================================================================================
    private void CreatePrefabs()
    {
        if (Name == "") return;

        GameObject baseTroopRoot = (GameObject)PrefabUtility.InstantiatePrefab(LoadTroopBase());
        baseTroopRoot.transform.position = Vector3.zero;

        GameObject meshGameObject = Instantiate(BaseMesh, baseTroopRoot.transform);
        meshGameObject.name = BaseMesh.name;
        meshGameObject.transform.localPosition = Vector3.zero;

        AddAnimatorToMesh(meshGameObject);
        AddColliderToMesh(meshGameObject);
        AddEventCatcherToMesh(meshGameObject, baseTroopRoot);

        GameObject baseClassVariant = PrefabUtility.SaveAsPrefabAsset(baseTroopRoot, BaseClassPath + "/Base" + Name + ".prefab");
        GameObject.DestroyImmediate(baseTroopRoot);

        Debug.Log($"<color=white>Base {Name}</color> Prefab Has Been Created! This is the path:  <color=white>{BaseClassPath}/Base{Name}.prefab</color>");

        if (CreateFinalPrefabs)
        {
            SaveTeamVariants("blue", baseClassVariant, BlueSearchLayermask, BlueLayer, BlueTemporaryMaterialPath, BlueTroopPath);
            SaveTeamVariants("red", baseClassVariant, RedSearchLayermask, RedLayer, RedTemporaryMaterialPath, RedTroopPath);
        }
    }

    private GameObject LoadTroopBase()
    {
        GameObject baseTypePrefab = null;

        if (BaseType == BaseTypeEnum.BaseMelee)
            baseTypePrefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(BaseMeleePath);
        else if(BaseType == BaseTypeEnum.BaseRanged)
            baseTypePrefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(BaseRangedPath);
        else if(BaseType == BaseTypeEnum.BaseRunner)
            baseTypePrefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(BaseRunnerPath);

        return baseTypePrefab;
    }

    private void AddAnimatorToMesh(GameObject _meshGameObject)
    {
        Animator animator = null;

        if (_meshGameObject.GetComponent<Animator>() == null)
            animator = _meshGameObject.AddComponent<Animator>();
        else
            animator = _meshGameObject.GetComponent<Animator>();

        animator.runtimeAnimatorController = AnimatorController;
    }

    private void AddColliderToMesh(GameObject _meshGameObject)
    {
        BoxCollider collider = null;

        if (_meshGameObject.GetComponent<BoxCollider>() == null)
            collider = _meshGameObject.AddComponent<BoxCollider>();
        else
            collider = _meshGameObject.GetComponent<BoxCollider>();

        collider.center = new Vector3(0f, 1.5f, 0f);
    }

    private void AddEventCatcherToMesh(GameObject _meshGameObject, GameObject _baseTroopRoot)
    {
        if (_meshGameObject.GetComponent<AIEventCatcher>() == null)
        {
            AIEventCatcher eventCatcher = _meshGameObject.AddComponent<AIEventCatcher>();
            eventCatcher.onDamageEvent.Add(new EventCall());
            eventCatcher.onDamageEvent[0].callName = "Damage";

            if (BaseType == BaseTypeEnum.BaseMelee)
            {
                UnityAction<GameObject> callback = new UnityAction<GameObject>(_baseTroopRoot.GetComponent<State_Attack_Melee>().DealDamage);
                UnityEventTools.AddObjectPersistentListener<GameObject>(eventCatcher.onDamageEvent[0].eventCall, callback, _baseTroopRoot);
            }
            else if(BaseType == BaseTypeEnum.BaseRanged)
            {
                UnityAction<GameObject> callback = new UnityAction<GameObject>(_baseTroopRoot.GetComponent<State_Attack_Ranged>().ShootProjectile);
                UnityEventTools.AddObjectPersistentListener<GameObject>(eventCatcher.onDamageEvent[0].eventCall, callback, _baseTroopRoot);
            }
            else if(BaseType == BaseTypeEnum.BaseRunner)
            {

            }
        }
        else
        {
            AIEventCatcher eventCatcher = _meshGameObject.GetComponent<AIEventCatcher>();
            eventCatcher.onDamageEvent.Add(new EventCall());
            eventCatcher.onDamageEvent[0].callName = "Damage";

            if (BaseType == BaseTypeEnum.BaseMelee)
            {
                UnityAction<GameObject> callback = new UnityAction<GameObject>(_baseTroopRoot.GetComponent<State_Attack_Melee>().DealDamage);
                UnityEventTools.AddObjectPersistentListener<GameObject>(eventCatcher.onDamageEvent[0].eventCall, callback, _baseTroopRoot);
            }
            else if (BaseType == BaseTypeEnum.BaseRanged)
            {
                UnityAction<GameObject> callback = new UnityAction<GameObject>(_baseTroopRoot.GetComponent<State_Attack_Ranged>().ShootProjectile);
                UnityEventTools.AddObjectPersistentListener<GameObject>(eventCatcher.onDamageEvent[0].eventCall, callback, _baseTroopRoot);
            }
            else if (BaseType == BaseTypeEnum.BaseRunner)
            {

            }
        }
    }

    private void SaveTeamVariants(string _debugName, GameObject _baseClassVariant, LayerMask _layerMask, int _layer, string _temporaryMaterialPath, string _troopPath)
    {
        GameObject troopRoot = (GameObject)PrefabUtility.InstantiatePrefab(_baseClassVariant);

        troopRoot.GetComponent<AIDataHolder>().troopType = TroopType;
        troopRoot.GetComponent<AIDataHolder>().searchLayerMask = _layerMask;
        troopRoot.transform.GetChild(0).gameObject.layer = _layer;

        Material troopTemporaryMaterial = (Material)AssetDatabase.LoadMainAssetAtPath(_temporaryMaterialPath);

        foreach (Transform g in troopRoot.transform.GetChild(0).transform.GetComponentsInChildren<Transform>())
        {
            if (g.GetComponent<SkinnedMeshRenderer>())
            {
                Material[] materials = g.GetComponent<SkinnedMeshRenderer>().sharedMaterials;

                for (int i = 0; i < g.GetComponent<SkinnedMeshRenderer>().sharedMaterials.Length; i++)
                {
                    materials[i] = troopTemporaryMaterial;
                }

                g.GetComponent<SkinnedMeshRenderer>().sharedMaterials = materials;
            }
        }

        PrefabUtility.SaveAsPrefabAsset(troopRoot, _troopPath + "/" + _debugName + Name + ".prefab");
        GameObject.DestroyImmediate(troopRoot);
        Debug.Log($"<color={_debugName}>{_debugName} {Name}</color> Prefab Has Been Created! This is the path: <color=white>{_troopPath}/{_debugName}{Name}.prefab</color>");
    }
    // TROOP PREFAB CREATOR ====================================================================================================================================================
}