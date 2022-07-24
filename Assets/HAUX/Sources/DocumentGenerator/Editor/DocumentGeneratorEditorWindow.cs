/* ----------------------------------------------------------------------- *

    * DocumentGeneratorEditorWindow.cs.cs

    ----------------------------------------------------------------------

    Copyright (C) 2022, HERO CONCEPT YAZILIM VE BİLİŞİM ANONİM ŞİRKETİ
    All Rights Reserved.

    THIS SOFTWARE IS PROVIDED 'AS-IS', WITHOUT ANY EXPRESS
    OR IMPLIED WARRANTY. IN NO EVENT WILL THE AUTHOR(S) BE HELD LIABLE FOR
    ANY DAMAGES ARISING FROM THE USE OR DISTRIBUTION OF THIS SOFTWARE

    Hero Concept <info@heroconcept.com>

* ------------------------------------------------------------------------ */
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.Compilation;
#endif

namespace hc {

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CustomEditor(typeof(DocumentGenerator))]
public class DocumentGeneratorEditorWindow: EditorWindow
{
  ReorderableList listView;
  SerializedProperty serializedFilters;
  Vector2 scrollPos;
  SerializedObject serializedObject;
  ListView obj;

  // -------------------------------------------------------------------------
  [MenuItem("Hero Concept/Document Generator &#d")]
  public static void  ShowWindow () 
  {
    EditorWindow documentWindow = EditorWindow.GetWindow<DocumentGeneratorEditorWindow>();
    documentWindow.title = "Document Generator";
    documentWindow.minSize = new Vector2(500, 750);
    
  }

  // -------------------------------------------------------------------------
  private void OnEnable()
  {
    obj = ScriptableObject.CreateInstance<ListView>();
    serializedObject = new UnityEditor.SerializedObject(obj);

    serializedFilters = serializedObject.FindProperty("myDlls");
    serializedObject.ApplyModifiedProperties();

    DocumentGenerator.createFilters();
  }

  // -------------------------------------------------------------------------
  public void OnGUI()
  {
    serializedObject.Update();
    GUILayout.Label("User options :", EditorStyles.boldLabel);
    GUILayout.Space(10);
    DocumentGenerator.settings.generateEveryCompile = GUILayout.Toggle(DocumentGenerator.settings.generateEveryCompile, "Generate documents every compilination (Not recommended)");
    DocumentGenerator.settings.generateHaux = GUILayout.Toggle(DocumentGenerator.settings.generateHaux, "Generate Haux");
    DocumentGenerator.settings.generateOtherScripts = GUILayout.Toggle(DocumentGenerator.settings.generateOtherScripts, "Generate Other Scripts");  
    GUILayout.Space(10);
    GUILayout.Label("Documentation options: ", EditorStyles.boldLabel);
    GUILayout.Space(10);
    GUILayout.BeginHorizontal();
    GUILayout.Label("Application Documentation Root:");
    GUILayout.TextField(DocumentGenerator.settings.applicationDocumentaionRoot);
    GUILayout.EndHorizontal();
    GUILayout.BeginHorizontal();
    GUILayout.Label("File extension");
    GUILayout.TextField(DocumentGenerator.settings.fileExtension);
    GUILayout.EndHorizontal(); 
    GUILayout.Space(10);
    DocumentGenerator.onGUI();
    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(450), GUILayout.Height(650));
    EditorGUILayout.PropertyField(serializedFilters, new GUIContent("Filtered dlls"));


    EditorGUILayout.EndScrollView();
    if(GUILayout.Button("Clear Filters")) 
    {
      DocumentGenerator.filters.Clear();
    }

    if(GUILayout.Button("Fill Filters"))
    {
      DocumentGenerator.createFilters();
    }

    serializedObject.ApplyModifiedProperties();
  }

}


// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class ListView : ScriptableObject
{
  public List<string> myDlls = DocumentGenerator.filters;
}


} // End of namespace hc