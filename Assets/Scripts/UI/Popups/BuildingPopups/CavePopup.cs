using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CavePopup : BuildingPopup
    {

        protected override void Awake()
        {
            base.Awake();
            Instantiate(Resources.Load("Prefabs/UI/Popups/BuildingPopups/CaveImage"), imageSpawnPoint);
        }

    }

}
