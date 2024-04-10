using System.Collections;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        private CanvasGroup _rootCG => root.GetComponent<CanvasGroup>();
        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab) : base(name, config, prefab) 
        {
            _rootCG.alpha = 0.0f;
            //Show();
            Debug.Log($"Created Sprite Character: '{name}'");
        }

        public override IEnumerator ShowingOrHiding(bool show)
        {
            float targetAlpha = show ? 1.0f : 0.0f;
            CanvasGroup self = _rootCG;

            while (self.alpha != targetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime);
                yield return null;
            }

            co_revealing = null;
            co_hiding = null;
        }
    }
}