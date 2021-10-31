using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class FightButton : MonoBehaviour
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
                GameGlobalStates.battle_EventStageData = eventStageData;
                UIManager.ShowPopup<PrepareBattlePopup>();
            }

            public static FightButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/FightButton"), parent);
                newItem.name = nameof(FightButton);
                return newItem.AddComponent<FightButton>();
            }
        }
    }
}
