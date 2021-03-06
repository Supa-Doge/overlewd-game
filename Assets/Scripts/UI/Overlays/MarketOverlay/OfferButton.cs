using System;
using System.Collections;
using System.Collections.Generic;
using Overlewd.NSMarketOverlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMarketOverlay
    {
        public class OfferButton : MonoBehaviour
        {
            private TextMeshProUGUI title;
            private TextMeshProUGUI notification;
            private Button button;
            private GameObject buttonSelected;
            private BaseOffer offer;
            public Transform offerPos;
            
            public event Action<OfferButton> selectButton;

            void Awake()
            {
                var canvas = transform.Find("Canvas");
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                buttonSelected = button.transform.Find("Selected").gameObject;
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                notification = button.transform.Find("Notification").GetComponent<TextMeshProUGUI>();
            }

            public void Customize()
            {
                offer = ResourceBundle.GetInstance(offerPos);
                Deselect();
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                selectButton?.Invoke(this);
            }

            public void Select()
            {
                buttonSelected?.SetActive(true);
                offer?.gameObject.SetActive(true);
            }
            
            public void Deselect()
            {
                buttonSelected?.SetActive(false);
                offer?.gameObject.SetActive(false);
            }

            public static OfferButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<OfferButton>(
                    "Prefabs/UI/Overlays/MarketOverlay/OfferButton", parent);
            }
        }
    }
}
