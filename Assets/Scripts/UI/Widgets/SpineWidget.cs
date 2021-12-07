using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;
using Spine;

namespace Overlewd
{
    public class SpineWidget : BaseWidget
    {
        public event Action startListeners;
        public event Action completeListeners;

        private SkeletonDataAsset skeletonDataAsset;
        private SkeletonGraphic skeletonGraphic;

        public void Initialize(string skeletonDataPath, bool multipleRenderCanvas)
        {
            skeletonDataAsset = Resources.Load<SkeletonDataAsset>(skeletonDataPath);
            skeletonGraphic = gameObject.AddComponent<SkeletonGraphic>();
            skeletonGraphic.allowMultipleCanvasRenderers = multipleRenderCanvas;
            skeletonGraphic.skeletonDataAsset = skeletonDataAsset;
            skeletonGraphic.Initialize(false);

            skeletonGraphic.AnimationState.Start += StartListener;
            skeletonGraphic.AnimationState.Complete += CompleteListener;
        }

        private void StartListener(TrackEntry e)
        {
            startListeners?.Invoke();
        }

        private void CompleteListener(TrackEntry e)
        {
            completeListeners?.Invoke();
        }

        public void PlayAnimation(string animationName, bool loop)
        {
            skeletonGraphic.AnimationState.SetAnimation(0, animationName, loop);
        }

        public static SpineWidget GetInstance(Transform parent)
        {
            var spineGO = new GameObject(nameof(SpineWidget));
            var spineGO_rt = spineGO.AddComponent<RectTransform>();
            spineGO_rt.SetParent(parent, false);
            return spineGO.AddComponent<SpineWidget>();
        }
    }
}
