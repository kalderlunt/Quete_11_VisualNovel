using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System.Collections.Specialized;

public class CommandManager : MonoBehaviour
{
    public static CommandManager instance { get; private set; }
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

    public void Execute(string commandName)
    {
        Delegate command = _database.GetCommand(commandName);

        if(command != null)
            command.DynamicInvoke();
    }
}