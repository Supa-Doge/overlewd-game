using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
	public class CharController : MonoBehaviour
	{
		public bool isEnemy = false;
		public bool isBoss = false;

		public Character character;
		private float idleScale = .5f, battleScale = .7f;
		[SerializeField] private int battleOrder = -1; //3,2,1 = on the table; -1 = in the deck;
		public int hp = 100;
		private int maxHp = 100;

		public int mp = 100;
		private int maxMp = 100;

		public int baseDamage = 10;
		public int defence = 5;

		public bool isDefence = false;
		private TempBattleScreen tempBattleScene;

		[HideInInspector]
		public bool isDamageBuff = false;
		[HideInInspector]
		public int buffDamageScale = 2;

		private bool isNotBusy = true;
		public bool isDead = false;

		private Transform battleLayer;
		private Transform persPos;
		private Transform battlePos;
		private List<SpineWidget> spineWidgets;
		private float[] aniDuration = { 2f, 0.9f, 0.9f, 1f, 1f, 2f, 2.333f };
		private int aniID = 0;

		private void Start()
		{
			tempBattleScene = FindObjectOfType<TempBattleScreen>();
			spineWidgets = new List<SpineWidget>();
			Init();
		}

		

		private void Init()
		{
			isEnemy = character.isEnemy;
			battleOrder = character.battleOrder;
			idleScale = character.idleScale;
			battleScale = character.battleScale;
			gameObject.AddComponent<RectTransform>();
			UIManager.SetStretch(GetComponent<RectTransform>());

			battlePos = isEnemy ? GameObject.Find("battlePos2").transform : GameObject.Find("battlePos1").transform;
			battleLayer = GameObject.Find("BattleLayer").transform;

			if (isEnemy)
				persPos = battleLayer.Find("enemy" + battleOrder.ToString())?.transform;
			else
				persPos = battleLayer.Find("pers" + battleOrder.ToString())?.transform;

			transform.SetParent(persPos, false);
			//0-6
			var folder = character.folder;
			InsAndAddSWToList(folder + character.ani_idle_path);
			InsAndAddSWToList(folder + character.ani_pAttack_1_path);
			InsAndAddSWToList(folder + character.ani_pAttack_2_path);
			InsAndAddSWToList(folder + character.ani_attack_1_path);
			InsAndAddSWToList(folder + character.ani_attack_2_path);
			InsAndAddSWToList(folder + character.ani_defence_path);
			InsAndAddSWToList(folder + character.ani_difeat_path);

			PlayIdle();

			/*for (int i = 0; i < spineWidgets.Count; i++)
			{
				Debug.Log($"aniDuration {aniDuration[i]}");
			}*/
		}

		private void DurationDataInit() //duration not working
		{
			for (int i = 0; i < spineWidgets.Count; i++)
			{
				//aniDuration[i] = spineWidgets[i].GetDuration();
				Debug.Log($"aniDuration {aniDuration[i]}");
			}
		}

		private void InsAndAddSWToList(string path)
		{
			var sW = SpineWidget.GetInstance(transform);
			sW.Initialize(path);
			sW.Scale(idleScale);
			spineWidgets.Add(sW);
		}

		private void PlayAnimID(int listID, string name, bool loop)
		{
			spineWidgets[listID].PlayAnimation(name, loop);
			foreach (var item in spineWidgets) item.gameObject.SetActive(false);
			spineWidgets[listID].gameObject.SetActive(true);
		}

		public void PlayIdle()
		{
			PlayAnimID(0, character.ani_idle_name, true);
		}
		
		public void PlayDifeat()
		{
			PlayAnimID(7, character.ani_difeat_name, true);
		}

		IEnumerator PlayAttack(int id, CharController target)
		{
			isNotBusy = false;
			PlayAnimID(1 + id, character.ani_pAttack_1_name, false);
			yield return new WaitForSeconds(aniDuration[1] + id);
			PlayAnimID(3 + id, character.ani_attack_1_name, false);
			yield return new WaitForSeconds(aniDuration[3] + id);

			target.Damage(baseDamage * (isDamageBuff ? buffDamageScale : 1));
			isNotBusy = true;
			PlayIdle();
		}
		
		IEnumerator PlayDefence(CharController target)
		{
			yield return new WaitForSeconds(target.aniDuration[1]);
			PlayAnimID(5, character.ani_defence_name, false);
			yield return new WaitForSeconds(aniDuration[5]);
			PlayIdle();
		}

		private void changeScelet()
		{
			//GetComponentInChildren<SkeletonGraphic>();
		}

		public void BattleIn()
		{
			//UI.SetActive(false)
			//SetParentTo Battle Pos
			//MoveTo Battle Pos Vector3.zero
			//Change Scale To BattleScale
		}
		public void BattleOut()
		{
			//Move To battleLayer pos
			//
		}

		public void Attack(CharController target)
		{
			if (isNotBusy)
			{
				StartCoroutine(PlayAttack(0, target));
			}
		}

		public void Defence()
		{
			StartCoroutine(PlayDefence(this));
		}

		public void Damage(int value)
		{
			//play animation damage
			//play hp UI animation
			value = Mathf.Max(value - (isDefence ? defence : 0), 0);
			if (value > 0)
			{
				hp -= value;
				hp = Mathf.Max(hp, 0);
				if (hp <= 0)
				{
					isDead = true;
				}
			}
		}

		public void Heal(int value)
		{
			//play animation heal
			hp += value;
			hp = Mathf.Min(hp, maxHp);
		}
		public void CastMahou(int id)
		{
			//drop spell or skill
		}
	}
}
