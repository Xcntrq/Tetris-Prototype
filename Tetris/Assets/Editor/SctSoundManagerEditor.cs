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

            int spacing = 30;
            SctSoundManager sctSoundManager = (SctSoundManager)target;

            GUILayout.BeginHorizontal();
            sctSoundManager.IsMusicEnabled = EditorGUILayout.Toggle("ImageTogglerMusic", sctSoundManager.IsMusicEnabled);
            sctSoundManager.ImageTogglerMusic = (SctImageTogglerOnOff)EditorGUILayout.ObjectField(sctSoundManager.ImageTogglerMusic, typeof(SctImageTogglerOnOff), true);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.MusicClip = (AudioClip)EditorGUILayout.ObjectField("Music", sctSoundManager.MusicClip, typeof(AudioClip), true);
            sctSoundManager.MusicVolume = EditorGUILayout.FloatField(sctSoundManager.MusicVolume, GUILayout.Width(spacing));
            GUILayout.EndHorizontal();

            sctSoundManager.MusicVolumeWhenPaused = EditorGUILayout.FloatField("MusicVolumeWhenPaused", sctSoundManager.MusicVolumeWhenPaused);

            GUILayout.Space(spacing);

            GUILayout.BeginHorizontal();
            sctSoundManager.IsSoundEnabled = EditorGUILayout.Toggle("ImageTogglerSound", sctSoundManager.IsSoundEnabled);
            sctSoundManager.ImageTogglerSound = (SctImageTogglerOnOff)EditorGUILayout.ObjectField(sctSoundManager.ImageTogglerSound, typeof(SctImageTogglerOnOff), true);
            GUILayout.EndHorizontal();

            sctSoundManager.SoundVolume = EditorGUILayout.FloatField("SoundVolume", sctSoundManager.SoundVolume);

            GUILayout.BeginHorizontal();
            sctSoundManager.AudioGameOver.AudioClip = (AudioClip)EditorGUILayout.ObjectField("GameOver", sctSoundManager.AudioGameOver.AudioClip, typeof(AudioClip), true);
            sctSoundManager.AudioGameOver.Volume = EditorGUILayout.FloatField(sctSoundManager.AudioGameOver.Volume, GUILayout.Width(spacing));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.AudioLevelUp.AudioClip = (AudioClip)EditorGUILayout.ObjectField("LevelUp", sctSoundManager.AudioLevelUp.AudioClip, typeof(AudioClip), true);
            sctSoundManager.AudioLevelUp.Volume = EditorGUILayout.FloatField(sctSoundManager.AudioLevelUp.Volume, GUILayout.Width(spacing));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.AudioRowClear.AudioClip = (AudioClip)EditorGUILayout.ObjectField("RowClear", sctSoundManager.AudioRowClear.AudioClip, typeof(AudioClip), true);
            sctSoundManager.AudioRowClear.Volume = EditorGUILayout.FloatField(sctSoundManager.AudioRowClear.Volume, GUILayout.Width(spacing));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.AudioShapeDrop.AudioClip = (AudioClip)EditorGUILayout.ObjectField("ShapeDrop", sctSoundManager.AudioShapeDrop.AudioClip, typeof(AudioClip), true);
            sctSoundManager.AudioShapeDrop.Volume = EditorGUILayout.FloatField(sctSoundManager.AudioShapeDrop.Volume, GUILayout.Width(spacing));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.AudioShapeHold.AudioClip = (AudioClip)EditorGUILayout.ObjectField("ShapeHold", sctSoundManager.AudioShapeHold.AudioClip, typeof(AudioClip), true);
            sctSoundManager.AudioShapeHold.Volume = EditorGUILayout.FloatField(sctSoundManager.AudioShapeHold.Volume, GUILayout.Width(spacing));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.AudioShapeMoveError.AudioClip = (AudioClip)EditorGUILayout.ObjectField("ShapeMoveError", sctSoundManager.AudioShapeMoveError.AudioClip, typeof(AudioClip), true);
            sctSoundManager.AudioShapeMoveError.Volume = EditorGUILayout.FloatField(sctSoundManager.AudioShapeMoveError.Volume, GUILayout.Width(spacing));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            sctSoundManager.AudioShapeMoveSuccess.AudioClip = (AudioClip)EditorGUILayout.ObjectField("ShapeMoveSuccess", sctSoundManager.AudioShapeMoveSuccess.AudioClip, typeof(AudioClip), true);
            sctSoundManager.AudioShapeMoveSuccess.Volume = EditorGUILayout.FloatField(sctSoundManager.AudioShapeMoveSuccess.Volume, GUILayout.Width(spacing));
            GUILayout.EndHorizontal();
        }
    }
}
