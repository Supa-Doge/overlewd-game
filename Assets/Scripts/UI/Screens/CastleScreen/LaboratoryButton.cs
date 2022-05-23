using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class LaboratoryButton : BaseButton
        {
            private Transform notification;
            
            protected override void Awake()
            {
                base.Awake();
                notification = transform.Find("MergingAvailableNotification");
                UITools.DisableButton(button);
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
            }

            public static LaboratoryButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<LaboratoryButton>
                    ("Prefabs/UI/Screens/CastleScreen/LaboratoryButton", parent);
            }
        }
    }
}