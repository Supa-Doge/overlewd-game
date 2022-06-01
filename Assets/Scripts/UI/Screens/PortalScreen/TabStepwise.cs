using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class TabStepwise : BaseTab
        {
            private Button buyButton;
            private TextMeshProUGUI buyButtonText;
            private TextMeshProUGUI timer;
            private List<Transform> steps = new List<Transform>();

            protected override void Awake()
            {
                base.Awake();
                
                buyButton = content.Find("BuyButton").GetComponent<Button>();
                buyButtonText = buyButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                timer = content.Find("Timer").Find("Time").GetComponent<TextMeshProUGUI>();

                for (int i = 0; i < 10; i++)
                {
                    steps.Add(content.Find("Steps").Find($"Step{i + 1}"));
                }
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {

            }
            
            public static TabStepwise GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<TabStepwise>
                    ("Prefabs/UI/Screens/PortalScreen/TabStepwise", parent);
            }
        }
    }
}
