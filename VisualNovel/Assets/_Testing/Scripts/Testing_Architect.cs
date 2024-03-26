using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class Testing_Architect : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitect architect;

        public TextArchitect.BuildMethod bm = TextArchitect.BuildMethod.instant;

        [SerializeField] string[] lines = new string[5]
        {
            "This is a random line of dialogue.",
            "I want to say something, come over here." ,
            "The world is a crazy place sometines.",
            "Don't lose hope, things will get better!" ,
            "It's a bird? It's a plane? No! — It's Super Sheltie!"
        };

        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new(ds._dialogueContainer.dialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.fade;
            architect.speed = 0.5f;
        }

        void Update()
        {
            if (bm != architect.buildMethod)
            {
                architect.buildMethod = bm;
                architect.Stop();
            }
            if (Input.GetKeyDown(KeyCode.S))
                architect.Stop();

            string longLine = "This is a very long line that makes no sense but i am just populating it with stuff because, you know, stuff is good right? I like stuff, you like stuff, we all like stuff and the turkey g gets stuffed.";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (architect.isBuilding)
                {
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    else
                        architect.ForceComplete();
                }
                else
                    //architect.Build(lines[Random.Range(0, lines.Length)]);
                    architect.Build(longLine);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                architect.Append(longLine);
                //architect.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}