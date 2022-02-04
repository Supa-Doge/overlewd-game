using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DebugScreenViewer : BaseScreen
    {
        
        void Awake()
        {
            var screenPrefab = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/SummoningScreen/SummoningScreen", transform);
        }

        void Start()
        {
            UIManager.ShowScreen<FTUE.PortalScreen>();
        }

        void OnGUI()
        {
            float offset = Screen.width * 0.05f;
            float size = Screen.width * 0.1f;
            var rect = new Rect(offset, offset, size, size);
            if (GUI.Button(rect, "Castle")) {
                UIManager.ShowScreen<CastleScreen>();
            }
        }
    }
}
