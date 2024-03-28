namespace DIALOGUE
{
    public class DIALOGUE_LINE
    {
        public DL_SPEAKER_DATA speaker;
        public DL_DIALOGUE_DATA dialogue;
        public string commands;

        public bool hasSpeaker => speaker != null; //speaker != string.Empty;
        public bool hasDialogue => dialogue.hasDialogue;
        public bool hasCommands => commands != string.Empty;

        public DIALOGUE_LINE(string speaker, string dialogue, string commands)
        {
            this.speaker = (string.IsNullOrWhiteSpace(speaker) ? null : new(speaker));
            this.dialogue = new(dialogue);
            this.commands = commands;
        }
    }
}