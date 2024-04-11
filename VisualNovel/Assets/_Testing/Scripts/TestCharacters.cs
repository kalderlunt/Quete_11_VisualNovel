using CHARACTERS;
using System.Collections;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.Rendering.LookDev;
using System.Reflection;

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
            /*
            Sprite bodySprite= Raelin.GetSprite("3");
            Sprite faceSprite= Raelin.GetSprite("Laugh 3");

            Raelin.SetSprite(bodySprite, 0);
            Raelin.SetSprite(faceSprite, 1);
            
            Guard.SetPosition(Vector2.zero);
            Raelin.SetPosition(new Vector2(0.5f, 0.5f));
            Stella.SetPosition(Vector2.one);
            Student.SetPosition(new Vector2(2,1));

            Raelin.Show();
            Stella.Show();

            yield return Guard.Show();
            yield return Guard.MoveToPosition(Vector2.one, smooth: true);
            yield return Guard.MoveToPosition(Vector2.zero, smooth: true);

            Guard.SetDialogueFont(_fontAsset);
            Guard.SetNameFont(_fontAsset);
            Raelin.SetDialogueColor(Color.cyan);
            Stella.SetNameColor(Color.red);

            yield return Guard.Say("I want to say something important.");
            yield return Raelin.Say("Hold your peace.");
            yield return Stella.Say("Let him speak...");
*/

            Character_Sprite Guard     = CreateCharacter("Guard as Generic")    as Character_Sprite;
            Character_Sprite Raelin      = CreateCharacter("Raelin")    as Character_Sprite;
            //Character_Sprite Stella    = CreateCharacter("Stella")              as Character_Sprite;
            //Stella.isVisible = false;
            //Character_Sprite Student   = CreateCharacter("Female Student 2")    as Character_Sprite;

            Guard.SetPosition(Vector2.zero);
            Raelin.SetPosition(new Vector2(1, 0));
            
            Raelin.UnHighlight();
            yield return Guard.Say(" I want to say something");

            Guard.UnHighlight();
            Raelin.Highlight();
            yield return Raelin.Say("But I want to say something too!{c}Can I go first?");

            Guard.Highlight();
            Raelin.UnHighlight();
            yield return Guard.Say("Sure,{a} be my guest.");
            
            Raelin.Highlight();
            Guard.UnHighlight();
            Raelin.TransitionSprite(Raelin.GetSprite("B1"));
            Raelin.TransitionSprite(Raelin.GetSprite("B_Happy"), layer: 1);
            yield return Raelin.Say("Yay!");
            
            yield return null;

            /*
            yield return new WaitForSeconds(1);

            yield return Raelin.UnHighlight();

            yield return new WaitForSeconds(1);

            yield return Raelin.TransitionColor(Color.red);

            yield return new WaitForSeconds(1);

            yield return Raelin.Highlight();

            yield return new WaitForSeconds(1);

            yield return Raelin.TransitionColor(Color.white);
            */
            /*
            //Raelin.SetColor(Color.red);
            //Raelin.layers[1].SetColor(Color.red);
            yield return Raelin.TransitionColor(Color.red);
            yield return Raelin.TransitionColor(Color.blue);
            yield return Raelin.TransitionColor(Color.yellow);
            yield return Raelin.TransitionColor(Color.white);
            */
            /*
            Sprite face = Raelin.GetSprite("B_Laugh");
            Sprite body = Raelin.GetSprite("B2");
            Raelin.TransitionSprite(body);
            yield return Raelin.TransitionSprite(face, 1, 0.3f);
            
            Raelin.MoveToPosition(Vector2.zero);
            Stella.Show();
            yield return Stella.MoveToPosition(new Vector2(1, 0));

            yield return Raelin.TransitionSprite(Raelin.GetSprite("A_Scold"), 1);
            //Raelin.TransitionSprite(Raelin.GetSprite("A_Scold"), 1);
            Raelin.TransitionSprite(Raelin.GetSprite("A2"));
            */
            /*
            body = Stella.GetSprite("3");
            face = Stella.GetSprite("sad 3");
            Stella.TransitionSprite(body);
            Stella.TransitionSprite(face, 1);
            */
            /*
            Raelin.TransitionSprite(Raelin.GetSprite("B_Scold"));
            Sprite s1 = Guard.GetSprite("Characters-Monk");
            Guard.TransitionSprite(s1);

            Debug.Log($"Visible = {Guard.isVisible}");
            */
        }

        void Update()
        {

        }
    }
}