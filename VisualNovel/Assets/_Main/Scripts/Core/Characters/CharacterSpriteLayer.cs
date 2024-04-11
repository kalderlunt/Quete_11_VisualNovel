using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace CHARACTERS
{
    public class CharacterSpriteLayer
    {
        private CharacterManager _characterManager => CharacterManager.instance;

        private const float DEFAULT_TRANSITION_SPEED = 3f;
        private float _transitionSpeedMultiplier = 1f;

        public int layer { get; private set; } = 0;
        public Image renderer {  get; private set; } = null;
        public CanvasGroup rendererCG => renderer.GetComponent<CanvasGroup>();

        private List<CanvasGroup> _oldRenderers = new();

        private Coroutine co_transitioningLayer = null;
        private Coroutine co_levelingAlpha      = null;
        private Coroutine co_changingColor      = null;


        public bool isTransitioningLayer    => co_transitioningLayer != null;
        public bool isLevelingAlpha         => co_levelingAlpha != null;
        public bool isChangingColor         => co_changingColor != null;


        public CharacterSpriteLayer (Image defaultRenderer, int layer = 0)
        {
            renderer = defaultRenderer;
            this.layer = layer;
        }

        public void SetSprite(Sprite sprite)
        {
            renderer.sprite = sprite;
        }
        public Coroutine TransitionSprite(Sprite sprite, float speed = 1)
        {
            if (sprite == renderer.sprite)
                return null;

            if (isTransitioningLayer)
                _characterManager.StopCoroutine(co_transitioningLayer);

            co_transitioningLayer = _characterManager.StartCoroutine(TransitioningSprite(sprite, speed));
            
            return co_transitioningLayer;
        }

        private IEnumerator TransitioningSprite(Sprite sprite, float speedMultiplier)
        {
            _transitionSpeedMultiplier = speedMultiplier;
            
            Image newRenderer = CreateRenderer(renderer.transform.parent);
            newRenderer.sprite = sprite;

            yield return TryStartLevelingAlphas();

            co_transitioningLayer = null;
        }

        private Image CreateRenderer(Transform parent)
        {
            Image newRenderer = Object.Instantiate(renderer, parent);
            _oldRenderers.Add(rendererCG);

            newRenderer.name = renderer.name;
            renderer = newRenderer;
            renderer.gameObject.SetActive(true);
            rendererCG.alpha = 0f;

            return newRenderer;
        }

        public Coroutine TryStartLevelingAlphas()
        {
            if (isLevelingAlpha)
                return co_levelingAlpha;

            co_levelingAlpha = _characterManager.StartCoroutine(RunAlphaLeveling());

            return co_levelingAlpha;
        }

        private IEnumerator RunAlphaLeveling()
        {
            while (rendererCG.alpha < 1f || _oldRenderers.Any(oldGC => oldGC.alpha > 0f))
            {
                float speed = DEFAULT_TRANSITION_SPEED * _transitionSpeedMultiplier * Time.deltaTime;
                rendererCG.alpha = Mathf.MoveTowards(rendererCG.alpha, 1, speed);

                for (int i = _oldRenderers.Count - 1; i >= 0; i--)
                {
                    CanvasGroup oldCG = _oldRenderers[i];
                    oldCG.alpha = Mathf.MoveTowards(oldCG.alpha, 0, speed);

                    if(oldCG.alpha <= 0f)
                    {
                        _oldRenderers.RemoveAt(i);
                        Object.Destroy(oldCG.gameObject);
                    }
                }

                yield return null;
            }

            co_levelingAlpha = null;
        }

        public void SetColor(Color color)
        {
            renderer.color = color;

            foreach (CanvasGroup oldCG in _oldRenderers)
            {
                oldCG.GetComponent<Image>().color = color;
            }
        }

        public Coroutine TransitionColor(Color color, float speed)
        {
            if (isChangingColor)
                _characterManager.StopCoroutine(co_changingColor);

            co_changingColor = _characterManager.StartCoroutine(ChangingColor(color, speed));

            return co_changingColor;
        }

        private IEnumerator ChangingColor(Color color, float speedMultiplier)
        {
            Color oldColor = renderer.color;

            List<Image> oldImages = new();

            foreach (CanvasGroup oldCG in _oldRenderers)
            {
                oldImages.Add(oldCG.GetComponent<Image>());
            }

            float colorPercent = 0;
            while (colorPercent < 1)
            {
                colorPercent += DEFAULT_TRANSITION_SPEED * speedMultiplier * Time.deltaTime;

                renderer.color = Color.Lerp(oldColor, color, colorPercent);

                foreach (Image oldImage in oldImages)
                {
                    oldImage.color = renderer.color;
                }

                yield return null;
            }

            co_changingColor = null;
        }
    }
}