using DIALOGUE;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{

    /// <summary>
    /// Gestionnaire de personnages
    /// </summary>
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager instance { get; private set; }
        private Dictionary<string, Character> _characters = new();

        private CharacterConfigSO _config => DialogueSystem.instance.config.characterConfigurationAsset;

        private const string CHARACTER_NAME_ID = "<charname>";
        private string _characterRootPath => $"Characters/{CHARACTER_NAME_ID}";
        private string _characterPrefabPath => $"{_characterRootPath}/Character - [{CHARACTER_NAME_ID}]";

        [SerializeField] private RectTransform _characterPanel = null;
        public RectTransform characterPanel => _characterPanel;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                DestroyImmediate(gameObject);
        }
        
        public CharacterConfigData GetCharacterConfig(string characterName)
        {
            return _config.GetConfig(characterName);
        }

        public Character GetCharacter(string characterName, bool createIfDoesNotExist = false)
        {
            if (_characters.ContainsKey(characterName.ToLower()))
                return _characters[characterName.ToLower()];
            else if (createIfDoesNotExist)
                return CreateCharacter(characterName);

            return null;
        }

        public Character CreateCharacter(string characterName)
        {
            if (_characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogWarning($"A Character called '{characterName}' already exists. Did not create the character.");
                return null;
            }

            CHARACTER_INFO info = GetCharacterInfo(characterName);

            Character character = CreateCharacterFromInfo(info);

            _characters.Add(characterName.ToLower(), character);

            return character;
        }

        private CHARACTER_INFO GetCharacterInfo(string characterName)
        {
            CHARACTER_INFO result = new();

            result.name = characterName;

            result.config = _config.GetConfig(characterName);

            result.prefab = GetPrefabForCharacter(characterName);

            return result;
        }

        private GameObject GetPrefabForCharacter(string characterName)
        {
            string prefabPath = FormatCharacterPath(_characterPrefabPath, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        private string FormatCharacterPath(string path, string characterName) => path.Replace(CHARACTER_NAME_ID, characterName);

        private Character CreateCharacterFromInfo(CHARACTER_INFO info)
        {
            CharacterConfigData config = info.config;

            if (config.characterType == Character.CharacterType.Text)
                return new Character_Text(info.name, config);
            
            if (config.characterType == Character.CharacterType.Sprite || config.characterType == Character.CharacterType.SpriteSheet)
                return new Character_Sprite(info.name, config, info.prefab);

            if (config.characterType == Character.CharacterType.Live2D)
                return new Character_Live2D(info.name, config, info.prefab);
            
            if (config.characterType == Character.CharacterType.Model3D)
                return new Character_Model3D(info.name, config, info.prefab);

            return null;
        }

        public class CHARACTER_INFO
        {
            public string name = "";

            public CharacterConfigData config = null;

            public GameObject prefab = null;
        }
    }
}