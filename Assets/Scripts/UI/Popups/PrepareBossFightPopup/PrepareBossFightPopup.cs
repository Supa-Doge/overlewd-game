using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class PrepareBossFightPopup : BasePopup
    {
        private List<Image> rewards;
        
        private Button backButton;
        private Button battleButton;
        private Button prepareButton;
        
        private int rewardsCount = 4;       
        
       private void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Popups/PrepareBossFightPopup/PrepareBossFightPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            rewards = new List<Image>(rewardsCount);
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            battleButton = canvas.Find("BattleButton").GetComponent<Button>();
            battleButton.onClick.AddListener(BattleButtonClick);

            prepareButton = canvas.Find("PrepareBattleButton").GetComponent<Button>();
            prepareButton.onClick.AddListener(PrepareButtonClick);
            
            TakeRewards(canvas);

            rewards[0].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Crystal");
            rewards[1].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Crystal");
            rewards[2].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gold");
            rewards[3].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Stone");
        }

        private void TakeRewards(Transform canvas)
        {
            rewards.Add(canvas.Find("FirstTimeReward").Find("Resource").GetComponent<Image>());
            rewards.Add(canvas.Find("Reward1").Find("Resource").GetComponent<Image>());
            rewards.Add(canvas.Find("Reward2").Find("Resource").GetComponent<Image>());
            rewards.Add(canvas.Find("Reward3").Find("Resource").GetComponent<Image>());
        }
        
        private void BackButtonClick()
        {
            UIManager.HidePopup();
        }

        private void BattleButtonClick()
        {
            UIManager.ShowScreen<BossFightScreen>();
        }

        private void PrepareButtonClick()
        {
            UIManager.ShowSubPopup<BottlesSubPopup>();
        }
    }
}
