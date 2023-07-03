#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

public class RopeEditor : EditorWindow
{
    private Transform ropeStart; 
    private ObiRopeBlueprint blueprint;
    private GameObject ropeSolverPrefab;
    private Rigidbody rigidbody; 
    
[MenuItem("Tools/Rope Editor")]
    public static void ShowWindow()
    {
        RopeEditor window = (RopeEditor)EditorWindow.GetWindow(typeof(RopeEditor));
        window.titleContent = new GUIContent("RopeEditor");
        window.TryFindPrefab();
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        
        if (!ropeSolverPrefab)
        {
            StagePrefab();
        }
        else if (!blueprint)
        {
            StageBlueprint();
        }
        else if (!ropeStart)
        {
            StageStart();
        }
        else if (!rigidbody)
        {
            StageEnd();
        }
        else
        {
            StageReady();
        }
    }

    private void StagePrefab()
    {
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Find RopeTemplate in Prefabs folder.", MessageType.Info);
        ropeSolverPrefab = (GameObject)EditorGUILayout.ObjectField("Rope prefab: ", ropeSolverPrefab, typeof(GameObject));
        
        blueprint = null;
        ropeStart = null;
        rigidbody = null;
    } 
    
    private void StageBlueprint()
    {
        ropeSolverPrefab = (GameObject)EditorGUILayout.ObjectField("Rope prefab: ", ropeSolverPrefab, typeof(GameObject));
        
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Create ObiRope Blueprint in Assets/Content/RopeBlueprints/Main", MessageType.Info);
        blueprint = (ObiRopeBlueprint)EditorGUILayout.ObjectField("Rope blueprint: ", blueprint, typeof(ObiRopeBlueprint));
        
        ropeStart = null;
        rigidbody = null;
    }    

    private void StageStart()
    {
        ropeSolverPrefab = (GameObject)EditorGUILayout.ObjectField("Rope prefab: ", ropeSolverPrefab, typeof(GameObject));
        blueprint = (ObiRopeBlueprint)EditorGUILayout.ObjectField("Rope blueprint: ", blueprint, typeof(ObiRopeBlueprint));
        
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Create EmptyObject on scene. This object will be static (top) end of the rope.", MessageType.Info);
        ropeStart = (Transform)EditorGUILayout.ObjectField("Rope start point: ", ropeStart, typeof(Transform) );
        
       rigidbody = null;
    }
    
    private void StageEnd()
    {
        ropeSolverPrefab = (GameObject)EditorGUILayout.ObjectField("Rope prefab: ", ropeSolverPrefab, typeof(GameObject));
        blueprint = (ObiRopeBlueprint)EditorGUILayout.ObjectField("Rope blueprint: ", blueprint, typeof(ObiRopeBlueprint));
        ropeStart = (Transform)EditorGUILayout.ObjectField("Rope start point: ", ropeStart, typeof(Transform) );
        
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Select RigidBody that will hang on the rope.", MessageType.Info);
        rigidbody = (Rigidbody)EditorGUILayout.ObjectField("Rigidbody: ", rigidbody, typeof(Rigidbody));
    }
    
    private void StageReady()
    {
        ropeSolverPrefab = (GameObject)EditorGUILayout.ObjectField("Rope prefab: ", ropeSolverPrefab, typeof(GameObject));
        blueprint = (ObiRopeBlueprint)EditorGUILayout.ObjectField("Rope blueprint: ", blueprint, typeof(ObiRopeBlueprint));
        ropeStart = (Transform)EditorGUILayout.ObjectField("Rope start point: ", ropeStart, typeof(Transform) );
        rigidbody = (Rigidbody)EditorGUILayout.ObjectField("Rigidbody: ", rigidbody, typeof(Rigidbody));
        
        if (GUILayout.Button("Create Rope"))
        {
            CreateNewRope();
            
            blueprint = null;
            ropeStart = null;
            rigidbody = null;
        }
    }

    private void TryFindPrefab()
    {
        GameObject prefab = null;
        prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/RopeTemplate.prefab", typeof(GameObject));

        if (prefab != null)
            ropeSolverPrefab = prefab;
    }

    private void Test()
    {
        ropeSolverPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/RopeTemplate.prefab", typeof(GameObject));

    }

    private void CreateNewRope()
    {
        GameObject newGO = (GameObject)PrefabUtility.InstantiatePrefab(ropeSolverPrefab);
        newGO.transform.position = Vector3.zero;
            
        StageUtility.PlaceGameObjectInCurrentStage(newGO);
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null)
        {
            EditorSceneManager.MarkSceneDirty(prefabStage.scene);
        }

        ObiRope obiRope = newGO.GetComponentInChildren<ObiRope>();
        // ObiRopeExtrudedRenderer renderer = newGO.GetComponentInChildren<ObiRopeExtrudedRenderer>();
        ObiParticleAttachment[] attachments = newGO.GetComponentsInChildren<ObiParticleAttachment>();
        
        if (blueprint.path.points.Count < 2)
        {
            int filter = ObiUtils.MakeFilter(ObiUtils.CollideWithEverything,0);
            blueprint.path.AddControlPoint(Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero,
                0.1f, 0.1f, 0.5f, filter, Color.white, "start");
            blueprint.path.AddControlPoint(Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero,
                0.1f, 0.1f, 0.5f, filter, Color.white, "end");
        }


        foreach (var attachment in attachments)
        {
            if (attachment.attachmentType == ObiParticleAttachment.AttachmentType.Dynamic)
            {
                attachment.target = rigidbody.transform;
                attachment.particleGroup = blueprint.groups[1];
            }
            
            if (attachment.attachmentType == ObiParticleAttachment.AttachmentType.Static)
            {
                attachment.particleGroup = blueprint.groups[0];
            }
        }

        ObiCollider obiCollider = rigidbody.gameObject.GetComponent<ObiCollider>();
        if (obiCollider == null)
        {
            obiCollider = rigidbody.gameObject.AddComponent<ObiCollider>();
        }
        
        obiCollider.Filter = ObiUtils.MakeFilter(ObiUtils.CollideWithNothing, 0); 

        obiRope.ropeBlueprint = blueprint;

        Vector3 rBodyPos = rigidbody.position;

        CapsuleCollider capsuleCollider = rigidbody.gameObject.GetComponent<CapsuleCollider>();
        BoxCollider boxCollider = rigidbody.gameObject.GetComponent<BoxCollider>();
        SphereCollider sphereCollider = rigidbody.gameObject.GetComponent<SphereCollider>();

        if (capsuleCollider != null)
        {
            rBodyPos = rigidbody.transform.TransformPoint(capsuleCollider.center);
        }
        else if (boxCollider != null)
        {
            rBodyPos = rigidbody.transform.TransformPoint(boxCollider.center);
        }
        else if (sphereCollider != null)
        {
            rBodyPos = rigidbody.transform.TransformPoint(sphereCollider.center);
        }

        SetBlueprintPoint(obiRope.ropeBlueprint, ropeStart.position, 0);
        SetBlueprintPoint(obiRope.ropeBlueprint, rBodyPos, 1);
        
        // obiRope.path.FlushEvents();
        obiRope.path.OnPathChanged.Invoke();
    }

    private void SetBlueprintPoint(ObiRopeBlueprint blueprint, Vector3 pos, int pointIdx)
    {

        ObiWingedPoint point = blueprint.path.m_Points[pointIdx];
        point.position = pos;
        blueprint.path.m_Points[pointIdx] = point;
    }
}
#endif