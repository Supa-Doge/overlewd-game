using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class SexSceneButton : BaseStageButton
        {
            protected const string animPath =
                "Prefabs/UI/Screens/ChapterScreens/FX/StageNew/sex_scene/Idle_SkeletonData";

            protected override void Start()
            {
                base.Start();

                var dialogData = stageData.dialogData;
                title.text = dialogData.title;
                
                if (anim != null)
                {
                    anim.Initialize(animPath);
                    anim.PlayAnimation("action", false);
                }
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.MakeScreen<FTUE.SexScreen>().
                    SetData(new SexScreenInData
                    {
                        ftueStageId = stageId
                    }).RunShowScreenProcess();
            }

            public static SexSceneButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SexSceneButton>
                    ("Prefabs/UI/Screens/ChapterScreens/SexSceneButton", parent);
            }
        }
    }
}