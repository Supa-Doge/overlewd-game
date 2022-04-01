using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenFadeShow : ScreenShow
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
            canvasGroup.alpha = 0.0f;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (localCanvasGroup)
            {
                Destroy(canvasGroup);
            }
        }

        public override async Task ProgressAsync()
        {
            screen.StartShow();
            await UITools.FadeShowAsync(canvasGroup);
            await screen.AfterShowAsync();
            Destroy(this);
        }
    }
}
