using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

namespace DIALOGUE
{
    [System.Serializable]
    /// <summary>
    /// The box that hols the name text on screen. Part of the dialogue container.
    /// </summary>
    public class NameContainer
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private TextMeshProUGUI _nameText;

        public void Show(string nameToShow = "")
        {
            _root.SetActive(true);

            if (nameToShow != string.Empty)
                _nameText.text = nameToShow;
        }

        public void Hide() 
        {
            _root.SetActive(false);
        }

        public void SetNameColor(Color color) => _nameText.color = color;
        public void SetNameFont(TMP_FontAsset font) => _nameText.font = font;
    }
}