using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DialogScreen : BaseScreen
    {
        protected Coroutine autoplayCoroutine;

        protected Transform charactersPos;
        protected Transform leftCharacterPos;
        protected Transform midCharacterPos;
        protected Transform rightCharacterPos;

        protected Button textContainer;
        protected TextMeshProUGUI personageName;

        protected Transform emotionBack;
        protected Transform emotionPos;
        protected TextMeshProUGUI text;

        protected Button skipButton;
        protected Button autoplayButton;
        protected Image autoplayButtonPressed;
        protected TextMeshProUGUI autoplayStatus;

        protected GameObject cutIn;
        protected Transform cutInAnimPos;

        protected AdminBRO.Dialog dialogData;
        protected int currentReplicaId;

        protected bool isAutoplayButtonPressed = false;

        private Dictionary<string, NSDialogScreen.DialogCharacter> characters = 
            new Dictionary<string, NSDialogScreen.DialogCharacter>();
        private Dictionary<string, Transform> slots = new Dictionary<string, Transform>();
        private Dictionary<string, string> slot_character = new Dictionary<string, string>();
        private Dictionary<string, string> character_slot = new Dictionary<string, string>();

        protected Dictionary<string, string> dialogCharacterPrefabPath = new Dictionary<string, string>
        {
            [AdminBRO.DialogCharacterSkin.Overlord] = "Prefabs/UI/Screens/DialogScreen/Overlord",
            [AdminBRO.DialogCharacterSkin.Ulvi] = "Prefabs/UI/Screens/DialogScreen/Ulvi",
            [AdminBRO.DialogCharacterSkin.UlviWolf] = "Prefabs/UI/Screens/DialogScreen/UlviFurry",
            [AdminBRO.DialogCharacterSkin.Faye] = "Prefabs/UI/Screens/DialogScreen/Faye",
            [AdminBRO.DialogCharacterSkin.Adriel] = "Prefabs/UI/Screens/DialogScreen/Adriel"
        };

        protected SpineWidgetGroup cutInAnimation;
        protected SpineWidgetGroup emotionAnimation;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/DialogScreen/DialogScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            charactersPos = canvas.Find("CharactersPos");
            leftCharacterPos = charactersPos.Find("LeftCharacterPos");
            midCharacterPos = charactersPos.Find("MidCharacterPos");
            rightCharacterPos = charactersPos.Find("RightCharacterPos");

            textContainer = canvas.Find("TextContainer").GetComponent<Button>();

            
            textContainer.onClick.AddListener(TextContainerButtonClick);

            personageName = canvas.Find("SubstrateName").Find("PersonageName").GetComponent<TextMeshProUGUI>();
            emotionBack = textContainer.transform.Find("EmotionBack");
            emotionPos = emotionBack.Find("EmotionPos");
            text = textContainer.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            skipButton = canvas.Find("SkipButton").GetComponent<Button>();
            skipButton.onClick.AddListener(SkipButtonClick);

            autoplayButton = canvas.Find("AutoplayButton").GetComponent<Button>();
            autoplayButtonPressed = canvas.Find("AutoplayButton").Find("ButtonPressed").GetComponent<Image>();
            autoplayStatus = canvas.Find("AutoplayButton").Find("Status").GetComponent<TextMeshProUGUI>();
            autoplayButton.onClick.AddListener(AutoplayButtonClick);
            autoplayButtonPressed.enabled = false;

            cutIn = canvas.Find("CutIn").gameObject;
            cutInAnimPos = cutIn.transform.Find("AnimPos");
            cutIn.SetActive(false);
        }

        public override async Task BeforeShowAsync()
        {
            await EnterScreen();

            Initialize();
            ShowCurrentReplica();
            AutoplayButtonCustomize();
        }

        protected virtual async Task EnterScreen()
        {
            dialogData = GameData.GetDialogById(GameGlobalStates.dialog_EventStageData.dialogId.Value);
            await GameData.EventStageStartAsync(GameGlobalStates.dialog_EventStageData);
        }

        protected virtual async void LeaveScreen()
        {
            await GameData.EventStageEndAsync(GameGlobalStates.dialog_EventStageData);

            UIManager.ShowScreen<EventMapScreen>();
        }

        private void HideCharacterByName(string keyName)
        {
            if (keyName == null)
                return;
            if (!characters.ContainsKey(keyName))
                return;

            Destroy(characters[keyName].gameObject);
            characters[keyName] = null;
            var keyPos = character_slot[keyName];
            character_slot[keyName] = null;
            if (keyPos != null)
            {
                slot_character[keyPos] = null;
            }
        }

        private void HideCharacterBySlot(string keyPos)
        {
            if (keyPos == null)
                return;
            if (!slots.ContainsKey(keyPos))
                return;

            var keyName = slot_character[keyPos];
            HideCharacterByName(keyName);
        }

        private void ShowCharacter(string keyName, string keyPos)
        {
            if (keyName == null || keyPos == null)
                return;
            if (!characters.ContainsKey(keyName))
                return;
            if (!slots.ContainsKey(keyPos))
                return;

            if (character_slot[keyName] == keyPos)
                return;

            if (slot_character[keyPos] != null)
            {
                HideCharacterBySlot(keyPos);
            }

            if (characters[keyName] == null)
            {
                var slot = slots[keyPos];
                var prefabPath = dialogCharacterPrefabPath[keyName];
                characters[keyName] = NSDialogScreen.DialogCharacter.GetInstance(prefabPath, slot);

                slot_character[keyPos] = keyName;
                character_slot[keyName] = keyPos;
                slot.SetAsLastSibling();
            }
            else
            {
                var curKeyPos = character_slot[keyName];
                slot_character[curKeyPos] = null;
                character_slot[keyName] = null;

                var slot = slots[keyPos];
                characters[keyName].transform.SetParent(slot, false);

                slot_character[keyPos] = keyName;
                character_slot[keyName] = keyPos;
                slot.SetAsLastSibling();
            }
        }

        private void CharacterSelect(string keyName)
        {
            if (keyName == null)
                return;
            if (!characters.ContainsKey(keyName))
                return;

            characters[keyName]?.Select();
        }

        private void CharacterDeselect(string keyName)
        {
            if (keyName == null)
                return;
            if (!characters.ContainsKey(keyName))
                return;

            characters[keyName]?.Deselect();
        }

        protected void Initialize()
        {
            slots[AdminBRO.DialogCharacterPosition.Left] = leftCharacterPos;
            slots[AdminBRO.DialogCharacterPosition.Right] = rightCharacterPos;
            slots[AdminBRO.DialogCharacterPosition.Middle] = midCharacterPos;
            slot_character[AdminBRO.DialogCharacterPosition.Left] = null;
            slot_character[AdminBRO.DialogCharacterPosition.Right] = null;
            slot_character[AdminBRO.DialogCharacterPosition.Middle] = null;

            foreach (var replica in dialogData.replicas)
            {
                var keyName = replica.characterSkin;
                var keyPos = replica.characterPosition;

                bool addKeyName = false;
                if (!characters.ContainsKey(keyName))
                {
                    characters[keyName] = null;
                    character_slot[keyName] = null;
                    addKeyName = true;
                }

                if (characters[keyName] == null)
                {
                    if (keyPos != null && addKeyName) 
                    {
                        if (slot_character[keyPos] == null)
                        {
                            ShowCharacter(keyName, keyPos);
                        }
                    }
                }
            }
        }

        private void ShowCutIn(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
        {
            if (replica.cutInAnimationId.HasValue)
            {
                if (replica.cutInAnimationId != prevReplica?.cutInAnimationId)
                {
                    Destroy(cutInAnimation?.gameObject);
                    cutInAnimation = null;

                    var animation = GetAnimationById(replica.cutInAnimationId.Value);
                    cutInAnimation = SpineWidgetGroup.GetInstance(cutInAnimPos);
                    cutInAnimation.Initialize(animation);
                }
            }
            else
            {
                Destroy(cutInAnimation?.gameObject);
                cutInAnimation = null;
            }
            cutIn.SetActive(cutInAnimation != null);
        }
        
        private void ShowPersEmotion(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
        {
            if (replica.emotionAnimationId.HasValue)
            {
                if (replica.emotionAnimationId != prevReplica?.emotionAnimationId)
                {
                    Destroy(emotionAnimation?.gameObject);
                    emotionAnimation = null;

                    var animation = GetAnimationById(replica.emotionAnimationId.Value);
                    emotionAnimation = SpineWidgetGroup.GetInstance(emotionPos);
                    emotionAnimation.Initialize(animation);
                }
            }
            else
            {
                Destroy(emotionAnimation?.gameObject);
                emotionAnimation = null;
            }
        }
        
        private void AutoplayButtonCustomize()
        {
            if (isAutoplayButtonPressed)
            {
                isAutoplayButtonPressed = true;
                autoplayButtonPressed.enabled = true;
                autoplayStatus.text = "ON";
            }
            else
            {
                isAutoplayButtonPressed = false;
                autoplayButtonPressed.enabled = false;
                autoplayStatus.text = "OFF";
            }
        }

        private void AutoplayButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (isAutoplayButtonPressed == false)
            {
                isAutoplayButtonPressed = true;
                autoplayCoroutine = StartCoroutine(Autoplay());
            }
            else
            {
                isAutoplayButtonPressed = false;
                if (autoplayCoroutine != null)
                {
                    StopCoroutine(autoplayCoroutine);
                }
            }

            AutoplayButtonCustomize();
        }

        private IEnumerator Autoplay()
        {
            while (currentReplicaId < dialogData.replicas.Count)
            {
                ShowCurrentReplica();
                yield return new WaitForSeconds(2f);
                currentReplicaId++;
            }
            AutoplayButtonClick();
        }

        private void SkipButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveScreen();
        }

        private void TextContainerButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_DialogNextButtonClick);
            currentReplicaId++;
            if (currentReplicaId < dialogData.replicas.Count)
            {
                ShowCurrentReplica();
            }
            else
            {
                LeaveScreen();
            }
        }

        private void ShowCurrentReplica()
        {
            var replica = dialogData.replicas[currentReplicaId];
            var prevReplica = (currentReplicaId > 0) ? dialogData.replicas[currentReplicaId - 1] : null;

            if (prevReplica != null)
            {
                var keyName = prevReplica.characterSkin;
                var keyPos = prevReplica.characterPosition;

                if (keyPos == null)
                {
                    HideCharacterByName(keyName);
                }
                else
                {
                    CharacterDeselect(keyName);
                }
            }
            else
            {
                foreach (var ch in characters)
                {
                    ch.Value?.Deselect();
                }
            }


            if (replica != null)
            {
                personageName.text = replica.characterName;
                text.text = replica.message;

                var keyName = replica.characterSkin;
                var keyPos = replica.characterPosition;

                ShowCharacter(keyName, keyPos);
                CharacterSelect(keyName);
            }

            ShowPersEmotion(replica, prevReplica);
            ShowCutIn(replica, prevReplica);
        }

        protected virtual AdminBRO.Animation GetAnimationById(int id)
        {
            return GameData.GetAnimationById(id);
        }
    }
}