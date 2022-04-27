using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DefeatPopup : BasePopup
    {
        protected Button magicGuildButton;
        protected Button inventoryButton;
        protected Button haremButton;
        protected Button editTeamButton;

        protected DefeatPopupInData inputData;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/DefeatPopup/DefeatPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            magicGuildButton = canvas.Find("MagicGuildButton").GetComponent<Button>();
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);

            inventoryButton = canvas.Find("InventoryButton").GetComponent<Button>();
            inventoryButton.onClick.AddListener(InventoryButtonClick);

            haremButton = canvas.Find("HaremButton").GetComponent<Button>();
            haremButton.onClick.AddListener(HaremButtonClick);

            editTeamButton = canvas.Find("EditTeamButton").GetComponent<Button>();
            editTeamButton.onClick.AddListener(EditTeamButtonClick);
        }

        public DefeatPopup SetData(DefeatPopupInData data)
        {
            inputData = data;
            return this;
        }

        void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {

        }

        public override void MakeMissclick()
        {
            var missClick = UIManager.MakePopupMissclick<PopupMissclickColored>();
            missClick.missClickEnabled = false;
        }

        protected virtual void EditTeamButtonClick()
        {
            
        }
        
        protected virtual void MagicGuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MagicGuildScreen>();
        }

        protected virtual void InventoryButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<InventoryAndUserScreen>();
        }

        protected virtual void HaremButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }
    }

    public class DefeatPopupInData
    {
        public AdminBRO.FTUEStageItem ftueStageData;
    }
}