using nsSoundManager;
using UnityEditor;
using UnityEngine;

namespace nsSctSoundManagerEditor
{
    [CustomEditor(typeof(SctSoundManager))]
    public class SctSoundManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            int fixedDistance = 30;

            serializedObject.Update();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_isMusicEnabled"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_imageTogglerMusic"), GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_music").FindPropertyRelative("m_audioClip"), new GUIContent("Music"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_music").FindPropertyRelative("m_volume"), GUIContent.none, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_musicVolumeWhenPaused"), new GUIContent("Vol When Paused"));

            GUILayout.Space(fixedDistance);

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_isSoundEnabled"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_imageTogglerSound"), GUIContent.none);
            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_soundVolume"));

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioGameOver").FindPropertyRelative("m_audioClip"), new GUIContent("GameOver"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioGameOver").FindPropertyRelative("m_volume"), GUIContent.none, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioLevelUp").FindPropertyRelative("m_audioClip"), new GUIContent("LevelUp"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioLevelUp").FindPropertyRelative("m_volume"), GUIContent.none, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioRowClear").FindPropertyRelative("m_audioClip"), new GUIContent("RowClear"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioRowClear").FindPropertyRelative("m_volume"), GUIContent.none, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioShapeDrop").FindPropertyRelative("m_audioClip"), new GUIContent("ShapeDrop"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioShapeDrop").FindPropertyRelative("m_volume"), GUIContent.none, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioShapeHold").FindPropertyRelative("m_audioClip"), new GUIContent("ShapeHold"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioShapeHold").FindPropertyRelative("m_volume"), GUIContent.none, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioShapeMoveError").FindPropertyRelative("m_audioClip"), new GUIContent("ShapeMoveError"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioShapeMoveError").FindPropertyRelative("m_volume"), GUIContent.none, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioShapeMoveSuccess").FindPropertyRelative("m_audioClip"), new GUIContent("ShapeMoveSuccess"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_audioShapeMoveSuccess").FindPropertyRelative("m_volume"), GUIContent.none, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
