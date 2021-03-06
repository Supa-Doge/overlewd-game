using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class CatacombsButton : BaseButton
        {
            protected override void Awake()
            {
                base.Awake();
                UITools.DisableButton(button);
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
            }

            public static CatacombsButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CatacombsButton>
                    ("Prefabs/UI/Screens/CastleScreen/CatacombsButton", parent);
            }
        }
    }
}
