using System;
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
        private NormalMapEditorData _data;

        private Button _normalMapEditorButton;
        private Toggle _batchDirectoryToggle;
        private Button _browseBatchDirectoryButton;
        private VisualElement _singleTexturePanelVisualElement;
        private VisualElement _textureBackgroundVisualElement;
        private Button _textureButton;
        private Label _textureNameLabelName;
        private VisualElement _singleTextureParamsVisualElement;
        private Toggle _overwriteOriginalToggle;
        private TextField _newNameTextField;
        private Toggle _changeSavePathToggle;
        private Button _browseSingleTextureButton;
        private Button _invertRedButton;
        private Button _invertGreenButton;
        private Button _invertBlueButton;
        private Button _rotateClockwiseButton;
        private Button _rotateCounterclockwiseButton;
        private Button _flipHorizontalButton;
        private Button _flipVerticalButton;

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
            _data = new NormalMapEditorData();

            #region INITIALIZATION AND QUERY
            var rootVisualTree = Resources.Load<VisualTreeAsset>(RootUxmlPath);
            rootVisualTree.CloneTree(_root);
            
            _normalMapEditorButton = _root.Q<Button>(NormalMapEditorButtonName);
            _batchDirectoryToggle = _root.Q<Toggle>(BatchDirectoryToggleName);
            _browseBatchDirectoryButton = _root.Q<Button>(BrowseBatchDirectoryButtonName);
            _singleTexturePanelVisualElement = _root.Q<VisualElement>(SingleTexturePanelVisualElementName);
            _textureBackgroundVisualElement = _root.Q<VisualElement>(TextureBackgroundVisualElementName);
            _textureButton = _root.Q<Button>(TextureButtonName);
            _textureNameLabelName = _root.Q<Label>(TextureNameLabelName);
            _singleTextureParamsVisualElement = _root.Q<VisualElement>(SingleTextureParamsVisualElementName);
            _overwriteOriginalToggle = _root.Q<Toggle>(OverwriteOriginalToggleName);
            _newNameTextField = _root.Q<TextField>(NewNameTextFieldName);
            _changeSavePathToggle = _root.Q<Toggle>(ChangeSavePathToggleName);
            _browseSingleTextureButton = _root.Q<Button>(BrowseSingleTexturePathButtonName);
            _invertRedButton = _root.Q<Button>(InvertRedButtonName);
            _invertGreenButton = _root.Q<Button>(InvertGreenButtonName);
            _invertBlueButton= _root.Q<Button>(InvertBlueButtonName);
            _rotateClockwiseButton = _root.Q<Button>(RotateClockwiseButtonName);
            _rotateCounterclockwiseButton = _root.Q<Button>(RotateCounterclockwiseButtonName);
            _flipHorizontalButton = _root.Q<Button>(FlipHorizontalButtonName);
            _flipVerticalButton = _root.Q<Button>(FlipVerticalButtonName);
            UpdateUiContentsFromData();
            #endregion

            #region BEHAVIOR
            _normalMapEditorButton.clickable.clicked += () => Application.OpenURL(NormalMapEditorToolsUrl);
            _batchDirectoryToggle.RegisterValueChangedCallback(evt =>
            {
                _data.IsBatchDirectory = _batchDirectoryToggle.value;
                UpdateUiContentsFromData();
            });

            _browseBatchDirectoryButton.clickable.clicked += BrowseBatchDirectory;
            _textureButton.clickable.clicked += LoadSingleTexture;
            _overwriteOriginalToggle.RegisterValueChangedCallback(evt =>
            {
                _data.IsOverwriteOriginal = _overwriteOriginalToggle.value;
                UpdateUiContentsFromData();
            });
            _newNameTextField.RegisterValueChangedCallback(evt =>
            {
                _data.NewName = _newNameTextField.value;
                UpdateUiContentsFromData();
            });
            _changeSavePathToggle.RegisterValueChangedCallback(evt =>
            {
                _data.IsChangePath = _changeSavePathToggle.value;
                UpdateUiContentsFromData();
            });
            

            #endregion
        }

        private void BrowseBatchDirectory()
        {
            _data.BatchDirectoryPath = EditorUtility.OpenFolderPanel(BrowseLabel, "", "");
        }

        private void LoadSingleTexture()
        {
            var texturePath = EditorUtility.OpenFilePanel(BrowseLabel, "", "");
            if (!texturePath.Contains(Application.dataPath))
            {
                Debug.LogError("Selected texture must be in the project.");
                return;
            }

            var internalTexturePath = texturePath.Replace(Application.dataPath, "Assets");
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(internalTexturePath);
            if (texture == null)
            {
                Debug.LogError("Selected asset cannot be loaded as a texture.");
                return;
            }

            _data.SingleTexture = texture;
            UpdateUiContentsFromData();
        }


        private void UpdateUiContentsFromData()
        {
            _batchDirectoryToggle.value = _data.IsBatchDirectory;
            _browseBatchDirectoryButton.SetEnabled(_batchDirectoryToggle.value);
            _singleTexturePanelVisualElement.SetEnabled(!_batchDirectoryToggle.value);
            _singleTextureParamsVisualElement.SetEnabled(!_batchDirectoryToggle.value);

            if (_data.SingleTexture != null)
            {
                _textureButton.text = string.Empty;
                _textureButton.style.backgroundImage = new StyleBackground(_data.SingleTexture);
                _textureNameLabelName.text = _data.SingleTexture.name;
            }
            
            _overwriteOriginalToggle.value = _data.IsOverwriteOriginal;
            _newNameTextField.SetEnabled(!_overwriteOriginalToggle.value);
            _changeSavePathToggle.SetEnabled(!_overwriteOriginalToggle.value);
            _browseSingleTextureButton.SetEnabled(!_overwriteOriginalToggle.value);
            
            _newNameTextField.value = _data.NewName;
            _changeSavePathToggle.value = _data.IsChangePath;
            _browseSingleTextureButton.SetEnabled(_changeSavePathToggle.value);
        }

    }
}