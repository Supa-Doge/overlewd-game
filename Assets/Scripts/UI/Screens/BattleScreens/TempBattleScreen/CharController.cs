using Spine;
using UnityEngine;

namespace Overlewd
{
	public class CharController : MonoBehaviour
	{
		public bool isEnemy = false;
		public bool isBoss = false;

		public Character character;

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

		public bool isDead = false;


		private Transform battleLayer;
		private Transform persPos;
		private Transform battlePos;
		private SpineWidget spineWiget;


		private void Start()
		{
			tempBattleScene = FindObjectOfType<TempBattleScreen>();
			Init();
		}

		private void Init()
		{
			isEnemy = character.isEnemy;
			battleOrder = character.battleOrder;

			gameObject.AddComponent<RectTransform>();
			UIManager.SetStretch(GetComponent<RectTransform>());

			battlePos = isEnemy ? GameObject.Find("battlePos2").transform : GameObject.Find("battlePos1").transform;
			battleLayer = GameObject.Find("BattleLayer").transform;

			if (isEnemy)
				persPos = battleLayer.Find("enemy" + battleOrder.ToString())?.transform;
			else
				persPos = battleLayer.Find("pers" + battleOrder.ToString())?.transform;

			transform.SetParent(persPos, false);
			spineWiget = SpineWidget.GetInstance(transform);
			PlayIdle();
			spineWiget.Scale(0.5f);
		}

		private void _PlayAnimation(string path, string name, bool loop)
		{
			spineWiget.Initialize(path);
			spineWiget.PlayAnimation(name, loop);
		}

		public void PlayIdle()
		{
			_PlayAnimation(character.folder + character.ani_idle_path, character.ani_idle_name, true);
		}

		public void PlayAttack()
		{
			_PlayAnimation(character.folder + character.ani_attack_1_path, character.ani_attack_1_name, false);
		}


		private void changeScelet()
		{
			GetComponentInChildren<SkeletonGraphic>();
		}

		public void Attack(CharController target)
		{
			//play animation preAttack -> ...
			target.Damage(baseDamage * (isDamageBuff ? buffDamageScale : 1));
			//... -> attack -> idle;
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
