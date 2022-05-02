using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class FightButton : BaseStageButton
        {
            private Button button;
            private GameObject fightDone;
            protected GameObject icon;
            protected GameObject bossIcon;
            
            private TextMeshProUGUI title;
            private TextMeshProUGUI loot;
            private TextMeshProUGUI markers;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                fightDone = button.transform.Find("Done").gameObject;
                icon = button.transform.Find("Icon").gameObject;
                bossIcon = button.transform.Find("BossIcon").gameObject;
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                loot = button.transform.Find("Loot").GetComponent<TextMeshProUGUI>();
                markers = button.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            }

            void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var eventStageData = stageData;
                var battleData = stageData.battleData;
                title.text = eventStageData.title;
                loot.text = battleData.rewardSpriteString;
                
                icon.SetActive(battleData.type == AdminBRO.Battle.Type_Battle);
                bossIcon.SetActive(battleData.type == AdminBRO.Battle.Type_Boss);
                fightDone.SetActive(eventStageData.status == AdminBRO.EventStageItem.Status_Complete);
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                var battleData = stageData.battleData;

                if (battleData.type == AdminBRO.Battle.Type_Battle)
                {
                    UIManager.MakePopup<PrepareBattlePopup>().
                        SetData(new PrepareBattlePopupInData
                        {
                            eventStageId = stageId
                        }).RunShowPopupProcess();
                }
                else if (battleData.type == AdminBRO.Battle.Type_Boss)
                {
                    UIManager.MakePopup<PrepareBossFightPopup>().
                        SetData(new PrepareBossFightPopupInData
                        {
                            eventStageId = stageId
                        }).RunShowPopupProcess();
                }
            }

            public static FightButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<FightButton>
                    ("Prefabs/UI/Screens/ChapterScreens/FightButton", parent);
            }
        }
    }
}