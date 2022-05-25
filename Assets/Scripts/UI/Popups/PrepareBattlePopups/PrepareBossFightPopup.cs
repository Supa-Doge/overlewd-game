using System;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class PrepareBossFightPopup : BasePopupParent<PrepareBossFightPopupInData>
    {
        private const int RewardsCount = 3;

        private Button backButton;
        private Button battleButton;
        private Button editTeamButton;
        private Button buffButton;
        private RectTransform buffRect;
        private Transform bossPos;
        private Transform allyContent;

        private Image firstTimeReward;
        private Image[] rewards = new Image[RewardsCount];
        private TextMeshProUGUI[] rewardsAmount = new TextMeshProUGUI[RewardsCount];

        private TextMeshProUGUI firstTimeRewardCount;

        private TextMeshProUGUI markers;
        private AdminBRO.Battle battleData;

        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/PrepareBattlePopups/PrepareBossFightPopup/PrepareBossFightPopup",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");
            var levelTitle = canvas.Find("LevelTitle");
            var rewardsTr = canvas.Find("ResourceBack").Find("Rewards");
            var alliesBack = canvas.Find("AlliesBack");
            var buff = canvas.Find("Buff");
            var enemyBack = canvas.Find("EnemyBack");
            
            bossPos = enemyBack.Find("BossPos");
            allyContent = alliesBack.Find("Characters");
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            battleButton = canvas.Find("BattleButton").GetComponent<Button>();
            battleButton.onClick.AddListener(BattleButtonClick);

            editTeamButton = alliesBack.Find("EditTeamButton").GetComponent<Button>();
            editTeamButton.onClick.AddListener(EditTeamButtonClick);

            buffButton = buff.Find("SwitchBuffButton").GetComponent<Button>();
            buffRect = buff.GetComponent<RectTransform>();

            buffButton.onClick.AddListener(BuffButtonClick);
            UITools.TopHide(buffRect);

            markers = levelTitle.Find("Markers").GetComponent<TextMeshProUGUI>();

            firstTimeReward = rewardsTr.Find("FirstTimeReward").GetComponent<Image>();
            firstTimeRewardCount = firstTimeReward.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            firstTimeReward.gameObject.SetActive(false);

            for (int i = 0; i < rewards.Length; i++)
            {
                var reward = rewardsTr.Find("Reward" + i).GetComponent<Image>();
                rewards[i] = reward;
                rewards[i].gameObject.SetActive(false);

                var amount = reward.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                rewardsAmount[i] = amount;
            }
        }

        private void Customize()
        {
            foreach (var phase in battleData.battlePhases)
            {
                foreach (var enemy in phase.enemyCharacters)
                {
                    var enemyChar = NSPrepareBossFightPopup.Boss.GetInstance(bossPos);
                    enemyChar.characterData = enemy;
                }
            }
            
            var characters = GameData.characters.myTeamCharacters;
            var overlordData = GameData.characters.overlord;

            var overlordInst = NSPrepareBattlePopup.AllyCharacter.GetInstance(allyContent);
            overlordInst.characterData = overlordData;
            
            foreach (var ally in characters)
            {
                var allyChar = NSPrepareBattlePopup.AllyCharacter.GetInstance(allyContent);
                allyChar.characterData = ally;
            }
            
            if (battleData.firstRewards.Count > 0)
            {
                var firstReward = battleData.firstRewards[0];

                firstTimeReward.gameObject.SetActive(true);
                firstTimeReward.sprite = ResourceManager.LoadSprite(firstReward.icon);
                firstTimeRewardCount.text = firstReward.amount.ToString();
            }

            if (battleData.rewards.Count < 1)
                return;

            for (int i = 0; i < battleData.rewards.Count; i++)
            {
                var reward = battleData.rewards[i];
                rewards[i].gameObject.SetActive(true);
                rewards[i].sprite = ResourceManager.LoadSprite(reward.icon);
                rewardsAmount[i].text = reward.amount.ToString();
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            battleData = inputData.eventStageData?.battleData ?? inputData.ftueStageData?.battleData;
            Customize();

            switch (inputData.ftueStageData?.ftueState)
            {
                case (_, "chapter1"):
                    UITools.DisableButton(editTeamButton);
                    break;
            }

            await Task.CompletedTask;
        }

        private void EditTeamButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<TeamEditScreen>().
                SetData(new TeamEditScreenInData 
                {
                    prevScreenInData = UIManager.currentScreenInData,
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId
                }).RunShowScreenProcess();
        }

        private void BuffButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<HaremScreen>().
                SetData(new HaremScreenInData
                {
                    prevScreenInData = UIManager.currentScreenInData,
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId
                }).RunShowScreenProcess();
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        private void BattleButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
            UIManager.MakeScreen<BossFightScreen>().
                SetData(new BaseBattleScreenInData
                {
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId
                }).RunShowScreenProcess();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenLeftShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenLeftHide>();
        }

        public override async Task AfterShowAsync()
        {
            await UITools.TopShowAsync(buffRect, 0.2f);

            //ftue part
            switch (GameData.ftue.stats.lastEndedState)
            {
                case ("battle4", "chapter1"):
                    GameData.ftue.info.chapter1.ShowNotifByKey("potionstutor1");
                    break;
            }
        }

        public override async Task BeforeHideAsync()
        {
            await UITools.TopHideAsync(buffRect, 0.2f);
        }
    }

    public class PrepareBossFightPopupInData : BasePopupInData
    {
       
    }
}