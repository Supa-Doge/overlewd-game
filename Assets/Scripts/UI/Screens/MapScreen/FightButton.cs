using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class FightButton : MonoBehaviour
        {
            public string header
            {
                set
                {
                    title.text = value;
                }
            }
            
            public string potentialLoot
            {
                set
                {
                    loot.text = $"You can loot   <size=26>{value}</size>";
                }
            }
            
            private Button button;
            private Transform fightDone;
            
            private TextMeshProUGUI title;
            private TextMeshProUGUI loot;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                loot = button.transform.Find("CanLoot").GetComponent<TextMeshProUGUI>();
                
                fightDone = button.transform.Find("FightDone");

                button.onClick.AddListener(ButtonClick);
            }

            private void ButtonClick()
            {
                UIManager.ShowScreen<PrepareBattlePopup>();
            }
            
            public static FightButton GetInstance(Transform parent)
            {
                var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/FightButton"), parent);
                newItem.name = nameof(FightButton);
                
                return newItem.AddComponent<FightButton>();
            }
        }
    }
}

