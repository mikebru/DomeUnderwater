using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NetworkSkeletalBuilder : EditorWindow
{
    [MenuItem("GameObject/Network/Skeletal Builder")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<NetworkSkeletalBuilder>();
    }

    GameObject selection;
    NetworkTransform netTransform;
    bool multiSelectFail = false;

    [SerializeField] Transform skeletalRoot;

    void OnGUI()
    {
        if (selection != null)
        {
            if (multiSelectFail)
            {
                EditorGUILayout.LabelField("Not compatible with multi-selection!");
            }
            else
            {
                if (netTransform == null)
                {
                    EditorGUILayout.LabelField(selection.name);
                    EditorGUILayout.LabelField("Object must have a NetworkTransform.");
                    if (GUILayout.Button("Try finding one further up the tree."))
                    {
                        NetworkTransform transformCandidate = selection.GetComponentInParent<NetworkTransform>();
                    }

                    if (GUILayout.Button("Add one to this object."))
                    {
                        netTransform = selection.AddComponent<NetworkTransform>();
                    }
                }
                else
                {
                    skeletalRoot = (Transform)EditorGUILayout.ObjectField("skeleton root", skeletalRoot, typeof(Transform), true);

                    if (skeletalRoot == null)
                    {
                        EditorGUILayout.LabelField("Please assign a skeleton root object before trying to assign network transform children");
                    }
                    else
                    {
                        if (GUILayout.Button("Build tree"))
                        {
                            BuildTree(skeletalRoot);
                        }
                    }

                    if (GUILayout.Button("DELETE ALL CHILDREN"))
                    {
                        NetworkTransformChild[] children = selection.GetComponents<NetworkTransformChild>();

                        for (int i = 0; i < children.Length; i++) DestroyImmediate(children[i]);
                    }
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("To use this script, you must have a single GameObject selected with a NetworkTransform component.");
            if (GUILayout.Button("Refresh"))
            {
                GetSelectionComponents();
            }
        }
    }

    void BuildTree(Transform skeletalRoot)
    {
        List<NetworkTransformChild> comprehensiveList = new List<NetworkTransformChild>();
        comprehensiveList.AddRange(selection.GetComponentsInChildren<NetworkTransformChild>());

        EnsurePathToParent(skeletalRoot, netTransform.transform, ref comprehensiveList);
        ApplyNetTransChild(skeletalRoot, netTransform.transform, ref comprehensiveList);
    }

    void EnsurePathToParent(Transform child, Transform root, ref List<NetworkTransformChild> childList)
    {
        if (child.GetInstanceID() == root.GetInstanceID()) return;

        if (!ChildListContains(child, childList))
        {
            NetworkTransformChild newChild = root.gameObject.AddComponent<NetworkTransformChild>();
            newChild.target = child;
            childList.Add(newChild);
        }

        EnsurePathToParent(child.parent, root, ref childList);
    }

    private static bool ChildListContains(Transform child, List<NetworkTransformChild> childList)
    {
        bool childListContainsTarget = false;

        for (int i = 0; i < childList.Count; i++)
        {
            if (childList[i].target == null)
            {
                Debug.Log("Network Transform Child had no target!");
                Debug.Break();
            }
            else
            {
                if (childList[i].target.GetInstanceID() == child.GetInstanceID())
                {
                    childListContainsTarget = true;
                    break;
                }
            }
        }

        return childListContainsTarget;
    }

    void ApplyNetTransChild(Transform child, Transform root, ref List<NetworkTransformChild> childList)
    {
        if (!ChildListContains(child, childList))
        {
            NetworkTransformChild newChild = root.gameObject.AddComponent<NetworkTransformChild>();
            newChild.target = child;
            childList.Add(newChild);
        }

        if (child.childCount > 0)
        {
            for (int i = 0; i < child.childCount; i++)
            {
                ApplyNetTransChild(child.GetChild(i), root, ref childList);
            }
        }
    }

    void GetSelectionComponents()
    {
        selection = null;
        multiSelectFail = false;

        if (Selection.gameObjects.Length == 0) return;
        if (Selection.gameObjects.Length > 1)
        {
            multiSelectFail = true;
            return;
        }

        selection = Selection.gameObjects[0];

        netTransform = selection.GetComponent<NetworkTransform>();
    }

    void OnSelectionChange()
    {
        GetSelectionComponents();
    }
}
