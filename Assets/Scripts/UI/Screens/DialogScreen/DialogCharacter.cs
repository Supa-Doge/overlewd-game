using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSDialogScreen
    {
        public class DialogCharacter : MonoBehaviour
        {
            private GameObject persImageShadow;

            void Awake()
            {
                persImageShadow = transform.Find("PersImageShadow").gameObject;
                persImageShadow.SetActive(true);
            }

            public void Select()
            {
                persImageShadow.SetActive(false);
            }

            public void Deselect()
            {
                persImageShadow.SetActive(true);
            }

            public static DialogCharacter GetInstance(string prefabPath, Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<DialogCharacter>(prefabPath, parent);
            }
        }
    }
}
