using DIALOGUE;
using System.Collections.Generic;
using System.Linq;
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

        private const string CHARACTER_CASTING_ID = " as ";
        private const string CHARACTER_NAME_ID = "<charname>";
        public string characterRootPathFormat => $"Characters/{CHARACTER_NAME_ID}";
        public string characterPrefabNameFormat => $"Character - [{CHARACTER_NAME_ID}]";
        public string Format => $"{characterRootPathFormat}/{characterPrefabNameFormat}";

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

            _characters.Add(info.name.ToLower(), character);

            return character;
        }

        private CHARACTER_INFO GetCharacterInfo(string characterName)
        {
            CHARACTER_INFO result = new();

            string[] nameData = characterName.Split(CHARACTER_CASTING_ID, System.StringSplitOptions.RemoveEmptyEntries);
            result.name = nameData[0];
            result.castingName = nameData.Length > 1 ? nameData[1] : result.name;

            result.config = _config.GetConfig(result.castingName);

            result.prefab = GetPrefabForCharacter(result.castingName);

            result.rootCharacterFolder = FormatCharacterPath(characterRootPathFormat, result.castingName);

            return result;
        }

        private GameObject GetPrefabForCharacter(string characterName)
        {
            string prefabPath = FormatCharacterPath(Format, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        public string FormatCharacterPath(string path, string characterName) => path.Replace(CHARACTER_NAME_ID, characterName);

        private Character CreateCharacterFromInfo(CHARACTER_INFO info)
        {
            CharacterConfigData config = info.config;

            if (config.characterType == Character.CharacterType.Text)
                return new Character_Text(info.name, config);
            
            if (config.characterType == Character.CharacterType.Sprite || config.characterType == Character.CharacterType.SpriteSheet)
                return new Character_Sprite(info.name, config, info.prefab, info.rootCharacterFolder);

            if (config.characterType == Character.CharacterType.Live2D)
                return new Character_Live2D(info.name, config, info.prefab, info.rootCharacterFolder);
            
            if (config.characterType == Character.CharacterType.Model3D)
                return new Character_Model3D(info.name, config, info.prefab, info.rootCharacterFolder);

            return null;
        }

        public void SortCharacters()
        {
            List<Character> activeCharacters = _characters.Values.Where(c => c.root.gameObject.activeInHierarchy && c.isVisible).ToList();
            List<Character> inactiveCharacters = _characters.Values.Except(activeCharacters).ToList();

            activeCharacters.Sort((a, b) => a.priority.CompareTo(b.priority));
            activeCharacters.Concat(inactiveCharacters);
            
            SortCharacters(activeCharacters);
        }

        public void SortCharacters(string[] characterNames)
        {
            List<Character> sortedCharacters = new();

            sortedCharacters = characterNames
                .Select(name => GetCharacter(name))
                .Where(character => character != null)
                .ToList();
            Debug.Log(sortedCharacters.Count);

            List<Character> remainingCharacters = _characters.Values
                .Except(sortedCharacters)
                .OrderBy(character => character.priority)
                .ToList();

            sortedCharacters.Reverse();

            int startingPriority = remainingCharacters.Count > 0 ? remainingCharacters.Max(c => c.priority) : 0;      // ne fonctionne pas trop a la fin je dois avoir 1, 2, 3, 4 et non 1001, 1002, 1003, 1004 (video 26 de la serie)
            for (int i = 0; i < sortedCharacters.Count; i++)
            {
                Character character = sortedCharacters[i];
                character.SetPriority(startingPriority + i + 1, autoSortCharactersOnUI:false);
            }

            List<Character> allCharacters = remainingCharacters.Concat(sortedCharacters).ToList();
            SortCharacters(allCharacters);
        }

        private void SortCharacters(List<Character> charactersSortingOrder)
        {
            int i = 0;
            foreach (Character character in charactersSortingOrder)
            {
                Debug.Log($"{character.name} priority is {character.priority}");
                character.root.SetSiblingIndex(i++);
            }
        }

        public class CHARACTER_INFO
        {
            public string name = "";
            public string castingName = "";

            public string rootCharacterFolder = "";

            public CharacterConfigData config = null;

            public GameObject prefab = null;
        }
    }
}