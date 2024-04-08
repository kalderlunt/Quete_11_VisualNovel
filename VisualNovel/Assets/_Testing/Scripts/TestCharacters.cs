using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {

        [SerializeField] private string _characterName = "Kaito";

        void Start()
        {
            /*
            Character Stella    = CharacterManager.instance.CreateCharacter("Stella");
            Character Stella2   = CharacterManager.instance.CreateCharacter("Stella");
            Character Adamn     = CharacterManager.instance.CreateCharacter("Adam");*/

            StartCoroutine(Test());
        }


        IEnumerator Test()
        {
            Character Elen      = CharacterManager.instance.CreateCharacter("Elen");
            Character Adam      = CharacterManager.instance.CreateCharacter("Adam");
            Character Sarah     = CharacterManager.instance.CreateCharacter("Sarah");

            List<string> lines = new()
            {
                "Hi there!",
                "My name is Elen",
                "What's your name ?",
                "Oh,{wa 1} that's very nice."
            };
            yield return Elen.Say(lines);
            
            lines = new()
            {
                "I am Adam.",
                "More lines{c}Here."
            };
            yield return Adam.Say(lines);
            
            yield return Sarah.Say("This is a line that I want to say.{a} It's a simple line.");

            Debug.Log("Finished");
        }

        void Update()
        {

        }
    }
}