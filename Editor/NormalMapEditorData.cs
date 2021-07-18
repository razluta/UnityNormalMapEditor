using System;
using UnityEditor;
using UnityEngine;

namespace UnityNormalMapEditor.Editor
{
    [System.Serializable]
    public class NormalMapEditorData
    {
        private bool _isBatchDirectory;
        private string _batchDirectoryPath;
        private Texture2D _singleTexture;
        private string _singleTexturePath;
        private bool _isOverwriteOriginal;
        private string _newName;
        private bool _isChangePath;
        private string _newAssetPath;

        private const bool IsBatchDirectoryDefault = false;
        private const string BatchDirectoryPathDefault = "";
        private const Texture2D SingleTextureDefault = null;
        private const string SingleTexturePathDefault = "";
        private const bool IsOverwriteOriginalDefault = true;
        private const string NewNameDefault = "<name>";
        private const bool IsChangePathDefault = false;
        private const string NewAssetPathDefault = "";

        public bool IsBatchDirectory
        {
            get => _isBatchDirectory;
            set => _isBatchDirectory = value;
        }

        public string BatchDirectoryPath
        {
            get => _batchDirectoryPath;
            set => _batchDirectoryPath = value;
        }

        public Texture2D SingleTexture
        {
            get => _singleTexture;
            set => _singleTexture = value;
        }

        public string SingleTexturePath
        {
            get => _singleTexturePath;
            set => _singleTexturePath = value;
        }

        public bool IsOverwriteOriginal
        {
            get => _isOverwriteOriginal;
            set => _isOverwriteOriginal = value;
        }

        public string NewName
        {
            get => _newName;
            set => _newName = value;
        }

        public bool IsChangePath
        {
            get => _isChangePath;
            set => _isChangePath = value;
        }

        public string NewAssetPath
        {
            get => _newAssetPath;
            set => _newAssetPath = value;
        }

        public NormalMapEditorData()
        {
            IsBatchDirectory = IsBatchDirectoryDefault;
            BatchDirectoryPath = BatchDirectoryPathDefault;
            SingleTexture = SingleTextureDefault;
            SingleTexturePath = SingleTexturePathDefault;
            IsOverwriteOriginal = IsOverwriteOriginalDefault;
            NewName = NewNameDefault;
            IsChangePath = IsChangePathDefault;
            NewAssetPath = NewAssetPathDefault;
        }

        public void UpdateLoadedTexture(string texturePath)
        {
            SingleTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        }
    }
}