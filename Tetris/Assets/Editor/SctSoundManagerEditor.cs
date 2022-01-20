using nsSoundManager;
using UnityEditor;
using UnityEngine;
using nsImageTogglerOnOff;

namespace nsSctSoundManagerEditor
{
    [CustomEditor(typeof(SctSoundManager))]
    public class SctSoundManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            int fixedDistance = 30;
            SctSoundManager sctSoundManager = (SctSoundManager)target;

            GUILayout.BeginHorizontal();
            sctSoundManager.m_isMusicEnabled = EditorGUILayout.Toggle("ImageTogglerMusic", sctSoundManager.m_isMusicEnabled);
            sctSoundManager.m_imageTogglerMusic = (SctImageTogglerOnOff)EditorGUILayout.ObjectField(sctSoundManager.m_imageTogglerMusic, typeof(SctImageTogglerOnOff), true);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.m_musicClip = (AudioClip)EditorGUILayout.ObjectField("Music", sctSoundManager.m_musicClip, typeof(AudioClip), true);
            sctSoundManager.m_musicVolume = EditorGUILayout.FloatField(sctSoundManager.m_musicVolume, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            sctSoundManager.m_musicVolumeWhenPaused = EditorGUILayout.FloatField("MusicVolumeWhenPaused", sctSoundManager.m_musicVolumeWhenPaused);

            GUILayout.Space(fixedDistance);

            GUILayout.BeginHorizontal();
            sctSoundManager.m_isSoundEnabled = EditorGUILayout.Toggle("ImageTogglerSound", sctSoundManager.m_isSoundEnabled);
            sctSoundManager.m_imageTogglerSound = (SctImageTogglerOnOff)EditorGUILayout.ObjectField(sctSoundManager.m_imageTogglerSound, typeof(SctImageTogglerOnOff), true);
            GUILayout.EndHorizontal();

            sctSoundManager.m_soundVolume = EditorGUILayout.FloatField("SoundVolume", sctSoundManager.m_soundVolume);

            GUILayout.BeginHorizontal();
            sctSoundManager.m_audioGameOverClip = (AudioClip)EditorGUILayout.ObjectField("GameOver", sctSoundManager.m_audioGameOverClip, typeof(AudioClip), true);
            sctSoundManager.m_audioGameOverVolume = EditorGUILayout.FloatField(sctSoundManager.m_audioGameOverVolume, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.m_audioLevelUpClip = (AudioClip)EditorGUILayout.ObjectField("LevelUp", sctSoundManager.m_audioLevelUpClip, typeof(AudioClip), true);
            sctSoundManager.m_audioLevelUpVolume = EditorGUILayout.FloatField(sctSoundManager.m_audioLevelUpVolume, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.m_audioRowClearClip = (AudioClip)EditorGUILayout.ObjectField("RowClear", sctSoundManager.m_audioRowClearClip, typeof(AudioClip), true);
            sctSoundManager.m_audioRowClearVolume = EditorGUILayout.FloatField(sctSoundManager.m_audioRowClearVolume, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.m_audioShapeDropClip = (AudioClip)EditorGUILayout.ObjectField("ShapeDrop", sctSoundManager.m_audioShapeDropClip, typeof(AudioClip), true);
            sctSoundManager.m_audioShapeDropVolume = EditorGUILayout.FloatField(sctSoundManager.m_audioShapeDropVolume, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.m_audioShapeHoldClip = (AudioClip)EditorGUILayout.ObjectField("ShapeHold", sctSoundManager.m_audioShapeHoldClip, typeof(AudioClip), true);
            sctSoundManager.m_audioShapeHoldVolume = EditorGUILayout.FloatField(sctSoundManager.m_audioShapeHoldVolume, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.m_audioShapeMoveErrorClip = (AudioClip)EditorGUILayout.ObjectField("ShapeMoveError", sctSoundManager.m_audioShapeMoveErrorClip, typeof(AudioClip), true);
            sctSoundManager.m_audioShapeMoveErrorVolume = EditorGUILayout.FloatField(sctSoundManager.m_audioShapeMoveErrorVolume, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.m_audioShapeMoveSuccessClip = (AudioClip)EditorGUILayout.ObjectField("ShapeMoveSuccess", sctSoundManager.m_audioShapeMoveSuccessClip, typeof(AudioClip), true);
            sctSoundManager.m_audioShapeMoveSuccessVolume = EditorGUILayout.FloatField(sctSoundManager.m_audioShapeMoveSuccessVolume, GUILayout.Width(fixedDistance));
            GUILayout.EndHorizontal();
        }
    }
}
