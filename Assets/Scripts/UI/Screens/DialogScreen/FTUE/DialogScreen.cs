using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Overlewd
{
    namespace FTUE
    {
        public class DialogScreen : Overlewd.DialogScreen
        {
            public override async Task BeforeShowDataAsync()
            {
                dialogData = GameData.GetDialogById(inputData.ftueStageData.dialogId.Value);
                await GameData.FTUEStartStage(inputData.ftueStageData.id);
            }

            public override async Task BeforeHideDataAsync()
            {
                await GameData.FTUEEndStage(inputData.ftueStageData.id);
            }

            protected override void LeaveScreen()
            {
                UIManager.ShowScreen<MapScreen>();
            }
        }
    }
}