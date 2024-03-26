using System.Collections;
using UnityEngine;
using TMPro;
using UnityEditor.Searcher;
using Unity.VisualScripting;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Rendering.Universal;
using System.Reflection;

public class TextArchitect
{
    private TextMeshProUGUI _tmpro_ui;
    private TextMeshPro _tmpro_world;

    public TMP_Text tmpro => _tmpro_ui != null ? _tmpro_ui : _tmpro_world;

    public string currentText => tmpro.text;
    public string targetText { get; private set; } = "";
    public string preText { get; private set; } = "";
    private int _preTextLenght = 0;
    public string fullTargetText => preText + targetText;

    public enum BuildMethod { instant, typewriter, fade }
    public BuildMethod buildMethod = BuildMethod.typewriter;

    public Color textColor { get { return tmpro.color; } set { tmpro.color = value; } }

    public float speed { get { return _baseSpeed * _speedMultiplier; } set { _speedMultiplier = value; } }
    private const float _baseSpeed = 1;
    private float _speedMultiplier = 1;

    public int characterPerCycle { get { return speed <= 2f ? _characterMultiplier : speed <= 2.5f ? _characterMultiplier * 2 : _characterMultiplier * 3; } }
    private int _characterMultiplier = 1;

    public bool hurryUp = false;

    public TextArchitect(TextMeshProUGUI tmpro_ui)
    {
        this._tmpro_ui = tmpro_ui;
    }
    
    public TextArchitect(TextMeshPro tmpro_world)
    {
        this._tmpro_world = tmpro_world;
    }

    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        Stop();

        _buildProcess = tmpro.StartCoroutine(Building());
        return _buildProcess;
    }

    // Append text to what is already in the text architect.
    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        _buildProcess = tmpro.StartCoroutine(Building());
        return _buildProcess;
    }

    private Coroutine _buildProcess = null;
    public bool isBuilding => _buildProcess != null;

    public void Stop()
    {
        if (!isBuilding)
            return;

        tmpro.StopCoroutine(_buildProcess);
        _buildProcess = null;
    }

    IEnumerator Building()
    {
        Prepare();

        switch(buildMethod)
        {
            case BuildMethod.typewriter:
                yield return Build_Typewriter();
                break;

            case BuildMethod.fade:
                yield return Build_Fade();
                break;
        }

        OnComplete();
    }

    private void OnComplete()
    {
        _buildProcess = null;
        hurryUp = false;
    }

    public void ForceComplete()
    {
        switch (buildMethod)
        { 
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;

            case BuildMethod.fade:
                break;
        }

        Stop();
        OnComplete();
    }

    private void Prepare()
    {
        switch(buildMethod)
        {
            case BuildMethod.instant:
                Prepare_Instant();
                break;

            case BuildMethod.typewriter:
                Prepare_Typewriter();
                break;

            case BuildMethod.fade:
                Prepare_Fade();
                break;
        }
    }

    private void Prepare_Instant()
    {
        tmpro.color = tmpro.color;
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
    }

    private void Prepare_Typewriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;

        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }

    private void Prepare_Fade()
    {

    }

    private IEnumerator Build_Typewriter()
    {
        while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += hurryUp ? characterPerCycle * 5 : characterPerCycle;

            yield return new WaitForSeconds(0.015f / speed);
        }
    }

    private IEnumerator Build_Fade()
    {
        yield return null;
    }
}