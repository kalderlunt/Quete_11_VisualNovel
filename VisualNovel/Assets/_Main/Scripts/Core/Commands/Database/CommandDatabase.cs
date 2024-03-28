using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDatabase
{
    private Dictionary<string, Delegate> _database = new(); 

    public bool HasCommand(string commandName) => _database.ContainsKey(commandName);

    public void ADDCommand(string commandName, Delegate commmand)
    {
        if (!_database.ContainsKey(commandName))
        {
            _database.Add(commandName, commmand);
        }
        else
            Debug.LogError($"Command already exists in the database '{commandName}'");
    }

    public Delegate GetCommand(string commandName)
    {
        if (!_database.ContainsKey(commandName))
        {
            Debug.LogError($"Command'{commandName}' does not exists in the database ");
            return null;
        }

        return _database[commandName];
    }
}