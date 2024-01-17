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

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using GorillaShirts.Data;
using System.Linq;

public class ExportWindow : EditorWindow
{
    public Vector2 scrollPosition = Vector2.zero;
    private ShirtDescriptor[] notes;

    private GUIStyle boldLabel, middleCenter;

    protected void OnEnable()
    {
        boldLabel = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(0, 0, 0, 4)
            };

        middleCenter = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter
            };
    }

    protected void OnFocus() => notes = GameObject.FindObjectsOfType<GorillaShirts.Data.ShirtDescriptor>();
    [MenuItem("GorillaShirts/Shirt Exporter")] public static void ShowWindow() => GetWindow(typeof(ExportWindow), false, "Shirt Exporter", true);

    protected void OnGUI()
    {
        var window = GetWindow(typeof(ExportWindow), false, "Shirt Exporter", false);

        int ScrollSpace = 16 + 20 + 16 + 17 + 17 + 20 + 20;
        ScrollSpace += (16 + 17 + 17 + 20 + 20 + 21 + 21) * (notes.Where(a => a != null).Count());

        float currentWindowWidth = EditorGUIUtility.currentViewWidth;
        float windowWidthIncludingScrollbar = currentWindowWidth;
        if (window.position.size.y >= ScrollSpace) windowWidthIncludingScrollbar += 30;
        scrollPosition = GUI.BeginScrollView(new Rect(0, 0, EditorGUIUtility.currentViewWidth, window.position.size.y), scrollPosition, new Rect(0, 0, EditorGUIUtility.currentViewWidth - 20, ScrollSpace), false, false);

        if (notes.Length == 0) GUILayout.Label("No shirt descriptors found!\nIf you're not already, go to Scenes > SampleScene and open it where there should be a descriptor.", middleCenter);
        GUILayout.Space(5.5f);
        foreach (var note in notes)
        {
            if (note != null)
            {
                GUILayout.Label($"Descriptor: {note.gameObject.name}", boldLabel);
                GUILayout.Space(5.5f);
                note.Name = EditorGUILayout.TextField("Name", note.Name, GUILayout.Width(windowWidthIncludingScrollbar - 40), GUILayout.Height(17));
                GUILayout.Space(2f);
                note.Pack = EditorGUILayout.TextField("Pack", note.Pack, GUILayout.Width(windowWidthIncludingScrollbar - 40), GUILayout.Height(17));
                GUILayout.Space(5f);

                if (GUILayout.Button("Export " + note.Name, GUILayout.Width(windowWidthIncludingScrollbar - 40), GUILayout.Height(20)))
                {
                    GameObject noteObject = note.gameObject;
                    noteObject.TryGetComponent(out ShirtDescriptor descriptor);

                    if (descriptor != null && (string.IsNullOrEmpty(descriptor.Pack) || string.IsNullOrWhiteSpace(descriptor.Pack)))
                    {
                        bool packNotice = EditorUtility.DisplayDialog("Export Warning", "Please make sure a valid name is assigned to the pack attached to this shirt, as it cannot be empty.", "Set it to \"Custom\"", "Close");
                        if (!packNotice) return;
                        descriptor.Pack = "Custom";
                    }

                    if (descriptor != null && descriptor.Body != null && descriptor.Name != string.Empty && descriptor.Author != string.Empty && descriptor.Info != string.Empty)
                    {
                        if (descriptor.ShirtSound1 && !descriptor.ShirtSound1.preloadAudioData)
                        {
                            EditorUtility.DisplayDialog("Export Warning", $"Please enable 'Preload Audio Data' for wear audio '{descriptor.ShirtSound1.name}'", "Close");
                            return;
                        }

                        if (descriptor.ShirtSound2 && !descriptor.ShirtSound2.preloadAudioData)
                        {
                            EditorUtility.DisplayDialog("Export Warning", $"Please enable 'Preload Audio Data' for remove audio '{descriptor.ShirtSound2.name}'", "Close");
                            return;
                        }


                        Debug.Log("Attempting to save Shirt file");
                        string path = EditorUtility.SaveFilePanel("Save Shirt file", "", note.Name + ".shirt", "Shirt");

                        if (path != "")
                        {
                            Debug.Log("Exporting shirt with path: " + path);

                            GameObject newO = Instantiate(noteObject);
                            ShirtDescriptor shirtD = newO.GetComponent<ShirtDescriptor>();

                            EditorUtility.SetDirty(shirtD);
                            GameObject baseObject = new("TempShirtObject");

                            #region Get objects

                            List<Fur> furObjects = new();
                            List<WobbleBone> wobbleObjects = new();
                            List<WobbleLock> ignoreWobbleObjects = new();
                            List<Billboard> billboardObjects = new();

                            // Body

                            shirtD.Body.transform.SetParent(baseObject.transform, false);
                            shirtD.Body.name = "BodyObject";

                            foreach (var shirtComponent in shirtD.Body.GetComponentsInChildren<ShirtComponent>())
                            {
                                if (shirtComponent.GetComponent<Fur>())
                                {
                                    furObjects.Add(shirtComponent.GetComponent<Fur>());
                                }
                                if (shirtComponent.GetComponent<Billboard>())
                                {
                                    billboardObjects.Add(shirtComponent.GetComponent<Billboard>());
                                }
                                if (shirtComponent.GetComponent<WobbleBone>())
                                {
                                    wobbleObjects.Add(shirtComponent.GetComponent<WobbleBone>());
                                }
                                else if (shirtComponent.GetComponent<WobbleLock>())
                                {
                                    ignoreWobbleObjects.Add(shirtComponent.GetComponent<WobbleLock>());
                                }
                            }

                            // Left arm (Upper)
                            if (shirtD.LeftUpperArm != null)
                            {
                                shirtD.LeftUpperArm.transform.SetParent(baseObject.transform, false);
                                shirtD.LeftUpperArm.name = "LUpperArm";

                                foreach (var shirtComponent in shirtD.LeftUpperArm.GetComponentsInChildren<ShirtComponent>())
                                {
                                    if (shirtComponent.GetComponent<Fur>())
                                    {
                                        furObjects.Add(shirtComponent.GetComponent<Fur>());
                                    }
                                    if (shirtComponent.GetComponent<Billboard>())
                                    {
                                        billboardObjects.Add(shirtComponent.GetComponent<Billboard>());
                                    }
                                    if (shirtComponent.GetComponent<WobbleBone>())
                                    {
                                        wobbleObjects.Add(shirtComponent.GetComponent<WobbleBone>());
                                    }
                                    else if (shirtComponent.GetComponent<WobbleLock>())
                                    {
                                        ignoreWobbleObjects.Add(shirtComponent.GetComponent<WobbleLock>());
                                    }
                                }
                            }
                            // Right arm (Upper)
                            if (shirtD.RightUpperArm != null)
                            {
                                shirtD.RightUpperArm.transform.SetParent(baseObject.transform, false);
                                shirtD.RightUpperArm.name = "RUpperArm";

                                foreach (var shirtComponent in shirtD.RightUpperArm.GetComponentsInChildren<ShirtComponent>())
                                {
                                    if (shirtComponent.GetComponent<Fur>())
                                    {
                                        furObjects.Add(shirtComponent.GetComponent<Fur>());
                                    }
                                    if (shirtComponent.GetComponent<Billboard>())
                                    {
                                        billboardObjects.Add(shirtComponent.GetComponent<Billboard>());
                                    }
                                    if (shirtComponent.GetComponent<WobbleBone>())
                                    {
                                        wobbleObjects.Add(shirtComponent.GetComponent<WobbleBone>());
                                    }
                                    else if (shirtComponent.GetComponent<WobbleLock>())
                                    {
                                        ignoreWobbleObjects.Add(shirtComponent.GetComponent<WobbleLock>());
                                    }
                                }
                            }
                            // Left arm (Lower)
                            if (shirtD.LeftLowerArm != null)
                            {
                                shirtD.LeftLowerArm.transform.SetParent(baseObject.transform, false);
                                shirtD.LeftLowerArm.name = "LLowerArm";

                                foreach (var shirtComponent in shirtD.LeftLowerArm.GetComponentsInChildren<ShirtComponent>())
                                {
                                    if (shirtComponent.GetComponent<Fur>())
                                    {
                                        furObjects.Add(shirtComponent.GetComponent<Fur>());
                                    }
                                    if (shirtComponent.GetComponent<Billboard>())
                                    {
                                        billboardObjects.Add(shirtComponent.GetComponent<Billboard>());
                                    }
                                    if (shirtComponent.GetComponent<WobbleBone>())
                                    {
                                        wobbleObjects.Add(shirtComponent.GetComponent<WobbleBone>());
                                    }
                                    else if (shirtComponent.GetComponent<WobbleLock>())
                                    {
                                        ignoreWobbleObjects.Add(shirtComponent.GetComponent<WobbleLock>());
                                    }
                                }
                            }
                            // Right arm (Lower)
                            if (shirtD.RightLowerArm != null)
                            {
                                shirtD.RightLowerArm.transform.SetParent(baseObject.transform, false);
                                shirtD.RightLowerArm.name = "RLowerArm";

                                foreach (var shirtComponent in shirtD.RightLowerArm.GetComponentsInChildren<ShirtComponent>())
                                {
                                    if (shirtComponent.GetComponent<Fur>())
                                    {
                                        furObjects.Add(shirtComponent.GetComponent<Fur>());
                                    }
                                    if (shirtComponent.GetComponent<Billboard>())
                                    {
                                        billboardObjects.Add(shirtComponent.GetComponent<Billboard>());
                                    }
                                    if (shirtComponent.GetComponent<WobbleBone>())
                                    {
                                        wobbleObjects.Add(shirtComponent.GetComponent<WobbleBone>());
                                    }
                                    else if (shirtComponent.GetComponent<WobbleLock>())
                                    {
                                        ignoreWobbleObjects.Add(shirtComponent.GetComponent<WobbleLock>());
                                    }
                                }
                            }
                            // Left hand
                            if (shirtD.LeftHand != null)
                            {
                                shirtD.LeftHand.transform.SetParent(baseObject.transform, false);
                                shirtD.LeftHand.name = "LHand";

                                foreach (var shirtComponent in shirtD.LeftHand.GetComponentsInChildren<ShirtComponent>())
                                {
                                    if (shirtComponent.GetComponent<Fur>())
                                    {
                                        furObjects.Add(shirtComponent.GetComponent<Fur>());
                                    }
                                    if (shirtComponent.GetComponent<Billboard>())
                                    {
                                        billboardObjects.Add(shirtComponent.GetComponent<Billboard>());
                                    }
                                    if (shirtComponent.GetComponent<WobbleBone>())
                                    {
                                        wobbleObjects.Add(shirtComponent.GetComponent<WobbleBone>());
                                    }
                                    else if (shirtComponent.GetComponent<WobbleLock>())
                                    {
                                        ignoreWobbleObjects.Add(shirtComponent.GetComponent<WobbleLock>());
                                    }
                                }
                            }
                            // Right hand
                            if (shirtD.RightHand != null)
                            {
                                shirtD.RightHand.transform.SetParent(baseObject.transform, false);
                                shirtD.RightHand.name = "RHand";

                                foreach (var shirtComponent in shirtD.RightHand.GetComponentsInChildren<ShirtComponent>())
                                {
                                    if (shirtComponent.GetComponent<Fur>())
                                    {
                                        furObjects.Add(shirtComponent.GetComponent<Fur>());
                                    }
                                    if (shirtComponent.GetComponent<Billboard>())
                                    {
                                        billboardObjects.Add(shirtComponent.GetComponent<Billboard>());
                                    }
                                    if (shirtComponent.GetComponent<WobbleBone>())
                                    {
                                        wobbleObjects.Add(shirtComponent.GetComponent<WobbleBone>());
                                    }
                                    else if (shirtComponent.GetComponent<WobbleLock>())
                                    {
                                        ignoreWobbleObjects.Add(shirtComponent.GetComponent<WobbleLock>());
                                    }
                                }
                            }
                            // Head
                            if (shirtD.Head != null)
                            {
                                shirtD.Head.transform.SetParent(baseObject.transform, false);
                                shirtD.Head.name = "HeadObject";

                                foreach (var shirtComponent in shirtD.Head.GetComponentsInChildren<ShirtComponent>())
                                {
                                    if (shirtComponent.GetComponent<Fur>())
                                    {
                                        furObjects.Add(shirtComponent.GetComponent<Fur>());
                                    }
                                    if (shirtComponent.GetComponent<Billboard>())
                                    {
                                        billboardObjects.Add(shirtComponent.GetComponent<Billboard>());
                                    }
                                    if (shirtComponent.GetComponent<WobbleBone>())
                                    {
                                        wobbleObjects.Add(shirtComponent.GetComponent<WobbleBone>());
                                    }
                                    else if (shirtComponent.GetComponent<WobbleLock>())
                                    {
                                        ignoreWobbleObjects.Add(shirtComponent.GetComponent<WobbleLock>());
                                    }
                                }
                            }
                            #endregion

                            void CheckAudio(AudioClip clip, string name)
                            {
                                if (clip)
                                {
                                    GameObject storedObject = new GameObject(name);
                                    storedObject.transform.SetParent(baseObject.transform);

                                    AudioSource audioSource = storedObject.AddComponent<AudioSource>();
                                    audioSource.clip = clip;
                                    audioSource.playOnAwake = false;
                                }
                            }

                            CheckAudio(descriptor.ShirtSound1, "OverrideWearClip");
                            CheckAudio(descriptor.ShirtSound2, "OverrideRemoveClip");

                            foreach (var furObject in furObjects)
                            {
                                if (furObject == null) continue;

                                GameObject colourObject = new("G_Fur" + ((int)furObject.mode).ToString());
                                colourObject.transform.SetParent(furObject.transform, false);
                                DestroyImmediate(furObject);
                            }

                            foreach(var wobbleBone in wobbleObjects)
                            {
                                if (wobbleBone == null) continue;

                                GameObject wobbleObj = new("Wobble0");
                                wobbleObj.transform.SetParent(wobbleBone.transform, false);
                                DestroyImmediate(wobbleBone);
                            }

                            foreach(var ignoreWobble in ignoreWobbleObjects)
                            {
                                if (ignoreWobble == null) continue;

                                GameObject ignoreWobbleObj = new("Wobble1");
                                ignoreWobbleObj.transform.SetParent(ignoreWobble.transform, false);
                                DestroyImmediate(ignoreWobble);
                            }

                            foreach(var billboard in billboardObjects)
                            {
                                if (billboard == null) continue;

                                GameObject billboardObject = new("G_BB" + ((int)billboard.mode).ToString());
                                billboardObject.transform.SetParent(billboard.transform, false);
                                DestroyImmediate(billboard);
                            }

                            foreach (var collider in baseObject.GetComponentsInChildren<Collider>()) DestroyImmediate(collider);
                            DestroyImmediate(newO);

                            Compiler.ExportShirt(baseObject, path, Compiler.ShirtToShirtJSON(note));
                            DestroyImmediate(baseObject);
                            Debug.Log("Exported shirt");

                            AssetDatabase.Refresh();
                            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

                            bool reveal = EditorUtility.DisplayDialog("Export Success", "Your shirt was exported successfully!", "Open", "Close");
                            if (reveal)
                            {
                                Debug.Log("Revealing path");
                                EditorUtility.RevealInFinder(path);
                            }
                        }
                        else EditorUtility.DisplayDialog("Export Warning", "Please assign a valid path as to where your shirt will be located at.", "Close");
                    }
                    else EditorUtility.DisplayDialog("Export Warning", "Plase assign a Name, Author, Description, and Body when creating a shirt.", "Close");
                }
                GUILayout.Space(20);
            }
        }
        GUI.EndScrollView();
    }
}