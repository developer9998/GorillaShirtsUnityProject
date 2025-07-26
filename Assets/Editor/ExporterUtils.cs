using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using GorillaShirts.Behaviours.Cosmetic;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GorillaShirtsUnityProject
{
    public class ExporterUtils
    {
        public static void ExportPackage(ShirtDescriptor originalDescriptor, string path)
        {
            GameObject gameObject = Object.Instantiate(originalDescriptor.gameObject);
            gameObject.TryGetComponent(out ShirtDescriptor descriptor);

            List<GameObject> children = new();
            foreach(Transform child in gameObject.transform)
            {
                children.Add(child.gameObject);
            }

            if (descriptor.Body is GameObject body && body)
            {
                body.transform.SetParent(gameObject.transform, false);
                body.name = "Body";
            }

            if (descriptor.Head is GameObject head && head)
            {
                head.transform.SetParent(gameObject.transform, false);
                head.name = "Head";
            }

            if (descriptor.LeftUpperArm is GameObject leftUpper && leftUpper)
            {
                leftUpper.transform.SetParent(gameObject.transform, false);
                leftUpper.name = "LeftUpper";
            }

            if (descriptor.LeftLowerArm is GameObject leftLower && leftLower)
            {
                leftLower.transform.SetParent(gameObject.transform, false);
                leftLower.name = "LeftLower";
            }

            if (descriptor.LeftHand is GameObject leftHand && leftHand)
            {
                leftHand.transform.SetParent(gameObject.transform, false);
                leftHand.name = "LeftHand";
            }

            if (descriptor.RightUpperArm is GameObject rightUpper && rightUpper)
            {
                rightUpper.transform.SetParent(gameObject.transform, false);
                rightUpper.name = "RightUpper";
            }

            if (descriptor.RightLowerArm is GameObject rightLower && rightLower)
            {
                rightLower.transform.SetParent(gameObject.transform, false);
                rightLower.name = "RightLower";
            }

            if (descriptor.RightHand is GameObject rightHand && rightHand)
            {
                rightHand.transform.SetParent(gameObject.transform, false);
                rightHand.name = "RightHand";
            }

            for (int i = 0; i < children.Count; i++)
            {
                Object.DestroyImmediate(children[i]);
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

            string updatedPath = $"Assets/GorillaShirtAsset.prefab";
            PrefabUtility.SaveAsPrefabAsset(Selection.activeObject as GameObject, updatedPath);
            AssetBundleBuild assetBundleBuild = default;
            assetBundleBuild.assetNames = new string[] { updatedPath };
            assetBundleBuild.assetBundleName = fileName;

            BuildPipeline.BuildAssetBundles(Application.temporaryCachePath, new AssetBundleBuild[] { assetBundleBuild }, 0, BuildTarget.StandaloneWindows64);

            string bundle_path = Path.Combine(Application.temporaryCachePath, fileName);

            if (File.Exists(path))
            {
                //Debug.Log("Removed existing shirt at path");
                File.Delete(path);
            }

            if (File.Exists(bundle_path))
            {
                //Debug.Log("Moved asset to path");
                File.Move(bundle_path, path);
            }

            if (File.Exists(updatedPath))
            {
                //Debug.Log("Removed asset prefab");
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
