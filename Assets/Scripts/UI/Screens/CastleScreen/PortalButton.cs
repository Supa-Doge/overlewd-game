using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class PortalButton : BaseButton
        {
            protected Transform markers;
            protected Transform freeSummonNotification;

            protected override void Awake()
            {
                base.Awake();

                markers = transform.Find("Markers");
                freeSummonNotification = transform.Find("FreeSummonNotification");
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.ShowScreen<PortalScreen>();
            }

            public static PortalButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<PortalButton>
                    ("Prefabs/UI/Screens/CastleScreen/PortalButton", parent);
            }
        }
    }
}
