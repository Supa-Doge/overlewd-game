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
            void Start()
            {
                transform.Find("Canvas").Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    UIManager.ShowScreen<PrepareBattleScreen>();
                });

                var rectTransform = GetComponent<RectTransform>();
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 40.0f, rectTransform.rect.width);
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 200.0f, rectTransform.rect.height);
            }

            void Update()
            {

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
