using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public abstract class SubPopupMissclick : BaseMissclick
    {
        protected override void OnClick()
        {
            UIManager.HideSubPopup();
        }
    }

    public class SubPopupMissclickClear : SubPopupMissclick
    {

    }

    public class SubPopupMissclickColored : SubPopupMissclick
    {
        protected override void Awake()
        {
            base.Awake();

            image.color = new Color(0.0f, 0.0f, 0.0f, 0.8f);
        }

        public override MissclickShow Show()
        {
            return gameObject.AddComponent<MissclickFadeShow>();
        }

        public override MissclickHide Hide()
        {
            return gameObject.AddComponent<MissclickFadeHide>();
        }
    }
}
