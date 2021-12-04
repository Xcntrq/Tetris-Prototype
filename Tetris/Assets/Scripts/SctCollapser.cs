using UnityEditor;

public class SctCollapser
{
    [MenuItem("GameObject/Collapse Components _1")]
    static void CollapseComponents()
    {
        ActiveEditorTracker tracker = ActiveEditorTracker.sharedTracker;
        for (int i = 0, length = tracker.activeEditors.Length; i < length; i++) tracker.SetVisible(i, 0);
        System.Type windowType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
        EditorWindow.GetWindow(windowType).Repaint();
    }
}
