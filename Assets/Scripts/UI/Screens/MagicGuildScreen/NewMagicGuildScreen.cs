using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    public class NewMagicGuildScreen : BaseScreen
    {
        private Button activeSpell;
        private Button ultimateSpell;
        private Button passiveSpell_1;
        private Button passiveSpell_2;
        private Button backButton;

        private Image activeSpellUnknown;
        private Image ultimateSpellUnknown;
        private Image passiveSpellUnknown_1;
        private Image passiveSpellUnknown_2;

        private Text mainTitle;

        private void Start()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/MagicGuildScreen/MagicGuild"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            activeSpell = canvas.Find("ActiveSpell").GetComponent<Button>();
            ultimateSpell = canvas.Find("UltimateSpell").GetComponent<Button>();
            passiveSpell_1 = canvas.Find("PassiveSpell1").GetComponent<Button>();
            passiveSpell_2 = canvas.Find("PassiveSpell2").GetComponent<Button>();
            backButton = canvas.Find("BackButton").GetComponent<Button>();

            activeSpellUnknown = activeSpell.transform.Find("UnknownSpell").GetComponent<Image>();
            ultimateSpellUnknown = ultimateSpell.transform.Find("UnknownSpell").GetComponent<Image>();
            passiveSpellUnknown_1 = passiveSpell_1.transform.Find("UnknownSpell").GetComponent<Image>();
            passiveSpellUnknown_2 = passiveSpell_2.transform.Find("UnknownSpell").GetComponent<Image>();

            mainTitle = canvas.Find("Window").Find("MainTitle").GetComponent<Text>();

            backButton.onClick.AddListener(BackButtonClick);
        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }
    }
}