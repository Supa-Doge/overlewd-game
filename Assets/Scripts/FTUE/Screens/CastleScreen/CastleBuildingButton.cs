using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class CastleBuildingButton : Overlewd.NSCastleScreen.CastleBuildingButton
            {
                protected override void ButtonClick()
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_CastleScreenButtons);
                    UIManager.ShowScreen<BuildingScreen>();
                }

                protected override void Customize()
                {
                    base.Customize();

                    notificationGrid.gameObject.SetActive(false);
                    markers.gameObject.SetActive(false);

                    if (/*GameGlobalStates.castle_BuildingButtonLock*/ true)
                    {
                        UIHelper.DisableButton(button);
                    }
                }

                public new static CastleBuildingButton GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<CastleBuildingButton>
                        ("Prefabs/UI/Screens/CastleScreen/CastleBuildingButton", parent);
                }
            }
        }
    }
}