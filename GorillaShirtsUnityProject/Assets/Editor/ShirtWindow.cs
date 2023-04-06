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
using GorillaShirts.Data;

public class ShirtWindow : EditorWindow
{
    public Vector2 scrollPosition = Vector2.zero;
    private ShirtDescriptor[] notes;

    protected void OnFocus() => notes = GameObject.FindObjectsOfType<GorillaShirts.Data.ShirtDescriptor>();
    [MenuItem("GorillaShirts/Shirt Exporter")] public static void ShowWindow() => GetWindow(typeof(ShirtWindow), false, "Shirt Exporter", false);

    public void SetParentTest(Transform obj, Transform parent, bool world)
    {
        if (obj != null && parent != null)
        {
            try
            {
                obj.transform.SetParent(parent.transform, world);
            }
            catch { }
        }
    }

    protected void OnGUI()
    {
        var window = GetWindow(typeof(ShirtWindow), false, "Shirt Exporter", false);

        int ScrollSpace = (16 + 20) + (16 + 17 + 17 + 20 + 20);
        foreach (var note in notes) if (note != null) ScrollSpace += 16 + 17 + 17 + 20 + 20; // specific
        float currentWindowWidth = EditorGUIUtility.currentViewWidth;
        float windowWidthIncludingScrollbar = currentWindowWidth;
        if (window.position.size.y >= ScrollSpace) windowWidthIncludingScrollbar += 30;
        scrollPosition = GUI.BeginScrollView(new Rect(0, 0, EditorGUIUtility.currentViewWidth, window.position.size.y), scrollPosition, new Rect(0, 0, EditorGUIUtility.currentViewWidth - 20, ScrollSpace), false, false);

        Font utopium = Resources.Load("Utopium-Regular") as Font;

        // GUIStyles
        GUIStyle titleLabel = new GUIStyle();
        titleLabel.fontStyle = FontStyle.Normal;
        titleLabel.alignment = TextAnchor.MiddleCenter;
        titleLabel.font = utopium;
        titleLabel.fontSize = 20;

        GUIStyle creditLabel = new GUIStyle();
        creditLabel.alignment = TextAnchor.MiddleCenter;
        creditLabel.font = utopium;
        creditLabel.fontSize = 14;

        GUIStyle boldLabel = new GUIStyle();
        boldLabel.alignment = TextAnchor.MiddleCenter;
        boldLabel.font = utopium;
        boldLabel.fontSize = 16;

        GUIStyle middleCenter = new GUIStyle();
        middleCenter.alignment = TextAnchor.MiddleCenter;

        // Intro
        GUILayout.Space(18);
        GUILayout.Label("GorillaShirts Exporter".ToUpper(), titleLabel);
        GUILayout.Space(3);
        GUILayout.Label("A mod by dev9998".ToUpper(), creditLabel);
        GUILayout.Space(10);

        if (notes.Length == 0) GUILayout.Label("No shirt descriptors found!\nIf you're not already, go to Scenes > SampleScene and open it where there should be a descriptor.", middleCenter);

        foreach (var note in notes)
        {
            if (note != null)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Label(string.Join(": ", "GameObject", note.gameObject.name).ToUpper(), boldLabel);
                GUILayout.Space(5);
                note.Name = EditorGUILayout.TextField("Shirt Name", note.Name, GUILayout.Width(windowWidthIncludingScrollbar - 40), GUILayout.Height(17));
                note.Author = EditorGUILayout.TextField("Shirt Author", note.Author, GUILayout.Width(windowWidthIncludingScrollbar - 40), GUILayout.Height(17));
                note.Info = EditorGUILayout.TextField("Shirt Information", note.Info, GUILayout.Width(windowWidthIncludingScrollbar - 40), GUILayout.Height(17));
                GUILayout.Space(3);

                if (GUILayout.Button("Export " + note.Name, GUILayout.Width(windowWidthIncludingScrollbar - 40), GUILayout.Height(20)))
                {
                    GameObject noteObject = note.gameObject;
                    noteObject.TryGetComponent(out ShirtDescriptor descriptor);
                    if (noteObject != null && note != null && descriptor.Body != null && descriptor.Name != string.Empty && descriptor.Author != string.Empty && descriptor.Info != string.Empty)
                    {
                        Debug.Log("Attempting to save Shirt file");
                        string path = EditorUtility.SaveFilePanel("Save Shirt file", "", note.Name + ".shirt", "Shirt");

                        if (path != "")
                        {
                            Debug.Log("Exporting shirt with path: " + path);

                            GameObject mBody = GameObject.Find("Gorilla Rig/body");
                            GameObject mHead = GameObject.Find("Gorilla Rig/body/head");
                            GameObject b1 = GameObject.Find("Gorilla Rig/body/shoulder.L/upper_arm.L");
                            GameObject b2 = GameObject.Find("Gorilla Rig/body/shoulder.L/upper_arm.L/forearm.L");
                            GameObject bHand1 = GameObject.Find("Gorilla Rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L");
                            GameObject b3 = GameObject.Find("Gorilla Rig/body/shoulder.R/upper_arm.R");
                            GameObject b4 = GameObject.Find("Gorilla Rig/body/shoulder.R/upper_arm.R/forearm.R");
                            GameObject bHand2 = GameObject.Find("Gorilla Rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R");

                            descriptor.Body.transform.SetParent(mBody.transform ?? null, true);
                            SetParentTest(descriptor.Head == null ? null : descriptor.Head.transform, mHead.transform ?? null, true);
                            SetParentTest(descriptor.LeftUpperArm == null ? null : descriptor.LeftUpperArm.transform, b1.transform ?? null, true);
                            SetParentTest(descriptor.LeftLowerArm == null ? null : descriptor.LeftLowerArm.transform, b2.transform ?? null, true);
                            SetParentTest(descriptor.LeftHand == null ? null : descriptor.LeftHand.transform, bHand1.transform ?? null, true);
                            SetParentTest(descriptor.RightUpperArm == null ? null : descriptor.RightUpperArm.transform, b3.transform ?? null, true);
                            SetParentTest(descriptor.RightLowerArm == null ? null : descriptor.RightLowerArm.transform, b4.transform ?? null, true);
                            SetParentTest(descriptor.RightHand == null ? null : descriptor.RightHand.transform, bHand2.transform ?? null, true);

                            GameObject newO = Instantiate(noteObject);
                            ShirtDescriptor shirtD = newO.GetComponent<ShirtDescriptor>();

                            EditorUtility.SetDirty(shirtD);
                            GameObject baseObject = new GameObject("TempShirtObject");

                            shirtD.Body.transform.SetParent(baseObject.transform, false);
                            shirtD.Body.name = "BodyObject";
                            if (shirtD.Boobs != null)
                            {
                                shirtD.Boobs.transform.SetParent(shirtD.Body.transform);
                                shirtD.Boobs.name = "BoobObject";
                            }
                            if (shirtD.LeftUpperArm != null)
                            {
                                shirtD.LeftUpperArm.transform.SetParent(baseObject.transform, false);
                                shirtD.LeftUpperArm.name = "LUpperArm";
                            }
                            if (shirtD.RightUpperArm != null)
                            {
                                shirtD.RightUpperArm.transform.SetParent(baseObject.transform, false);
                                shirtD.RightUpperArm.name = "RUpperArm";
                            }
                            if (shirtD.LeftLowerArm != null)
                            {
                                shirtD.LeftLowerArm.transform.SetParent(baseObject.transform, false);
                                shirtD.LeftLowerArm.name = "LLowerArm";
                            }
                            if (shirtD.RightLowerArm != null)
                            {
                                shirtD.RightLowerArm.transform.SetParent(baseObject.transform, false);
                                shirtD.RightLowerArm.name = "RLowerArm";
                            }
                            if (shirtD.LeftHand != null)
                            {
                                shirtD.LeftHand.transform.SetParent(baseObject.transform, false);
                                shirtD.LeftHand.name = "LHand";
                            }
                            if (shirtD.RightHand != null)
                            {
                                shirtD.RightHand.transform.SetParent(baseObject.transform, false);
                                shirtD.RightHand.name = "RHand";
                            }
                            if (shirtD.Head != null)
                            {
                                shirtD.Head.transform.SetParent(baseObject.transform, false);
                                shirtD.Head.name = "HeadObject";
                            }
                            if (shirtD.FurTextures.Count > 0)
                            {
                                foreach (var furObject in shirtD.FurTextures)
                                {
                                    GameObject colorObject = new GameObject("RegisteredFurTexture");
                                    colorObject.transform.SetParent(furObject.transform, false);
                                }
                            }

                            foreach (var collider in baseObject.GetComponentsInChildren<Collider>()) DestroyImmediate(collider);
                            DestroyImmediate(newO);

                            ShirtExporter.ExportShirt(baseObject, path, ShirtExporter.ShirtToShirtJSON(note));
                            DestroyImmediate(baseObject);

                            Debug.Log("Successfully saved shirt");
                            EditorUtility.DisplayDialog("Export success", "Your shirt was exported successfully.", "OK");
                            Debug.Log("Opening path");
                            EditorUtility.RevealInFinder(path);
                        }
                        else EditorUtility.DisplayDialog("Export failed", "Please set a valid path", "OK");
                    }
                    else EditorUtility.DisplayDialog("Export failed", "Please ensure your descriptor exists and you filled out a Name, Author, Info, and a Body object", "OK");
                }
                GUILayout.Space(20);
            }
        }
        GUI.EndScrollView();
    }
}