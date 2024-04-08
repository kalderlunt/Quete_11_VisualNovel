using DIALOGUE;
using System.Collections;
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

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                DestroyImmediate(gameObject);
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

            return result;
        }

        private Character CreateCharacterFromInfo(CHARACTER_INFO info)
        {
            CharacterConfigData config = info.config;

            if (config.characterType == Character.CharacterType.Text)
                return new Character_Text(info.name);
            
            if (config.characterType == Character.CharacterType.Sprite || config.characterType == Character.CharacterType.SpriteSheet)
                return new Character_Sprite(info.name);

            if (config.characterType == Character.CharacterType.Live2D)
                return new Character_Live2D(info.name);
            
            if (config.characterType == Character.CharacterType.Model3D)
                return new Character_Model3D(info.name);

            return null;
        }

        public class CHARACTER_INFO
        {
            public string name = "";

            public CharacterConfigData config = null;
        }
    }
}