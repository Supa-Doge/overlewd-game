using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Overlewd
{
    public class BattleScreen : BaseScreen
    {
        private Button startBattleButton;
        private Button backButton;

        private VideoPlayer battleVideo;

        async void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/BattleScreen/BattleScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            startBattleButton = canvas.Find("StartBattleButton").GetComponent<Button>();
            startBattleButton.onClick.AddListener(StartBattleButtonClick);

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            battleVideo = canvas.Find("TestVideo").GetComponent<VideoPlayer>();

            await GameData.EventStageStartAsync(GameGlobalStates.battle_EventStageData);
        }

        private async void EndBattleVideo(VideoPlayer vp)
        {
            backButton.gameObject.SetActive(true);
            startBattleButton.gameObject.SetActive(true);

            await GameData.EventStageEndAsync(GameGlobalStates.battle_EventStageData);

            UIManager.ShowPopup<VictoryPopup>();
        }

        private void StartBattleButtonClick()
        {
            battleVideo.Play();
            battleVideo.loopPointReached += EndBattleVideo;

            backButton.gameObject.SetActive(false);
            startBattleButton.gameObject.SetActive(false);
        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<EventMapScreen>();
        }

        void Update()
        {

        }
    }
}
