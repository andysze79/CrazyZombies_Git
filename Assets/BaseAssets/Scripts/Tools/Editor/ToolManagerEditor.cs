using System.Collections.Generic;
using UnityEngine.Events;
using UnityEditor.Events;
using UnityEngine;
using UnityEditor;

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
    private enum BaseTypeEnum { BaseMelee, BaseRanged }
    private BaseTypeEnum BaseType = BaseTypeEnum.BaseMelee;
    private AnimatorOverrideController AnimatorController = null;
    private AIDataHolder.TroopType TroopType = AIDataHolder.TroopType.Infantry;

    // Paths
    private string BaseMeleePath = "Assets/BaseAssets/Troops/Tier 2 - Base Type/BaseMelee.prefab";
    private string BaseRangedPath = "Assets/BaseAssets/Troops/Tier 2 - Base Type/BaseRanged.prefab";
    private string BaseClassPath = "Assets/BaseAssets/Troops/Tier 5 - Base Class/CreatedByTPC";
    private string BlueTemporaryMaterialPath = "Assets/BaseAssets/ToolMaterials/TPCMaterials/TemporaryBlueTroopMaterial.mat";
    private string RedTemporaryMaterialPath = "Assets/BaseAssets/ToolMaterials/TPCMaterials/TemporaryRedTroopMaterial.mat";
    private string BlueTroopPath = "Assets/BaseAssets/Troops/Tier 6 - Final Prefabs/CreatedByTPC";
    private string RedTroopPath = "Assets/BaseAssets/Troops/Tier 6 - Final Prefabs/CreatedByTPC";

    [MenuItem("Project Tools/Tool Manager %t")]
    private static void ShowWindow() 
    {
        ToolManagerEditor window = GetWindow<ToolManagerEditor>(false, "Tool Manager", true);
    }

    private void OnGUI()
    {
        tab = GUILayout.Toolbar(tab, new string[] { "General", "Troop Prefab Creator" });
        switch (tab)
        {
            case 0:

                GUILayout.Space(10);
                scrollPositionGeneralTab = GUILayout.BeginScrollView(scrollPositionGeneralTab);
                GUILayout.Label("GLOBAL TOOL SETTINGS", EditorStyles.boldLabel);
                GUILayout.BeginVertical();
                EditorGUIUtility.labelWidth = 200f;
                ProjectManagerDataHolder.GizmoInPlayMode = EditorGUILayout.Toggle("Gizmos In Play Mode:", ProjectManagerDataHolder.GizmoInPlayMode);
                ProjectManagerDataHolder.ShowInfoLog = EditorGUILayout.Toggle("Show Info Log In Play Mode:", ProjectManagerDataHolder.ShowInfoLog);
                ProjectManagerDataHolder.ShowErrorLog = EditorGUILayout.Toggle("Show Error Log In Play Mode:", ProjectManagerDataHolder.ShowErrorLog);
                GUILayout.EndVertical();

                GUILayout.Space(30);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

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

                    if(Selection.activeGameObject != null)
                    {
                        smRoot.transform.SetParent(Selection.activeGameObject.transform);
                    }
                    else
                    {
                        smRoot.transform.position = Vector3.zero;
                    }

                    Undo.RegisterCreatedObjectUndo(smRoot, "Created SpawnManager");
                }
                if (GUILayout.Button("Create Spawn Manager As GameObject"))
                {
                    GameObject sm = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/BaseAssets/ToolPrefabs/SpawnManager.prefab");
                    GameObject smRoot = (GameObject)Instantiate(sm);
                    smRoot.name = $"SpawnManager_{spawnManagerName}";

                    if (Selection.activeGameObject != null)
                    {
                        smRoot.transform.SetParent(Selection.activeGameObject.transform);
                    }
                    else
                    {
                        smRoot.transform.position = Vector3.zero;
                    }

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

                    if (Selection.activeGameObject != null)
                    {
                        fpRoot.transform.SetParent(Selection.activeGameObject.transform);
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

                    if (Selection.activeGameObject != null)
                    {
                        fpRoot.transform.SetParent(Selection.activeGameObject.transform);
                    }
                    else
                    {
                        fpRoot.transform.position = Vector3.zero;
                    }

                    Undo.RegisterCreatedObjectUndo(fpRoot, "Created FormationPlanner");
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                SpawnManager[] SpawnManagers = (SpawnManager[])Object.FindObjectsOfType(typeof(SpawnManager));
                FormationPlanner[] FormationPlanners = (FormationPlanner[])Object.FindObjectsOfType(typeof(FormationPlanner));

                System.Array.Sort(SpawnManagers, (x, y) => System.String.Compare(x.name, y.name));
                System.Array.Sort(FormationPlanners, (x, y) => System.String.Compare(x.name, y.name));

                GUILayout.Space(30);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                GUILayout.Label("TOOL TRACKER", EditorStyles.boldLabel);

                EditorGUIUtility.labelWidth = 200f;
                filter = EditorGUILayout.Toggle("Filter:", filter);
                filterLock = EditorGUILayout.Toggle("Filter Lock:", filterLock);

                GUILayout.BeginHorizontal();

                if (!filterLock)
                {
                    editorFormationPlanners.Clear();
                }
                else
                {
                    filter = true;
                }
                GUILayout.EndHorizontal();

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

                break;
            case 1:

                GUILayout.Space(10);
                scrollPositionTroopPrefabCreatorTab = GUILayout.BeginScrollView(scrollPositionTroopPrefabCreatorTab);
                GUILayout.Label("BASE TYPE SETTINGS", EditorStyles.boldLabel);
                Name = EditorGUILayout.TextField("Name:", Name) as string;
                BaseMesh = EditorGUILayout.ObjectField("BaseMesh:", BaseMesh, typeof(GameObject), false) as GameObject;
                AnimatorController = EditorGUILayout.ObjectField("AnimatorController:", AnimatorController, typeof(AnimatorOverrideController), false) as AnimatorOverrideController;
                BaseType = (BaseTypeEnum)EditorGUILayout.EnumPopup("BaseType:", BaseType);
                TroopType = (AIDataHolder.TroopType)EditorGUILayout.EnumPopup("TroopType:", TroopType);

                GUILayout.Space(30);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                GUILayout.Label("FINAL PREFAB SETTINGS", EditorStyles.boldLabel);
                CreateFinalPrefabs = EditorGUILayout.Toggle("CreateFinalPrefabs:", CreateFinalPrefabs);
                BlueLayer = EditorGUILayout.LayerField("BlueLayer:", BlueLayer);
                RedLayer = EditorGUILayout.LayerField("RedLayer:", RedLayer);
                LayerMask tempMaskBlue = EditorGUILayout.MaskField("BlueSearchLayermask:", UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(BlueSearchLayermask), UnityEditorInternal.InternalEditorUtility.layers);
                BlueSearchLayermask = UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMaskBlue);
                LayerMask tempMaskRed = EditorGUILayout.MaskField("RedSearchLayermask:", UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(RedSearchLayermask), UnityEditorInternal.InternalEditorUtility.layers);
                RedSearchLayermask = UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMaskRed);

                GUILayout.Space(30);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                foldoutPosition = EditorGUILayout.Foldout(foldoutPosition, "SHOW PATH SETTINGS");
                if(foldoutPosition)
                {
                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Browse Base Melee Path", GUILayout.Width(250)))
                    {
                        string temp = BaseMeleePath;
                        BaseMeleePath = EditorUtility.OpenFilePanel("Browse Base Melee Path", "", "prefab");
                        if (System.String.IsNullOrEmpty(BaseMeleePath)) BaseMeleePath = temp;
                    }
                    BaseMeleePath = EditorGUILayout.TextField(BaseMeleePath) as string;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Browse Base Ranged Path", GUILayout.Width(250)))
                    {
                        string temp = BaseRangedPath;
                        BaseRangedPath = EditorUtility.OpenFilePanel("Browse Base Ranged Path", "", "prefab");
                        if (System.String.IsNullOrEmpty(BaseRangedPath)) BaseRangedPath = temp;
                    }
                    BaseRangedPath = EditorGUILayout.TextField(BaseRangedPath) as string;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Browse Blue Temporary Material Path", GUILayout.Width(250)))
                    {
                        string temp = BlueTemporaryMaterialPath;
                        BlueTemporaryMaterialPath = EditorUtility.OpenFilePanel("Browse Blue Temporary Material Path", "", "mat");
                        if (System.String.IsNullOrEmpty(BlueTemporaryMaterialPath)) BlueTemporaryMaterialPath = temp;
                    }
                    BlueTemporaryMaterialPath = EditorGUILayout.TextField(BlueTemporaryMaterialPath) as string;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Browse Red Temporary Material Path", GUILayout.Width(250)))
                    {
                        string temp = RedTemporaryMaterialPath;
                        RedTemporaryMaterialPath = EditorUtility.OpenFilePanel("Browse Red Temporary Material Path", "", "mat");
                        if (System.String.IsNullOrEmpty(RedTemporaryMaterialPath)) RedTemporaryMaterialPath = temp;
                    }
                    RedTemporaryMaterialPath = EditorGUILayout.TextField(RedTemporaryMaterialPath) as string;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Browse Base Class Output", GUILayout.Width(250)))
                    {
                        string temp = BaseClassPath;
                        BaseClassPath = EditorUtility.OpenFolderPanel("Browse Base Class Output", "", "");
                        if (System.String.IsNullOrEmpty(BaseClassPath)) BaseClassPath = temp;
                    }
                    BaseClassPath = EditorGUILayout.TextField(BaseClassPath) as string;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Browse Blue Troop Output", GUILayout.Width(250)))
                    {
                        string temp = BlueTroopPath;
                        BlueTroopPath = EditorUtility.OpenFolderPanel("Browse Blue Troop Output", "", "");
                        if (System.String.IsNullOrEmpty(BlueTroopPath)) BlueTroopPath = temp;
                    }
                    BlueTroopPath = EditorGUILayout.TextField(BlueTroopPath) as string;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Browse Red Troop Output", GUILayout.Width(250)))
                    {
                        string temp = RedTroopPath;
                        RedTroopPath = EditorUtility.OpenFolderPanel("Browse Red Troop Output", "", "");
                        if (System.String.IsNullOrEmpty(RedTroopPath)) RedTroopPath = temp;
                    }
                    RedTroopPath = EditorGUILayout.TextField(RedTroopPath) as string;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Save Paths", GUILayout.Width(122)))
                    {
                        PlayerPrefs.SetString("Base Melee Path", BaseMeleePath);
                        PlayerPrefs.SetString("Base Ranged Path", BaseRangedPath);
                        PlayerPrefs.SetString("Blue Temporary Material Path", BlueTemporaryMaterialPath);
                        PlayerPrefs.SetString("Red Temporary Material Path", RedTemporaryMaterialPath);
                        PlayerPrefs.SetString("Base Class Path", BaseClassPath);
                        PlayerPrefs.SetString("Blue Troop Path", BlueTroopPath);
                        PlayerPrefs.SetString("Red Troop Path", RedTroopPath);
                    }
                    GUILayout.Space(2);
                    if (GUILayout.Button("Load Paths", GUILayout.Width(122)))
                    {
                        if (PlayerPrefs.HasKey("Base Melee Path"))
                            BaseMeleePath = PlayerPrefs.GetString("Base Melee Path");
                        if (PlayerPrefs.HasKey("Base Ranged Path"))
                            BaseRangedPath = PlayerPrefs.GetString("Base Ranged Path");
                        if (PlayerPrefs.HasKey("Blue Temporary Material Path"))
                            BlueTemporaryMaterialPath = PlayerPrefs.GetString("Blue Temporary Material Path");
                        if (PlayerPrefs.HasKey("Red Temporary Material Path"))
                            RedTemporaryMaterialPath = PlayerPrefs.GetString("Red Temporary Material Path");
                        if (PlayerPrefs.HasKey("Base Class Path"))
                            BaseClassPath = PlayerPrefs.GetString("Base Class Path");
                        if (PlayerPrefs.HasKey("Blue Troop Path"))
                            BlueTroopPath = PlayerPrefs.GetString("Blue Troop Path");
                        if (PlayerPrefs.HasKey("Red Troop Path"))
                            RedTroopPath = PlayerPrefs.GetString("Red Troop Path");
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }

                GUILayout.EndScrollView();

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
            ProjectManagerDataHolder.GizmoInPlayMode = true;

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
            BaseClassPath = $"Assets/BaseAssets/Troops/Tier 5 - Base Class/CreatedByTPC";
            BlueTemporaryMaterialPath = "Assets/BaseAssets/ToolMaterials/TPCMaterials/TemporaryBlueTroopMaterial.mat";
            RedTemporaryMaterialPath = "Assets/BaseAssets/ToolMaterials/TPCMaterials/TemporaryRedTroopMaterial.mat";
            BlueTroopPath = $"Assets/BaseAssets/Troops/Tier 6 - Final Prefabs/CreatedByTPC";
            RedTroopPath = $"Assets/BaseAssets/Troops/Tier 6 - Final Prefabs/CreatedByTPC";
        }
        EditorGUILayout.EndHorizontal();
    }

    private void CreatePrefabs()
    {
        if (Name == "")
        {
            return;
        }

        GameObject baseTypePrefab = null;

        if (BaseType == BaseTypeEnum.BaseMelee)
        {
            baseTypePrefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(BaseMeleePath);
        }
        else
        {
            baseTypePrefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(BaseRangedPath);
        }

        GameObject baseTroopRoot = (GameObject)PrefabUtility.InstantiatePrefab(baseTypePrefab);
        baseTroopRoot.transform.position = Vector3.zero;

        GameObject mesh = Instantiate(BaseMesh);
        mesh.name = BaseMesh.name;
        mesh.transform.SetParent(baseTroopRoot.transform);
        mesh.transform.localPosition = Vector3.zero;

        if (mesh.GetComponent<Animator>() == null)
        {
            Animator animator = mesh.AddComponent<Animator>();
            animator.runtimeAnimatorController = AnimatorController;
        }
        else
        {
            Animator animator = mesh.GetComponent<Animator>();
            animator.runtimeAnimatorController = AnimatorController;
        }

        if (mesh.GetComponent<BoxCollider>() == null)
        {
            BoxCollider collider = mesh.AddComponent<BoxCollider>();
            collider.center = new Vector3(0f, 1.5f, 0f);
        }
        else
        {
            BoxCollider collider = mesh.GetComponent<BoxCollider>();
            collider.center = new Vector3(0f, 1.5f, 0f);
        }

        if (mesh.GetComponent<AIEventCatcher>() == null)
        {
            AIEventCatcher eventCatcher = mesh.AddComponent<AIEventCatcher>();
            eventCatcher.onDamageEvent.Add(new EventCall());
            eventCatcher.onDamageEvent[0].callName = "Damage";

            if (BaseType == BaseTypeEnum.BaseMelee)
            {
                UnityAction<GameObject> callback = new UnityAction<GameObject>(baseTroopRoot.GetComponent<State_Attack_Melee>().DealDamage);
                UnityEventTools.AddObjectPersistentListener<GameObject>(eventCatcher.onDamageEvent[0].eventCall, callback, baseTroopRoot);
            }
            else
            {
                UnityAction<GameObject> callback = new UnityAction<GameObject>(baseTroopRoot.GetComponent<State_Attack_Ranged>().ShootProjectile);
                UnityEventTools.AddObjectPersistentListener<GameObject>(eventCatcher.onDamageEvent[0].eventCall, callback, baseTroopRoot);
            }
        }
        else
        {
            AIEventCatcher eventCatcher = mesh.GetComponent<AIEventCatcher>();
            eventCatcher.onDamageEvent.Add(new EventCall());
            eventCatcher.onDamageEvent[0].callName = "Damage";

            if (BaseType == BaseTypeEnum.BaseMelee)
            {
                UnityAction<GameObject> callback = new UnityAction<GameObject>(baseTroopRoot.GetComponent<State_Attack_Melee>().DealDamage);
                UnityEventTools.AddObjectPersistentListener<GameObject>(eventCatcher.onDamageEvent[0].eventCall, callback, baseTroopRoot);
            }
            else
            {
                UnityAction<GameObject> callback = new UnityAction<GameObject>(baseTroopRoot.GetComponent<State_Attack_Ranged>().ShootProjectile);
                UnityEventTools.AddObjectPersistentListener<GameObject>(eventCatcher.onDamageEvent[0].eventCall, callback, baseTroopRoot);
            }
        }

        GameObject baseClassVariant = PrefabUtility.SaveAsPrefabAsset(baseTroopRoot, BaseClassPath + "/Base" + Name + ".prefab");
        GameObject.DestroyImmediate(baseTroopRoot);

        Debug.Log($"<color=white>Base {Name}</color> Prefab Has Been Created! This is the path:  <color=white>{BaseClassPath}/Base{Name}.prefab</color>");

        if (CreateFinalPrefabs)
        {
            GameObject blueTroopRoot = (GameObject)PrefabUtility.InstantiatePrefab(baseClassVariant);
            GameObject redTroopRoot = (GameObject)PrefabUtility.InstantiatePrefab(baseClassVariant);

            blueTroopRoot.GetComponent<AIDataHolder>().troopType = TroopType;
            redTroopRoot.GetComponent<AIDataHolder>().troopType = TroopType;

            blueTroopRoot.GetComponent<AIDataHolder>().searchLayerMask = BlueSearchLayermask;
            redTroopRoot.GetComponent<AIDataHolder>().searchLayerMask = RedSearchLayermask;

            blueTroopRoot.transform.GetChild(0).gameObject.layer = BlueLayer;
            redTroopRoot.transform.GetChild(0).gameObject.layer = RedLayer;

            Material blueTroopTemporaryMaterial = (Material)AssetDatabase.LoadMainAssetAtPath(BlueTemporaryMaterialPath);
            Material redTroopTemporaryMaterial = (Material)AssetDatabase.LoadMainAssetAtPath(RedTemporaryMaterialPath);

            foreach (Transform g in blueTroopRoot.transform.GetChild(0).transform.GetComponentsInChildren<Transform>())
            {
                if (g.GetComponent<SkinnedMeshRenderer>())
                {
                    Material[] materials = g.GetComponent<SkinnedMeshRenderer>().sharedMaterials;

                    for (int i = 0; i < g.GetComponent<SkinnedMeshRenderer>().sharedMaterials.Length; i++)
                    {
                        materials[i] = blueTroopTemporaryMaterial;
                    }

                    g.GetComponent<SkinnedMeshRenderer>().sharedMaterials = materials;
                }
            }

            foreach (Transform g in redTroopRoot.transform.GetChild(0).transform.GetComponentsInChildren<Transform>())
            {
                if (g.GetComponent<SkinnedMeshRenderer>())
                {
                    Material[] materials = g.GetComponent<SkinnedMeshRenderer>().sharedMaterials;

                    for (int i = 0; i < g.GetComponent<SkinnedMeshRenderer>().sharedMaterials.Length; i++)
                    {
                        materials[i] = redTroopTemporaryMaterial;
                    }

                    g.GetComponent<SkinnedMeshRenderer>().sharedMaterials = materials;
                }
            }

            PrefabUtility.SaveAsPrefabAsset(blueTroopRoot, BlueTroopPath + "/Blue" + Name + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(redTroopRoot, RedTroopPath + "/Red" + Name + ".prefab");

            GameObject.DestroyImmediate(blueTroopRoot);
            GameObject.DestroyImmediate(redTroopRoot);

            Debug.Log($"<color=blue>Blue {Name}</color> Prefab Has Been Created! This is the path: <color=white>{BlueTroopPath}/Blue{Name}.prefab</color>");
            Debug.Log($"<color=red>Red {Name}</color> Prefab Has Been Created! This is the path: <color=white>{RedTroopPath}/Red{Name}.prefab</color>");
        }
    }
}