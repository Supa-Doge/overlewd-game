using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class TempBattleScreen : BaseScreen
	{
		private Button backButton;
		/*
		private Transform battleLayer;
		private Transform pers1Pos;
		private Transform pers2Pos;
		*/
		//private SpineWidget pers1;
		//private SpineWidget pers2;

		private GameObject battleManager;

		private

		void Awake()
		{
			var screenInst = ResourceManager.InstantiateScreenPrefab(
				"Prefabs/UI/Screens/BattleScreens/TempBattleScreen/TempBattleScreen", transform);

			var canvas = screenInst.transform.Find("Canvas");
			/*
			battleLayer = canvas.Find("BattleLayer");
			pers1Pos = battleLayer.Find("pers1");
			pers2Pos = battleLayer.Find("enemy1");
			*/
			backButton = canvas.Find("BackButton").GetComponent<Button>();
			backButton.onClick.AddListener(BackButtonClick);
		}

		void Start()
		{

			battleManager = Resources.Load("Prefabs/Battle/BattleManager") as GameObject;
			if (battleManager != null) 
				Instantiate(battleManager, transform);
			else
				Debug.LogError("BattleManager Loading Error");

			//Customize();
		}

		/*
		 private void Customize()
		{
			
			pers1 = SpineWidget.GetInstance(pers1Pos);
            pers1.Initialize("BattlePersonages/BattleCat1/pers/idle0_SkeletonData");
            pers1.PlayAnimation("idle", true);
            pers1.Scale(0.5f);
           
			pers2 = SpineWidget.GetInstance(pers2Pos);
			pers2.Initialize("BattlePersonages/BattleCat2/pers/idle0_SkeletonData");
			pers2.PlayAnimation("Idle", true);
			//pers2.Initialize("BattlePersonages/BattleCat2/pers/defence_SkeletonData");
			//pers2.PlayAnimation("defence", true);
			pers2.Scale(0.5f);
			pers2.FlipX();
			
		}*/

		public override async Task BeforeShowAsync()
		{
			await Task.CompletedTask;
		}

        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_BattleScreenShow);
        }

		public override void AfterShow()
		{

		}

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<StartingScreen>();
        }
    }
}