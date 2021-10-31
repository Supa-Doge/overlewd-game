using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class BossFightButton : MonoBehaviour
        {
            public AdminBRO.EventStageItem eventStageData { get; set; }

            private Button button;
            private Transform fightDone;

            void Start()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                fightDone = button.transform.Find("FightDone");
            }

            void Update()
            {

            }

            private void ButtonClick()
            {
                GameGlobalStates.bossFight_EventStageData = eventStageData;
                UIManager.ShowPopup<PrepareBossFightPopup>();
            }

            public static BossFightButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/BossFightButton"), parent);
                newItem.name = nameof(BossFightButton);
                return newItem.AddComponent<BossFightButton>();
            }
        }
    }
}
