using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextDisplayAnimate))]
public class TextDisplayAnimateEditor : Editor
{
    public SerializedObject mObj;

    public SerializedProperty database;
    public SerializedProperty tagname;

    public SerializedProperty valueType;
    public SerializedProperty decimalCount;
    public SerializedProperty trueString;
    public SerializedProperty falseString;
    public SerializedProperty timeFormate;

    /// <summary>
    /// 
    /// </summary>
    public void OnEnable()
    {
        mObj = new SerializedObject(target);
        this.database = mObj.FindProperty("database");
        this.tagname = mObj.FindProperty("tagName");
        this.valueType = mObj.FindProperty("ValueType");
        this.decimalCount = mObj.FindProperty("decimalCount");
        this.trueString = mObj.FindProperty("trueString");
        this.falseString = mObj.FindProperty("falseString");
        this.timeFormate = mObj.FindProperty("timeFormate");
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnInspectorGUI()
    {
        this.mObj.Update();
        EditorGUILayout.PropertyField(this.database);
        EditorGUILayout.PropertyField(this.tagname);
        EditorGUILayout.PropertyField(this.valueType);
        switch(this.valueType.enumValueIndex)
        {
            case 0:
                break;
            case 1:
                EditorGUILayout.PropertyField(this.decimalCount);
                break;
            case 2:
                EditorGUILayout.PropertyField(this.trueString);
                EditorGUILayout.PropertyField(this.falseString);
                break;
            case 3:
                EditorGUILayout.PropertyField(this.timeFormate);
                break;
        }
        this.mObj.ApplyModifiedProperties();
    }
}
