using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityNormalMapEditor.Editor.NormalMapEditorGuiConstants;

namespace UnityNormalMapEditor.Editor 
{
    public class NormalMapEditorWindow : EditorWindow
    {
        private static readonly Vector2 NormalMapEditorWindowSize = new Vector2(350, 800);

        private static VisualElement _root;


        [MenuItem(NormalMapEditorMenuItemPath)]
        public static void ShowWindow()
        {
            var window = GetWindow<NormalMapEditorWindow>();
            window.titleContent = new GUIContent(NormalMapEditorName);
            window.minSize = NormalMapEditorWindowSize;
        }

        private void OnEnable()
        {
            _root = rootVisualElement;

            #region INITIALIZATION AND QUERY
            // Root
            var rootVisualTree = Resources.Load<VisualTreeAsset>(RootUxmlPath);
            rootVisualTree.CloneTree(_root);
            
            #endregion
        }

    }
}