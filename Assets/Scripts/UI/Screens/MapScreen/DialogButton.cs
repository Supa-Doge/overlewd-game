using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class DialogButton : BaseStageButton
        {
            protected override void Start()
            {
                base.Start();

                var dialogId = stageData?.dialogId;
                if (dialogId.HasValue)
                {
                    var dialogData = GameData.GetDialogById(dialogId.Value);
                    title.text = dialogData.title;
                }
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.MakeScreen<FTUE.DialogScreen>().
                    SetData(new DialogScreenInData
                    {
                        ftueStageData = stageData
                    }).RunShowScreenProcess();
            }

            public static DialogButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<DialogButton>
                    ("Prefabs/UI/Screens/MapScreen/DialogButton", parent);
            }
        }
    }
}
