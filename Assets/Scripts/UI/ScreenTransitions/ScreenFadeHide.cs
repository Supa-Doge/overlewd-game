using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenFadeHide : ScreenHide
    {
        private CanvasGroup canvasGroup;
        private bool localCanvasGroup = false;

        protected override void Awake()
        {
            base.Awake();

            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
                localCanvasGroup = true;
            }
            canvasGroup.alpha = 1.0f;
        }
        async void Start()
        {
            await screen.BeforeHideAsync();
            prepared = true;
            startTransitionListeners?.Invoke();
        }

        void Update()
        {
            if (!prepared || locked)
                return;

            time += deltaTimeInc;
            float transitionProgressPercent = time / duration;
            float transitionPercent = 1.0f - EasingFunction.easeInBack(transitionProgressPercent);

            canvasGroup.alpha = transitionPercent;

            if (time > duration)
            {
                endTransitionListeners?.Invoke();
                screen.AfterHide();
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            if (localCanvasGroup)
            {
                Destroy(canvasGroup);
            }
        }
    }
}