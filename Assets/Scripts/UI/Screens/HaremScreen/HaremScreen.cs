using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class HaremScreen : BaseScreen
    {
        private Button backButton;

        private Button ulviButton;
        private Image ulviGirl;
        private Image ulviBuff;
        private TextMeshProUGUI ulviDescription;
        private TextMeshProUGUI ulviName;

        private Button adrielButton;
        private Image adrielGirl;
        private Image adrielBuff;
        private TextMeshProUGUI adrielDescription;
        private TextMeshProUGUI adrielName;
        private Transform adrielNotActive;

        private Button ingieButton;
        private TextMeshProUGUI ingieName;
        private Transform ingieNotActive;

        private Button fayeButton;
        private TextMeshProUGUI fayeName;
        private Transform fayeNotActive;

        private Button liliButton;
        private TextMeshProUGUI liliName;
        private Transform liliNotActive;

        private Button battleGirlsButton;
        private Image battleGirlsGirl;
        private TextMeshProUGUI battleGirlsTitle;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/HaremScreen/Harem", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            ulviButton = canvas.Find("UlviButton").GetComponent<Button>();
            ulviButton.onClick.AddListener(UlviButtonClick);
            ulviGirl = ulviButton.transform.Find("Girl").GetComponent<Image>();
            ulviBuff = ulviButton.transform.Find("Buff").GetComponent<Image>();
            ulviDescription = ulviButton.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            ulviName = ulviButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            adrielButton = canvas.Find("AdrielButton").GetComponent<Button>();
            adrielButton.onClick.AddListener(AdrielButtonClick);
            adrielGirl = adrielButton.transform.Find("Girl").GetComponent<Image>();
            adrielBuff = adrielButton.transform.Find("Buff").GetComponent<Image>();
            adrielDescription = adrielButton.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            adrielName = adrielButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            adrielNotActive = adrielButton.transform.Find("NotActive");

            ingieButton = canvas.Find("IngieButton").GetComponent<Button>();
            ingieButton.onClick.AddListener(IngieButtonClick);
            ingieName = ingieButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            ingieNotActive = ingieButton.transform.Find("NotActive");

            fayeButton = canvas.Find("FayeButton").GetComponent<Button>();
            fayeButton.onClick.AddListener(FayeButtonClick);
            fayeName = fayeButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            fayeNotActive = fayeButton.transform.Find("NotActive");

            liliButton = canvas.Find("LiliButton").GetComponent<Button>();
            liliButton.onClick.AddListener(LiliButtonClick);
            liliName = liliButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            liliNotActive = liliButton.transform.Find("NotActive");

            battleGirlsButton = canvas.Find("BattleGirlsButton").GetComponent<Button>();
            battleGirlsButton.onClick.AddListener(BattleGirlsButtonClick);
            battleGirlsGirl = battleGirlsButton.transform.Find("Girl").GetComponent<Image>();
            battleGirlsTitle = battleGirlsButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            Customize();
        }

        private void Customize()
        {

        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

        private void UlviButtonClick()
        {
            UIManager.ShowScreen<GirlScreen>();
            GameGlobalStates.haremGirlNameSelected = ulviName.text;
        }

        private void AdrielButtonClick()
        {
            // UIManager.ShowScreen<GirlScreen>();
        }

        private void IngieButtonClick()
        {

        }

        private void FayeButtonClick()
        {

        }

        private void LiliButtonClick()
        {

        }

        private void BattleGirlsButtonClick()
        {

        }
    }
}
