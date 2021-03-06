using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace Overlewd
{
    public class LaboratoryScreen : BaseFullScreenParent<LaboratoryScreenInData>
    {
        private const int tabAllUnits = 0;
        private const int tabAssassins = 1;
        private const int tabCasters = 2;
        private const int tabHealers = 3;
        private const int tabBruisers = 4;
        private const int tabTanks = 5;
        private const int tabsCount = 6;

        private int activeTabId;

        private string[] tabNames = { "AllUnits", "Assassins", "Casters", "Healers", "Bruisers", "Tanks" };

        private int[] tabIds = { tabAllUnits, tabAssassins, tabCasters, tabHealers, tabBruisers, tabTanks };
        private Button[] tabs = new Button[tabsCount];
        private GameObject[] pressedTabs = new GameObject[tabsCount];
        private GameObject[] scrollViews = new GameObject[tabsCount];
        private Transform[] scrollContents = new Transform[tabsCount];

        private Button marketButton;
        private Button portalButton;
        private Button backButton;
        
        private Button mergeButton;
        private Image[] mergePriceImage = new Image[2];
        private TextMeshProUGUI[] mergePriceAmount = new TextMeshProUGUI[2];

        private GameObject slotFull;
        private GameObject slotEmpty;
        private GameObject slotAdvanced;
        private GameObject slotEpic;
        private GameObject slotHeroic;
        private Button slotButton;
        private Image girlImage;
        private TextMeshProUGUI girlName;

        private Transform currencyBack;

        private List<NSLaboratoryScreen.Character>  flaskCharacters = new List<NSLaboratoryScreen.Character>();
        
        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/LaboratoryScreen/LaboratoryScreen",
                    transform);
        
            var canvas = screenInst.transform.Find("Canvas");
            var tabsArea = canvas.Find("TabsArea");
            var pressedTabsArea = canvas.Find("PressedTabsArea");
            var charactersBack = canvas.Find("CharactersBack");
            var slot = canvas.Find("Slot");
        
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            
            marketButton = canvas.Find("MarketButton").GetComponent<Button>();
            marketButton.onClick.AddListener(MarketButtonClick);
            
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);
            
            mergeButton = canvas.Find("MergeButton").Find("Button").GetComponent<Button>();
            mergeButton.onClick.AddListener(MergeButtonClick);

            slotEmpty = slot.Find("SlotEmpty").gameObject;
            slotAdvanced = slot.Find("SlotAdvanced").gameObject;
            slotEpic = slot.Find("SlotEpic").gameObject;
            slotHeroic = slot.Find("SlotHeroic").gameObject;

            for (int i = 0; i < inputData?.characterData?.mergePrice?.Count; i++)
            {
                mergePriceImage[i] = mergeButton.transform.Find($"Resource{i + 1}").GetComponent<Image>();
                mergePriceAmount[i] = mergePriceImage[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
            }
            
            slotFull = slot.Find("SlotFull").gameObject;
            girlImage = slotFull.transform.Find("Girl").GetComponent<Image>();
            girlName = slotFull.transform.Find("GirlName").GetComponent<TextMeshProUGUI>();
            currencyBack = canvas.Find("CurrencyBack");
            
            foreach (var i in tabIds)
            {
                tabs[i] = tabsArea.Find(tabNames[i]).GetComponent<Button>();
                tabs[i].onClick.AddListener(() => TabClick(i));
        
                pressedTabs[i] = pressedTabsArea.Find(tabNames[i]).GetComponent<Image>().gameObject;
        
                scrollViews[i] = charactersBack.Find("ScrollView_" + tabNames[i]).gameObject;
                scrollContents[i] = scrollViews[i].transform.Find("Viewport").Find("Content");
            }
        }

        private List<AdminBRO.Character> SortCharacters(List<AdminBRO.Character> characters)
        {
            return characters.Where(ch => ch.characterClass != AdminBRO.Character.Class_Overlord).ToList();
        }

        private void AddChToTab(AdminBRO.Character ch)
        {
            var tabId = ch.characterClass switch
            {
                AdminBRO.Character.Class_Assassin => tabAssassins,
                AdminBRO.Character.Class_Bruiser => tabBruisers,
                AdminBRO.Character.Class_Caster => tabCasters,
                AdminBRO.Character.Class_Healer => tabHealers,
                AdminBRO.Character.Class_Tank => tabTanks,
                _ => tabAllUnits
            };

            var newCh = NSLaboratoryScreen.Character.GetInstance(scrollContents[tabId]);
            newCh.characterId = ch.id;
            newCh.labScreen = this;

            var newChAll = NSLaboratoryScreen.Character.GetInstance(scrollContents[tabAllUnits]);
            newChAll.characterId = ch.id;
            newChAll.labScreen = this;
        }

        private void Customize()
        {
            foreach (var i in tabIds)
            {
                pressedTabs[i].gameObject.SetActive(false);
                scrollViews[i].gameObject.SetActive(false);
            }
            
            var sortCharacters = SortCharacters(GameData.characters.orderByLevel);
            foreach (var ch in sortCharacters)
            {
                AddChToTab(ch);
            }
            
            UITools.FillWallet(currencyBack);
            UpdFlaskState();
        }
        
        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            
            EnterTab(activeTabId);
            
            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (GameData.ftue.stats.lastEndedState)
            {
                case (_, _):
                    switch (GameData.ftue.activeChapter.key)
                    {
                        case "chapter1":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_laboratory);
                            break;
                        case "chapter2":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Reactions_laboratory);
                            break;
                        case "chapter3":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ingie_Reactions_laboratory);
                            break;
                    }
                    break;
            }

            await Task.CompletedTask;
        }

        private void TabClick(int tabId)
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(activeTabId);
            EnterTab(tabId);
        }

        private void EnterTab(int tabId)
        {
            activeTabId = tabId;
            tabs[tabId].gameObject.SetActive(false);
            pressedTabs[tabId].SetActive(true); 
            scrollViews[tabId].SetActive(true);
        }

        private void LeaveTab(int tabId)
        {
            tabs[tabId].gameObject.SetActive(true);
            pressedTabs[tabId].SetActive(false);
            scrollViews[tabId].SetActive(false);
        }

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData.type)
            {
                case GameDataEvent.Type.CharacterMerge:
                    UITools.FillWallet(currencyBack);
                    EraseAllFromFlusk();
                    break;
            }
        }

        public bool IsInFlask(NSLaboratoryScreen.Character ch)
        {
            return flaskCharacters.Exists(item => item.characterId == ch.characterId);
        }

        public bool CanAddToFlask(NSLaboratoryScreen.Character ch)
        {
            if (IsInFlask(ch))
            {
                return false;
            }

            var chData = ch.chracterData;
            if (chData.isHeroic || !chData.isLvlMax)
            {
                return false;
            }

            if (flaskCharacters.Count == 0)
            {
                return true;
            }
            else if (flaskCharacters.Count == 1)
            {
                var fChData = flaskCharacters.First().chracterData;
                return (fChData.key == chData.key &&
                    fChData.level == chData.level);
            }
            return false;
        }

        public bool HasPair()
        {
            var tabChs = scrollContents.SelectMany(tab => tab.GetComponentsInChildren<NSLaboratoryScreen.Character>()).ToList();
            foreach (var tabCh in tabChs)
            {
                if (CanAddToFlask(tabCh))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddToFlask(NSLaboratoryScreen.Character ch)
        {
            flaskCharacters.Add(ch);
            UpdChStates();
            UpdFlaskState();
        }

        private void UpdFlaskState()
        {
            if (flaskCharacters.Count == 0)
            {
                slotEmpty.SetActive(true);
                slotAdvanced.SetActive(false);
                slotEpic.SetActive(false);
                slotHeroic.SetActive(false);
                slotFull.SetActive(false);
                mergeButton.gameObject.SetActive(false);
            }
            else
            {
                var chData = flaskCharacters.First().chracterData;
                if (flaskCharacters.Count == 1)
                {
                    if (HasPair())
                    {
                        slotEmpty.SetActive(chData.isBasic);
                        slotAdvanced.SetActive(chData.isAdvanced);
                        slotEpic.SetActive(chData.isEpic);
                        slotHeroic.SetActive(chData.isHeroic);
                    }
                    else
                    {
                        //TODO: not has pair
                        slotEmpty.SetActive(true);
                        slotAdvanced.SetActive(false);
                        slotEpic.SetActive(false);
                        slotHeroic.SetActive(false);
                    }
                }
                else //2 characters
                {
                    slotEmpty.SetActive(false);
                    slotAdvanced.SetActive(chData.isBasic);
                    slotEpic.SetActive(chData.isAdvanced);
                    slotHeroic.SetActive(chData.isEpic);
                }

                slotFull.SetActive(true);
                girlImage.sprite = ResourceManager.LoadSprite(chData.teamEditSlotPersIcon);
                girlName.text = chData.name;
                mergeButton.gameObject.SetActive(flaskCharacters.Count > 1);
            }
        }

        public void EraseFromFlask(NSLaboratoryScreen.Character ch)
        {
            flaskCharacters.RemoveAll(item => item.characterId == ch.characterId);
            UpdChStates();
            UpdFlaskState();
        }

        public void EraseAllFromFlusk()
        {
            flaskCharacters.Clear();
            UpdChStates();
            UpdFlaskState();
        }

        private void UpdChStates()
        {
            var actualChs = SortCharacters(GameData.characters.orderByLevel);
            var tabsChs = scrollContents.SelectMany(tab => tab.GetComponentsInChildren<NSLaboratoryScreen.Character>()).ToList();
            var removedChs = tabsChs.Where(tCh => !actualChs.Exists(aCh => aCh.id == tCh.characterId));
            var addChs = actualChs.Where(aCh => !tabsChs.Exists(tCh => tCh.characterId == aCh.id));
            
            foreach (var rCh in removedChs)
            {
                DestroyImmediate(rCh.gameObject);
            }

            foreach (var nCh in addChs)
            {
                AddChToTab(nCh);
            }

            var actTabsChs = scrollContents.SelectMany(tab => tab.GetComponentsInChildren<NSLaboratoryScreen.Character>()).ToList();
            foreach (var ch in actTabsChs)
            {
                ch.Customize();
            }
        }

        private void PortalButtonClick()
        {
            UIManager.ShowScreen<PortalScreen>();
        }

        private void MarketButtonClick()
        {
            UIManager.ShowOverlay<MarketOverlay>();
        }

        private async void MergeButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            var ch1 = flaskCharacters.First();
            var ch2 = flaskCharacters.Last();
            await GameData.characters.Mrg(ch1.characterId, ch2.characterId);
        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }
        
    }

    public class LaboratoryScreenInData : BaseFullScreenInData
    {
        public int? characterId;
        public AdminBRO.Character characterData => GameData.characters.GetById(characterId);
    }
}
