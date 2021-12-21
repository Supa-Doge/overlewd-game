using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        public static class GameGlobalStates
        {
            public static int currentStageId = 0;
            public static void IncStage()
            {
                currentStageId++;
            }

            public static bool ulviCaveCanBuild = false;
            public static bool ulviCaveIsBuild = false;
            public static bool portalCanBuild = false;
            public static bool portalIsBuild = false;

            public static int battleScreen_BattleId = 0;

            public static int sexScreen_DialogId;
            public static AdminBRO.Dialog sexScreen_DialogData
            {
                get
                {
                    return GameData.GetSexById(sexScreen_DialogId);
                }
            }

            public static int dialogScreen_DialogId;
            public static AdminBRO.Dialog dialogScreen_DialogData
            {
                get
                {
                    return GameData.GetDialogById(dialogScreen_DialogId);
                }
            }

            public static int dialogNotification_DialogId;
            public static AdminBRO.Dialog dialogNotification_DialogData
            {
                get
                {
                    return GameData.GetNotificationById(dialogNotification_DialogId);
                }
            }
        }
    }
}
