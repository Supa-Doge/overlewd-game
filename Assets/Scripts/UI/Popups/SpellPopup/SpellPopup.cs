using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SpellPopup : BasePopupParent<SpellPopupInData>
    {
        protected List<Transform> resources = new List<Transform>();
        protected List<GameObject> notEnough = new List<GameObject>();
        protected List<TextMeshProUGUI> count = new List<TextMeshProUGUI>();
        protected List<Image> resourceIcon = new List<Image>();

        protected Transform spawnPoint;

        protected TextMeshProUGUI spellName;
        protected TextMeshProUGUI description;
        protected TextMeshProUGUI fullPotentialDescription;

        protected Button paidBuildButton;
        protected Button freeBuildButton;
        protected Button backButton;

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/SpellPopup/SpellPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            spawnPoint = canvas.Find("Background").Find("ImageSpawnPoint");

            spellName = canvas.Find("SpellName").GetComponent<TextMeshProUGUI>();
            description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();
            fullPotentialDescription = canvas.Find("FullPotentialDescription").GetComponent<TextMeshProUGUI>();

            paidBuildButton = canvas.Find("PaidBuildButton").GetComponent<Button>();
            freeBuildButton = canvas.Find("FreeBuildButton").GetComponent<Button>();
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            
            paidBuildButton.onClick.AddListener(PaidBuildButtonClick);
            freeBuildButton.onClick.AddListener(FreeBuildButtonClick);
            backButton.onClick.AddListener(BackButtonClick);
            
            CustomizeResources(canvas);
        }

        private void CustomizeResources(Transform canvas)
        {
            var grid = canvas.Find("Grid");
            for (int i = 1; i <= grid.childCount; i++)
            {
                var resource = grid.Find($"Recource{i}");
                resources.Add(resource);
                notEnough.Add(resource.Find("NotEnough").gameObject);
                count.Add(resource.Find("Count").GetComponent<TextMeshProUGUI>());
                resourceIcon.Add(resource.Find("RecourceIcon").GetComponent<Image>());
            }

            resourceIcon[0].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Gem");
            resourceIcon[1].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Gold");
            resourceIcon[2].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Wood");
            resourceIcon[3].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Stone");
        }

        protected virtual void PaidBuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

        protected virtual void FreeBuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_FreeSpellLearnButton);
            UIManager.ShowScreen<CastleScreen>();
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenLeftShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenLeftHide>();
        }
    }

    public class SpellPopupInData : BasePopupInData
    {

    }
}