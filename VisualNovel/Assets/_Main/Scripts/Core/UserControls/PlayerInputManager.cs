using UnityEngine;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        void Start()
        {
        
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Input keyboard detected.\n\n\n\n\n\n");
                PromptAdvance();
            }
        }

        public void PromptAdvance()
        {
            DialogueSystem.instance.OnUserPrompt_Next();
        }
    }
}