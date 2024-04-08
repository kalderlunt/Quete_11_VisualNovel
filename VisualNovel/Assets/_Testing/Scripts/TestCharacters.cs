using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {

        [SerializeField] private string _characterName = "Kaito";

        void Start()
        {
            Character Stella    = CharacterManager.instance.CreateCharacter("Stella");
            Character Stella2   = CharacterManager.instance.CreateCharacter("Stella");
            Character Elen      = CharacterManager.instance.CreateCharacter("Elen");
            Character Adamn     = CharacterManager.instance.CreateCharacter("Adam");
        }

        void Update()
        {

        }
    }
}