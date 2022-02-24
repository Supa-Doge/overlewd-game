using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
	public class BattleManager : MonoBehaviour
	{
		public List<Character> characters;
		public List<CharController> characterControllerList;

		private void Start()
		{
			characters = new List<Character>(Resources.LoadAll<Character>("BattlePersonages"));
			Initialize();
		}

		private void Initialize()
		{
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
		}


	}
}
