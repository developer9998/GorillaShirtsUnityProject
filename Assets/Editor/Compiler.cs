/* MIT License

Copyright (c) 2021 legoandmars

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using System.IO.Compression;

public class Compiler : MonoBehaviour
{
    public static ShirtJSON ShirtToShirtJSON(GorillaShirts.Data.ShirtDescriptor descriptor)
    {
        ShirtJSON shirtJSON = new ShirtJSON();
        shirtJSON.descriptor = new SDescriptor();
        shirtJSON.config = new SConfig();
        shirtJSON.descriptor.shirtAuthor = descriptor.Author;
        shirtJSON.descriptor.shirtName = descriptor.Name;
        shirtJSON.descriptor.shirtInfo = descriptor.Info;
        shirtJSON.config.customColors = descriptor.customColors;
        shirtJSON.config.invisibility = descriptor.invisibility;
        shirtJSON.config.SillyNSteady = descriptor.SillyNSteady;
        shirtJSON.config.isCreator = descriptor.isCreator;
        return shirtJSON;
    }

    public static void ExportShirt(GameObject gameObject, string path, ShirtJSON shirtJSON)
    {
        string folderPath = Path.GetDirectoryName(path);
        string fileName = Path.GetFileNameWithoutExtension(path) + "_file";

        Selection.activeObject = gameObject;
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
        EditorSceneManager.SaveScene(gameObject.scene);

        string updatedPath = $"Assets/ExportShirt.prefab";
        PrefabUtility.SaveAsPrefabAsset(Selection.activeObject as GameObject, updatedPath);
        AssetBundleBuild assetBundleBuild = default;
        assetBundleBuild.assetNames = new string[] { updatedPath };
        assetBundleBuild.assetBundleName = fileName;

        BuildPipeline.BuildAssetBundles(Application.temporaryCachePath, new AssetBundleBuild[] { assetBundleBuild }, 0, BuildTarget.StandaloneWindows64);
        EditorPrefs.SetString("currentBuildingAssetBundlePath", folderPath);

        shirtJSON.fileName = fileName;
        string json = JsonUtility.ToJson(shirtJSON, true);
        File.WriteAllText(Application.temporaryCachePath + "/package.json", json);
        AssetDatabase.DeleteAsset(updatedPath);

        if (File.Exists(Application.temporaryCachePath + "/tempZip.zip")) File.Delete(Application.temporaryCachePath + "/tempZip.zip");
        CreateZipFile(Application.temporaryCachePath + "/tempZip.zip", new List<string> { Application.temporaryCachePath + "/" + fileName, Application.temporaryCachePath + "/package.json" });
        if (File.Exists(path)) File.Delete(path);
        File.Move(Application.temporaryCachePath + "/tempZip.zip", path);
        DestroyImmediate(gameObject);

        AssetDatabase.Refresh();
    }

    public static void CreateZipFile(string fileName, IEnumerable<string> files)
    {
        var zip = ZipFile.Open(fileName, ZipArchiveMode.Create);
        foreach (var file in files) zip.CreateEntryFromFile(file, Path.GetFileName(file), System.IO.Compression.CompressionLevel.Optimal);
        zip.Dispose();
    }
}
