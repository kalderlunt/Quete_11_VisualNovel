using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        public DialogueContainer _dialogueContainer = new();

    
        public static DialogueSystem instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                DestroyImmediate(gameObject);
        }

        void Start()
        {
        
        }

        void Update()
        {
        
        }
    }
}