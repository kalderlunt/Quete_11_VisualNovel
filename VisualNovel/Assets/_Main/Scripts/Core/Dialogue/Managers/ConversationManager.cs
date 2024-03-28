using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager : MonoBehaviour
    {
        private DialogueSystem _dialogueSystem => DialogueSystem.instance;
        private Coroutine _process = null;
        public bool isRunning => _process != null;

        private TextArchitect _architect = null;
        private bool _userPrompt = false;


        public ConversationManager(TextArchitect architect) 
        {
            this._architect = architect;
            _dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            _userPrompt = true;
        }

        public void StartConversation(List<string> conversation)
        {
            StopConversation();

            _process = _dialogueSystem.StartCoroutine(RunningConversation(conversation));
        }

        public void StopConversation()
        {
            if (!isRunning)
                return;

            _dialogueSystem.StopCoroutine(_process);
            _process = null;
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                // Don't show any blank lines or try to run any logic on them.
                if (string.IsNullOrWhiteSpace(conversation[i])) // conversation[i] == string.Empty
                    continue;
                    
                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

                // Show dialogue
                if (line.hasDialogue)
                    yield return Line_RunDialogue(line);

                // Run any commands
                if (line.hasCommands)
                    yield return Line_RunCommands(line);

                //yield return new WaitForSeconds(1);
            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            // Show or hide the speaker name if there is one present
            if (line.hasSpeaker)
                _dialogueSystem.ShowSpeakerName(line.speaker);
            /*else
                _dialogueSystem.HideSpeakerName();*/

            // Build dialogue
            yield return BuildLineSegments(line.dialogue);

            // Wait for user input
            yield return WaitForUserInput();
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            Debug.Log(line.commands);
            yield return null;
        }

        IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }

        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch(segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                
                default
                    : break;
            }
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            // Build the dialogue
            if (!append)
                _architect.Build(dialogue);
            else
                _architect.Append(dialogue);

            // Wait for the dialogue to complete
            while (_architect.isBuilding)
            {
                if (_userPrompt)
                {
                    if (!_architect.hurryUp)
                        _architect.hurryUp = true;
                    else
                        _architect.ForceComplete();

                    _userPrompt = false;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            while (!_userPrompt)
                yield return null;

            _userPrompt = false;
        }
    }
}