﻿using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects, CustomEditor (typeof (RaycastCollider), false)]
public class RaycastColliderEditor : GraphicEditor {

    public override void OnInspectorGUI () {
        base.serializedObject.Update ();
        EditorGUILayout.PropertyField (base.m_Script, new GUILayoutOption[0]);
        base.RaycastControlsGUI ();
        base.serializedObject.ApplyModifiedProperties ();
    }
}
