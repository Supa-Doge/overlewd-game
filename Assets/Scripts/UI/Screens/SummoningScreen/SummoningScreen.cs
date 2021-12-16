using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SummoningScreen : BaseScreen
    {
        private Button backButton;
        private Button haremButton;
        private Button portalButton;

        private List<Transform> shardPositions = new List<Transform>();

        private void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/SummoningScreen/SummoningScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            haremButton = canvas.Find("HaremButton").GetComponent<Button>();
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            
            backButton.onClick.AddListener(BackButtonClick);
            haremButton.onClick.AddListener(HaremButtonClick);
            portalButton.onClick.AddListener(PortalButtonClick);
            
            Customize();
        }

        private void Customize()
        {
            var shardPos = transform.Find("ShardPositions");
            var childCount = shardPos.Find("ShardPositions").childCount;
            
            for (int i = 1; i <= childCount; i++)
            {
                shardPositions.Add(shardPos.Find($"Shard{i}"));
            }
        }
        
        private void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }

        private void HaremButtonClick()
        {
            UIManager.ShowScreen<HaremScreen>();
        }

        private void PortalButtonClick()
        {
            UIManager.ShowScreen<PortalScreen>();
        }
    }

}
