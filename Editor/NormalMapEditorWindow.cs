using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityNormalMapEditor.Editor.NormalMapEditorGuiConstants;

namespace UnityNormalMapEditor.Editor 
{
    public class NormalMapEditorWindow : EditorWindow
    {
        private static readonly Vector2 NormalMapEditorWindowSize = new Vector2(350, 850);

        private static VisualElement _root;
        private NormalMapEditorData _data;

        private Button _normalMapEditorButton;
        private Toggle _batchDirectoryToggle;
        private Button _browseBatchDirectoryButton;
        private VisualElement _singleTexturePanelVisualElement;
        private VisualElement _textureVisualElement;
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
        private Button _rotate180Button;
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
            _textureVisualElement = _root.Q<VisualElement>(TextureVisualElementName);
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
            _rotate180Button = _root.Q<Button>(Rotate180ButtonName);
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

            _invertRedButton.clickable.clicked += InvertRedChannel;
            
            _invertGreenButton.clickable.clicked += InvertGreenChannel;
            
            _invertBlueButton.clickable.clicked += InvertBlueChannel;

            _rotateClockwiseButton.clickable.clicked += Rotate90DegreesClockwise;

            _rotateCounterclockwiseButton.clickable.clicked += Rotate90DegreesCounterclockwise;

            _rotate180Button.clickable.clicked += Rotate180;
            
            _flipHorizontalButton.clickable.clicked += FlipHorizontal;
            
            _flipVerticalButton.clickable.clicked += FlipVertical;
            #endregion
        }

        private void BrowseBatchDirectory()
        {
            _data.BatchDirectoryPath = EditorUtility.OpenFolderPanel(BrowseLabel, "", "");
        }

        private void LoadSingleTexture()
        {
            _data.SingleTexture = null;
            
            var texturePath = EditorUtility.OpenFilePanel(BrowseLabel, "", "");
            if (!texturePath.Contains(Application.dataPath))
            {
                Debug.LogError("Selected texture must be in the project.");
                return;
            }

            var internalTexturePath = texturePath.Replace(Application.dataPath, "Assets");
            _data.SingleTexturePath = internalTexturePath;
            _data.SingleTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(internalTexturePath);
            if (_data.SingleTexture == null)
            {
                Debug.LogError("Selected asset cannot be loaded as a texture.");
                return;
            }
            
            UpdateUiContentsFromData();
        }


        private void UpdateUiContentsFromData()
        {
            _data.UpdateLoadedTexture(_data.SingleTexturePath);
            _batchDirectoryToggle.value = _data.IsBatchDirectory;
            _browseBatchDirectoryButton.SetEnabled(_batchDirectoryToggle.value);
            _singleTexturePanelVisualElement.SetEnabled(!_batchDirectoryToggle.value);
            _singleTextureParamsVisualElement.SetEnabled(!_batchDirectoryToggle.value);

            if (_data.SingleTexture != null)
            {
                _textureButton.text = string.Empty;
                _textureVisualElement.style.backgroundImage = new StyleBackground(_data.SingleTexture);
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

        private void InvertRedChannel()
        {
            // Single Texture
            InvertColorChannel(new []{_data.SingleTexture}, TextureUtilities.TextureChannel.Red);
            UpdateUiContentsFromData();
        }

        private void InvertGreenChannel()
        {
            // Single Texture
            InvertColorChannel(new []{_data.SingleTexture}, TextureUtilities.TextureChannel.Green);
            UpdateUiContentsFromData();
        }

        private void InvertBlueChannel()
        {
            // Single Texture
            InvertColorChannel(new []{_data.SingleTexture}, TextureUtilities.TextureChannel.Blue);
            UpdateUiContentsFromData();
        }

        private void InvertColorChannel(Texture2D[] textures, TextureUtilities.TextureChannel textureChannel)
        {
            foreach (var texture in textures)
            {
                if (texture == null) continue;
                var invertedTexture = TextureUtilities.GetTextureWithInvertedChannel(texture, textureChannel);
                TextureUtilities.SaveTextureToPath(invertedTexture, AssetDatabase.GetAssetPath(texture));
            }
        }

        private void Rotate90DegreesClockwise()
        {
            // Single Texture
            var newTexture = TextureUtilities.RotateNormalClockwise(_data.SingleTexture);
            TextureUtilities.SaveTextureToPath(newTexture, AssetDatabase.GetAssetPath(_data.SingleTexture));
            UpdateUiContentsFromData();
        }

        private void Rotate90DegreesCounterclockwise()
        {
            // Single Texture
            var newTexture = TextureUtilities.RotateNormalCounterclockwise(_data.SingleTexture);
            TextureUtilities.SaveTextureToPath(newTexture, AssetDatabase.GetAssetPath(_data.SingleTexture));
            UpdateUiContentsFromData();
        }

        private void Rotate180()
        {
            // Single Texture
            var newTexture = TextureUtilities.RotateNormal180(_data.SingleTexture);
            TextureUtilities.SaveTextureToPath(newTexture, AssetDatabase.GetAssetPath(_data.SingleTexture));
            UpdateUiContentsFromData();
        }

        private void FlipHorizontal()
        {
            // Single Texture
            var newTexture = TextureUtilities.FlipNormalHorizontal(_data.SingleTexture);
            TextureUtilities.SaveTextureToPath(newTexture, AssetDatabase.GetAssetPath(_data.SingleTexture));
            UpdateUiContentsFromData();
        }

        private void FlipVertical()
        {
            // Single Texture
            var newTexture = TextureUtilities.FlipNormalVertical(_data.SingleTexture);
            TextureUtilities.SaveTextureToPath(newTexture, AssetDatabase.GetAssetPath(_data.SingleTexture));
            UpdateUiContentsFromData();
        }
    }
}