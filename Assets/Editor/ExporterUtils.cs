using GorillaShirts.Behaviours.Cosmetic;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GorillaShirtsUnityProject
{
    public class ExporterUtils
    {
        public static void ExportPackage(ShirtDescriptor originalDescriptor, string path)
        {
            GameObject gameObject = Object.Instantiate(originalDescriptor.gameObject);
            gameObject.TryGetComponent(out ShirtDescriptor descriptor);

            List<FieldInfo> objectMembers = new();
            foreach(var field in descriptor.GetType().GetFields().Where(field => field.DeclaringType == descriptor.GetType()))
            {
                if (field.FieldType == typeof(GameObject))
                {
                    GameObject value = (GameObject)field.GetValue(descriptor);
                    if (value == null || !value) continue;
                    objectMembers.Add(field);
                }
            }

            List<GameObject> objectsToRemove = new();
            foreach(Transform child in gameObject.transform)
            {
                if (objectMembers.Any(field => ((GameObject)field.GetValue(descriptor)) == child.gameObject)) continue;
                objectsToRemove.Add(child.gameObject);
            }

            List<string> recognizedTags = new() { "Body", "Head", "LeftUpperArm", "LeftLowerArm", "LeftHand", "RightUpperArm", "RightLowerArm", "RightHand", "NameTagAnchor", "BadgeAnchor", "SlingshotAnchor" };
            Dictionary<string, Transform> bodyPartsPerTag = new();
            foreach (Transform descendant in gameObject.GetComponentsInChildren<Transform>(false))
            {
                if (objectMembers.Any(field => ((GameObject)field.GetValue(descriptor)) == descendant.gameObject)) continue;

                if (recognizedTags.Contains(descendant.tag) && !bodyPartsPerTag.ContainsKey(descendant.tag) && descendant.gameObject.activeInHierarchy)
                    bodyPartsPerTag.Add(descendant.tag, descendant);

                if (descendant.CompareTag("Substitute"))
                {
                    objectsToRemove.Add(descendant.gameObject);
                }
            }

            if (descriptor.Body is GameObject body && body)
            {
                if (bodyPartsPerTag.TryGetValue("Body", out Transform bone)) body.transform.SetParent(bone, true);
                body.transform.SetParent(gameObject.transform, false);
                body.name = "Body";
            }

            if (bodyPartsPerTag.TryGetValue("BadgeAnchor", out Transform badgeAnchor))
            {
                if (bodyPartsPerTag.TryGetValue("Body", out Transform bone)) badgeAnchor.transform.SetParent(bone, true);
                badgeAnchor.transform.SetParent(gameObject.transform, false);
                badgeAnchor.name = "BadgeAnchor";
            }

            if (bodyPartsPerTag.TryGetValue("SlingshotAnchor", out Transform chestAnchor))
            {
                if (bodyPartsPerTag.TryGetValue("Body", out Transform bone)) chestAnchor.transform.SetParent(bone, true);
                chestAnchor.transform.SetParent(gameObject.transform, false);
                chestAnchor.name = "SlingshotAnchor";
            }

            if (bodyPartsPerTag.TryGetValue("NameTagAnchor", out Transform nameAnchor))
            {
                if (bodyPartsPerTag.TryGetValue("Body", out Transform bone)) nameAnchor.transform.SetParent(bone, true);
                nameAnchor.transform.SetParent(gameObject.transform, false);
                nameAnchor.name = "NameTagAnchor";
            }

            if (descriptor.Head is GameObject head && head)
            {
                if (bodyPartsPerTag.TryGetValue("Head", out Transform bone)) head.transform.SetParent(bone, true);
                head.transform.SetParent(gameObject.transform, false);
                head.name = "Head";
            }

            if (descriptor.LeftUpperArm is GameObject leftUpper && leftUpper)
            {
                if (bodyPartsPerTag.TryGetValue("LeftUpperArm", out Transform bone)) leftUpper.transform.SetParent(bone, true);
                leftUpper.transform.SetParent(gameObject.transform, false);
                leftUpper.name = "LeftUpper";
            }

            if (descriptor.LeftLowerArm is GameObject leftLower && leftLower)
            {
                if (bodyPartsPerTag.TryGetValue("LeftLowerArm", out Transform bone)) leftLower.transform.SetParent(bone, true);
                leftLower.transform.SetParent(gameObject.transform, false);
                leftLower.name = "LeftLower";
            }

            if (descriptor.LeftHand is GameObject leftHand && leftHand)
            {
                if (bodyPartsPerTag.TryGetValue("LeftHand", out Transform bone)) leftHand.transform.SetParent(bone, true);
                leftHand.transform.SetParent(gameObject.transform, false);
                leftHand.name = "LeftHand";
            }

            if (descriptor.RightUpperArm is GameObject rightUpper && rightUpper)
            {
                if (bodyPartsPerTag.TryGetValue("RightUpperArm", out Transform bone)) rightUpper.transform.SetParent(bone, true);
                rightUpper.transform.SetParent(gameObject.transform, false);
                rightUpper.name = "RightUpper";
            }

            if (descriptor.RightLowerArm is GameObject rightLower && rightLower)
            {
                if (bodyPartsPerTag.TryGetValue("RightLowerArm", out Transform bone)) rightLower.transform.SetParent(bone, true);
                rightLower.transform.SetParent(gameObject.transform, false);
                rightLower.name = "RightLower";
            }

            if (descriptor.RightHand is GameObject rightHand && rightHand)
            {
                if (bodyPartsPerTag.TryGetValue("RightHand", out Transform bone)) rightHand.transform.SetParent(bone, true);
                rightHand.transform.SetParent(gameObject.transform, false);
                rightHand.name = "RightHand";
            }

            for (int i = 0; i < objectsToRemove.Count; i++)
            {
                if (objectsToRemove[i] == null || !objectsToRemove[i]) continue;
                Object.DestroyImmediate(objectsToRemove[i]);
            }

            EditorUtility.SetDirty(descriptor);

            string fileName = Path.GetFileName(path);

            Debug.Log($"Exporting shirt {descriptor.ShirtName} to {fileName}");

            (bool error, string errorMessage) = CheckShirt(descriptor);
            if (error && errorMessage != null)
            {
                Object.DestroyImmediate(gameObject);
                if (!PrefabStageUtility.GetCurrentPrefabStage() && !EditorUtility.IsPersistent(originalDescriptor.gameObject)) EditorSceneManager.SaveScene(originalDescriptor.gameObject.scene);
                Debug.Log($"Got error: {errorMessage}");
                EditorUtility.DisplayDialog($"Warning: {descriptor.ShirtName} has an error", errorMessage, "Ok");
                return;
            }

            Selection.activeObject = gameObject;

            string updatedPath = "Assets/GorillaShirtAsset.prefab";
            PrefabUtility.SaveAsPrefabAsset(Selection.activeObject as GameObject, updatedPath);
            AssetBundleBuild assetBundleBuild = default;
            assetBundleBuild.assetNames = new string[] { updatedPath };
            assetBundleBuild.assetBundleName = fileName;

            BuildPipeline.BuildAssetBundles(Application.temporaryCachePath, new AssetBundleBuild[] { assetBundleBuild }, 0, BuildTarget.StandaloneWindows64);

            string bundle_path = Path.Combine(Application.temporaryCachePath, fileName);

            if (File.Exists(path))
            {
                Debug.Log("Removed existing shirt at path");
                File.Delete(path);
            }

            if (File.Exists(bundle_path))
            {
                Debug.Log("Moved asset to path");
                File.Move(bundle_path, path);
            }

            if (File.Exists(updatedPath))
            {
                Debug.Log("Removed asset prefab");
                File.Delete(updatedPath);
                File.Delete(string.Concat(updatedPath, ".meta"));
            }

            AssetDatabase.Refresh();
            Object.DestroyImmediate(gameObject);

            var scene = originalDescriptor.gameObject.scene;
            // EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            EditorUtility.DisplayDialog($"Success", $"{originalDescriptor.ShirtName} has been exported", "Ok");
        }

        public static (bool error, string error_message) CheckShirt(ShirtDescriptor descriptor)
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() || EditorUtility.IsPersistent(descriptor.gameObject))
            {
                return (true, "This shirt must be exported in a scene, and not as a prefab");
            }

            Scene scene = descriptor.gameObject.scene;
            if (!scene.IsValid() || scene != SceneManager.GetActiveScene())
            {
                return (true, "This shirt must be exported in the active scene");
            }

            static bool IsValid(string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
            if (!IsValid(descriptor.ShirtName) || !IsValid(descriptor.PackName) || !IsValid(descriptor.Author))
            {
                return (true, "This shirt requires a name, author, and pack that cannot be null or empty");
            }

            static bool IsValidObject(GameObject gameObject) => gameObject != null && gameObject;

            if (!IsValidObject(descriptor.Body) 
            && !IsValidObject(descriptor.Head)
            && !IsValidObject(descriptor.LeftUpperArm)
            && !IsValidObject(descriptor.LeftLowerArm)
            && !IsValidObject(descriptor.LeftHand)
            && !IsValidObject(descriptor.RightUpperArm)
            && !IsValidObject(descriptor.RightLowerArm)
            && !IsValidObject(descriptor.RightHand))
            {
                return (true, "This shirt requires any object, whether that would be for the body, the head, the left/right limb");
            }

            return (false, null);
        }
    }
}
