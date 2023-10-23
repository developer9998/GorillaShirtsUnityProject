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
                        Debug.Log("Attempting to save Shirt file");
                        string path = EditorUtility.SaveFilePanel("Save Shirt file", "", note.Name + ".shirt", "Shirt");

                        if (path != "")
                        {
                            Debug.Log("Exporting shirt with path: " + path);

                            GameObject newO = Instantiate(noteObject);
                            ShirtDescriptor shirtD = newO.GetComponent<ShirtDescriptor>();

                            EditorUtility.SetDirty(shirtD);
                            GameObject baseObject = new GameObject("TempShirtObject");

                            #region Get objects

                            // Body
                            List<FurMatch> furObjects = new List<FurMatch>();
                            shirtD.Body.transform.SetParent(baseObject.transform, false);
                            shirtD.Body.name = "BodyObject";
                            foreach (var FurObject in shirtD.Body.transform.GetComponentsInChildren<FurMatch>()) furObjects.Add(FurObject);

                            // Left arm (Upper)
                            if (shirtD.LeftUpperArm != null)
                            {
                                shirtD.LeftUpperArm.transform.SetParent(baseObject.transform, false);
                                shirtD.LeftUpperArm.name = "LUpperArm";
                                foreach (var FurObject in shirtD.LeftUpperArm.transform.GetComponentsInChildren<FurMatch>()) furObjects.Add(FurObject);
                            }
                            // Right arm (Upper)
                            if (shirtD.RightUpperArm != null)
                            {
                                shirtD.RightUpperArm.transform.SetParent(baseObject.transform, false);
                                shirtD.RightUpperArm.name = "RUpperArm";
                                foreach (var FurObject in shirtD.RightUpperArm.transform.GetComponentsInChildren<FurMatch>()) furObjects.Add(FurObject);
                            }
                            // Left arm (Lower)
                            if (shirtD.LeftLowerArm != null)
                            {
                                shirtD.LeftLowerArm.transform.SetParent(baseObject.transform, false);
                                shirtD.LeftLowerArm.name = "LLowerArm";
                                foreach (var FurObject in shirtD.LeftLowerArm.transform.GetComponentsInChildren<FurMatch>()) furObjects.Add(FurObject);
                            }
                            // Right arm (Lower)
                            if (shirtD.RightLowerArm != null)
                            {
                                shirtD.RightLowerArm.transform.SetParent(baseObject.transform, false);
                                shirtD.RightLowerArm.name = "RLowerArm";
                                foreach (var FurObject in shirtD.RightLowerArm.transform.GetComponentsInChildren<FurMatch>()) furObjects.Add(FurObject);
                            }
                            // Left hand
                            if (shirtD.LeftHand != null)
                            {
                                shirtD.LeftHand.transform.SetParent(baseObject.transform, false);
                                shirtD.LeftHand.name = "LHand";
                                foreach (var FurObject in shirtD.LeftHand.transform.GetComponentsInChildren<FurMatch>()) furObjects.Add(FurObject);
                            }
                            // Right hand
                            if (shirtD.RightHand != null)
                            {
                                shirtD.RightHand.transform.SetParent(baseObject.transform, false);
                                shirtD.RightHand.name = "RHand";
                                foreach (var FurObject in shirtD.RightHand.transform.GetComponentsInChildren<FurMatch>()) furObjects.Add(FurObject);
                            }
                            // Head
                            if (shirtD.Head != null)
                            {
                                shirtD.Head.transform.SetParent(baseObject.transform, false);
                                shirtD.Head.name = "HeadObject";
                                foreach (var FurObject in shirtD.Head.transform.GetComponentsInChildren<FurMatch>()) furObjects.Add(FurObject);
                            }
                            #endregion

                            foreach (var furObject in furObjects)
                            {
                                if (furObject == null) continue;
                                GameObject colorObject = new GameObject("G_Fur" + ((int)furObject.mode).ToString());
                                colorObject.transform.SetParent(furObject.transform, false);
                                DestroyImmediate(furObject);
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