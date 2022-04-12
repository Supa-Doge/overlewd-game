using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Overlewd.FTUE;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class StartingScreen : BaseFullScreen
    {
        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/StartingScreen/StartingScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            canvas.Find("FTUE_Progress").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                GameGlobalStates.ftueProgressMode = true;
                GameGlobalStates.ftueChapterData = null;
                UIManager.ShowScreen<MapScreen>();
            });

            canvas.Find("FTUE").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                GameGlobalStates.ftueProgressMode = false;
                GameGlobalStates.ftueChapterData = null;
                UIManager.ShowScreen<MapScreen>();
                /*UIManager.ShowScreen<FTUE.SexScreen>().
                    SetStageData(GameGlobalStates.GetFTUEStageByKey("sex1"));*/
            });

            canvas.Find("Reset_FTUE").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                FTUEReset();
            });

            canvas.Find("Castle").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowScreen<CastleScreen>();
            });
            
            canvas.Find("BattleScreen").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowScreen<BaseBattleScreen>();
            });
        }

        private async void FTUEReset()
        {
            await GameData.FTUEReset();
        }
    }
}
