using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPrepareBattlePopup
    {
        public class EnemyCharacter : BaseCharacter
        {
            protected override void Start()
            {
                Customize();
            }

            protected override void Customize()
            {

            }

            public static EnemyCharacter GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EnemyCharacter>(
                    "Prefabs/UI/Popups/PrepareBattlePopups/EnemyCharacter", parent);
            }
        }
    }
}