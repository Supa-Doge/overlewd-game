using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class SidebarButtonWidget : Overlewd.SidebarButtonWidget
        {
            protected override void SidebarMenuButtonClick()
            {
                if (!UIManager.HasOverlay<SidebarMenuOverlay>())
                {
                    UIManager.ShowOverlay<SidebarMenuOverlay>();
                }
                else
                {
                    UIManager.HideOverlay();
                }
            }

            public new static SidebarButtonWidget GetInstance(Transform parent)
            {
                var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/SidebarButtonWidget/SidebarButtonWidget"), parent);
                prefab.name = nameof(SidebarButtonWidget);
                var rectTransform = prefab.GetComponent<RectTransform>();
                UIManager.SetStretch(rectTransform);
                return prefab.AddComponent<SidebarButtonWidget>();
            }
        }
    }
}