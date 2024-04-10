using CHARACTERS;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {

        //[SerializeField] private string _characterName = "Kaito";
        
        [SerializeField] private TMP_FontAsset _fontAsset;

        private Character CreateCharacter(string name) =>  CharacterManager.instance.CreateCharacter(name);

        void Start()
        {
            /*
            Character Stella    = CharacterManager.instance.CreateCharacter("Female Student 2");
            Character Stella2   = CharacterManager.instance.CreateCharacter("Raelin");
            Character Adam     = CharacterManager.instance.CreateCharacter("Adam");
            Character Generic     = CharacterManager.instance.CreateCharacter("Generic");
            Character Stella = CharacterManager.instance.CreateCharacter("Stella");
            */

            StartCoroutine(Test());
        }


        IEnumerator Test()
        {
            /*
            Character Elen      = CharacterManager.instance.CreateCharacter("Elen");
            Character Adam      = CharacterManager.instance.CreateCharacter("Adam");
            Character Ben     = CharacterManager.instance.CreateCharacter("Benjamin");

            List<string> lines = new()
            {
                "Hi there!",
                "My name is Elen",
                "What's your name ?",
                "Oh,{wa 1} that's very nice."
            };
            yield return Elen.Say(lines);
            
            Elen.SetNameColor(Color.red);
            Elen.SetDialogueColor(Color.green);
            Elen.SetNameFont(_fontAsset);
            Elen.SetDialogueFont(_fontAsset);
            
            yield return Elen.Say(lines);
            
            Elen.ResetConfigurationData();
            
            yield return Elen.Say(lines);

            lines = new()
            {
                "I am Adam.",
                "More lines{c}Here."
            };
            yield return Adam.Say(lines);
            
            yield return Ben.Say("This is a line that I want to say.{a} It's a simple line.");

            Debug.Log("Finished");*/
            /*            
            yield return new WaitForSeconds(1f);

            Character Raelin   = CharacterManager.instance.CreateCharacter("Raelin");

            yield return new WaitForSeconds(1f);

            yield return Raelin.Hide();

            yield return new WaitForSeconds(0.5f);

            yield return Raelin.Show();

            yield return Raelin.Say("Hello !");*/

            Character guard1 = CreateCharacter("Guard1 as Generic");
            Character guard2 = CreateCharacter("Guard2 as Generic");
            Character guard3 = CreateCharacter("Guard3 as Generic");

            guard1.Show();
            guard2.Show();
            guard3.Show();

            guard1.SetDialogueFont(_fontAsset);
            guard1.SetNameFont(_fontAsset);
            guard2.SetDialogueColor(Color.cyan);
            guard3.SetNameColor(Color.red);

            yield return guard1.Say("I want to say something important.");
            yield return guard2.Say("Hold your peace.");
            yield return guard3.Say("Let him speak...");


            yield return null;
        }

        void Update()
        {

        }
    }
}