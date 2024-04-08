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
                //Debug.Log("Input keyboard detected.");
                PromptAdvance();
            }
        }

        public void PromptAdvance()
        {
            DialogueSystem.instance.OnUserPrompt_Next();
        }
    }
}