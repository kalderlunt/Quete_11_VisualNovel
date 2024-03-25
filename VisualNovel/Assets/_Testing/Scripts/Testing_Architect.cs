using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class Testing_Architect : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitect architect;

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
            architect.buildMethod = TextArchitect.BuildMethod.typewriter;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                architect.Build(lines[Random.Range(0, lines.Length)]);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                architect.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}