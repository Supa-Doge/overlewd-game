using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenFadeShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override async Task ProgressAsync()
        {
            screen.StartShow();
            await UITools.FadeShowAsync(gameObject);
            await screen.AfterShowAsync();
            Destroy(this);
        }
    }
}
