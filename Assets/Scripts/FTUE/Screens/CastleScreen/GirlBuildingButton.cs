using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class GirlBuildingButton : Overlewd.NSCastleScreen.GirlBuildingButton
            {
                protected override void ButtonClick()
                {
                    
                }

                protected override void Customize()
                {
                    base.Customize();

                }

                public new static GirlBuildingButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/GirlBuildingButton"), parent);
                    newItem.name = nameof(GirlBuildingButton);

                    return newItem.AddComponent<GirlBuildingButton>();
                }
            }
        }
    }
}