using DIALOGUE;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class TestParsing : MonoBehaviour
    {
        //[SerializeField] private TextAsset _file;

        void Start()
        {
        /*string line = "Speaker \"Dialogue \\\"Goes In\\\" Here!\" Command(arguments here)";
            DialogueParser.Parse(line);*/

            //List<string> lines = FileManager.ReadTextAsset(_file);

            SendFileToParse();
        }

        void SendFileToParse()
        {
            List<string> lines = FileManager.ReadTextAsset("testFile");
            
            foreach (string line in lines)
            {
                if (line == string.Empty) 
                    continue;
                DIALOGUE_LINE dl = DialogueParser.Parse(line);
            }
        }
    }
}