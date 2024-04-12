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
        private Coroutine co_flipping = null;

        private bool _isFacingLeft = Character.DEFAULT_ORIENTATION_IS_FACING_LEFT;

        public bool isTransitioningLayer    => co_transitioningLayer != null;
        public bool isLevelingAlpha         => co_levelingAlpha != null;
        public bool isChangingColor         => co_changingColor != null;
        public bool isFlipping              => co_flipping      != null;

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

        public void StopChangingColor()
        {
            if (!isChangingColor)
                return;

            _characterManager.StopCoroutine(co_changingColor);

            co_changingColor = null;
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

        public Coroutine Flip(float speed = 1, bool immediate = false)
        {
            if (_isFacingLeft)
                return FaceRight(speed, immediate);
            else
                return FaceLeft(speed, immediate);
        }

        public Coroutine FaceLeft(float speed = 1, bool immediate = false)
        {
            if (isFlipping)
                _characterManager.StopCoroutine(co_flipping);

            _isFacingLeft = true;
            co_flipping = _characterManager.StartCoroutine(FaceDirection(_isFacingLeft, speed, immediate));
            
            return co_flipping;
        }

        public Coroutine FaceRight(float speed = 1, bool immediate = false)
        {
            if (isFlipping)
                _characterManager.StopCoroutine(co_flipping);

            _isFacingLeft = false;
            co_flipping = _characterManager.StartCoroutine(FaceDirection(_isFacingLeft, speed, immediate));

            return co_flipping;
        }

        private IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            float xScale = faceLeft ? 1 : -1;
            Vector3 newScale = new(xScale, 1, 1);

            if (!immediate)
            {
                Image newRenderer = CreateRenderer(renderer.transform.parent);

                newRenderer.transform.localScale = newScale;

                _transitionSpeedMultiplier = speedMultiplier;
                TryStartLevelingAlphas();

                while (isLevelingAlpha)
                    yield return null;
            }
            else
            {
                renderer.transform.localScale = newScale;
            }

            co_flipping = null;


            /*
             *  {
            Image newRenderer = CreateRenderer(renderer.transform.parent);

            Vector3 currentScale = newRenderer.transform.localScale;
            
            float currentX = System.Math.Abs(currentScale.x);

            float xScale = faceLeft ? currentX : -currentX;

            Vector3 newScale = new Vector3(xScale, currentScale.y, currentScale.z);

            if (!immediate)
            {
                newRenderer.transform.localScale = newScale;

                transitionSpeedMultiplier = speedMultiplier;

                TryStartLevelingAlphas();

                while (isLevelingAlpha)
                    yield return null;
            }
            else
            {
                renderer.transform.localScale = newScale;
            }

            co_flipping = null;
        }
             */
        }
    }
}