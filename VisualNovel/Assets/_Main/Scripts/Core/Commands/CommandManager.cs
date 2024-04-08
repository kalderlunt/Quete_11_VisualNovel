using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace COMMANDS
{
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager instance { get; private set; }
        private static Coroutine process = null;
        public static bool isRunning => process != null;

        private CommandDatabase _database;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                _database = new();

                Assembly assembly = Assembly.GetExecutingAssembly();
                Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

                foreach (Type extension in extensionTypes)
                {
                    MethodInfo extendMethod = extension.GetMethod("Extend");
                    extendMethod.Invoke(null, new object[] { _database });
                }
            }
            else
                DestroyImmediate(gameObject);
        }

        public Coroutine Execute(string commandName, params string[] args)
        {
            Delegate command = _database.GetCommand(commandName);

            if (command == null)
                return null;

            return StartProcess(commandName, command, args);
        }

        private Coroutine StartProcess(string commandName, Delegate command, params string[] args)
        {
            StopCurrentProcess();

            process = StartCoroutine(RunnningProcess(command, args));

            return process;
        }

        private void StopCurrentProcess()
        {
            if (process != null)
                StopCoroutine(process);

            process = null;
        }

        private IEnumerator RunnningProcess(Delegate command, string[] args)
        {
            yield return WaitingForProcessToComplete(command, args);
            process = null;
        }

        private IEnumerator WaitingForProcessToComplete(Delegate command, string[] args)
        {
            if (command is Action)
                command.DynamicInvoke();

            else if (command is Action<string>)
                command.DynamicInvoke(args[0]);

            else if (command is Action<string[]>)
                command.DynamicInvoke((object)args);

            else if (command is Func<IEnumerator>)
                yield return ((Func<IEnumerator>)command)();

            else if (command is Func<string, IEnumerator>)
                yield return ((Func<string, IEnumerator>)command)(args[0]);

            else if (command is Func<string[], IEnumerator>)
                yield return ((Func<string[], IEnumerator>)command)(args);
        }
    }
}