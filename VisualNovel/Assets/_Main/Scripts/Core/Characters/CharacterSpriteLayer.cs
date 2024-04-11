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
        public bool isTransitioningLayer    => co_transitioningLayer != null;
        public bool isLevelingAlpha         => co_levelingAlpha != null;


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
    }
}