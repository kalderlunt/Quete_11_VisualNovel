using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_Model3D : Character
    {
        public Character_Model3D(string name) : base(name)
        {
            Debug.Log($"Create Model#d Character: '{name}'");
        }
    }
}