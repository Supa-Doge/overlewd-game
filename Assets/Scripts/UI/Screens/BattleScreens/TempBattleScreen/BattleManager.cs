using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
	public class BattleManager : MonoBehaviour
	{
		public List<Character> characters;
		public List<CharController> characterControllerList;

		//public List<CharacterController> conv;

		private void Start() => Initialize();
		

		private void Initialize()
		{
			characters = new List<Character>(Resources.LoadAll<Character>("BattlePersonages"));
			foreach (var c in characters)
			{
				if (c.battleOrder > 0)
				{
					var charGO = new GameObject("character");
					var cc = charGO.AddComponent<CharController>();
					cc.character = c;
					characterControllerList.Add(cc);
				}
			}
			//characterControllerList.Sort();

		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{
				foreach (var cc in characterControllerList)
				{
					cc.Attack(cc);
				}
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				foreach (var cc in characterControllerList)
				{
					cc.PlayIdle();
				}
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				foreach (var cc in characterControllerList)
				{
					cc.Defence();
				}
			}
		}

		public enum BattleState { ATTACK, DEFENCE, INIT, WIN, LOSE }
		public BattleState battleState = BattleState.INIT;

	}
}
