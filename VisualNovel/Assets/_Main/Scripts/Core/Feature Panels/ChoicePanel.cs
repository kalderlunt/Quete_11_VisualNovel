using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour
{
    public static ChoicePanel instance { get; private set; }
    
    [SerializeField] private CanvasGroup _cancasGroup;
    [SerializeField] private TextMeshProUGUI _titleText_;
    [SerializeField] private GameObject _choiceButtonPrefab;
    [SerializeField] private VerticalLayoutGroup _buttonLayoutGroup;

    //private CanvasGroupController cg = null;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(gameObject);
    }

    void Start()
    {
        //cg = new(this, _cancasGroup);
    }
}