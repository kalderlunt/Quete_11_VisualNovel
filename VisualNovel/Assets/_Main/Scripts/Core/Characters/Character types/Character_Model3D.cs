using UnityEngine;

namespace CHARACTERS
{
    public class Character_Model3D : Character
    {
        public Character_Model3D(string name, CharacterConfigData config, GameObject prefab) : base(name, config, prefab)
        {
            Debug.Log($"Create Model#d Character: '{name}'");
        }
    }
}