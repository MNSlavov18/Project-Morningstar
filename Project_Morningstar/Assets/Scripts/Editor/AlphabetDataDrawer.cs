using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AlphabetData))]
[CanEditMultipleObjects]
[System.Serializable]
public class AlphabetDataDrawer : Editor
{
    private ReorderableList AphabetPlainList;
    private ReorderableList AphabetNormalList;
    private ReorderableList AphabetHightList;
    private ReorderableList AphabetWrongList;

    private void OnEnable()
    {
        InitialisedReordableList(ref AphabetPlainList, "AlphabetPlain", "Alphabet Plain");
        InitialisedReordableList(ref AphabetNormalList, "AlphabetNormal", "Alphabet Normal");
        InitialisedReordableList(ref AphabetHightList, "AlphabetHighlited", "Alphabet Highlited");
        InitialisedReordableList(ref AphabetWrongList, "AlphabetWrong", "Alphabet Wrong");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        AphabetPlainList.DoLayoutList();
        AphabetNormalList.DoLayoutList();
        AphabetHightList.DoLayoutList();
        AphabetWrongList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void InitialisedReordableList(ref ReorderableList list, string propertyName, string listLable)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName), true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLable);
        };

        var l = list;

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("letter"), GUIContent.none);

            EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("image"), GUIContent.none);
        };
    }
}
