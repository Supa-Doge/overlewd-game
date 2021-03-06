using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public abstract class BaseStageButton : BaseButton
        {
            public int? stageId { get; set; }

            protected AdminBRO.FTUEStageItem stageData =>
                GameData.ftue.info.GetStageById(stageId);


            protected Transform done;
            protected Transform animPos;
            protected SpineWidget anim;

            protected override void Awake()
            {
                base.Awake();
                done = button.transform.Find("Done");
                animPos = canvas.transform.Find("AnimPos");
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                done.gameObject.SetActive(stageData.isComplete);
            }

            protected void SetAnimation(string prefabPath)
            {
                if (stageData.isComplete)
                    return;
                
                button.gameObject.SetActive(false);
                anim = SpineWidget.GetInstance(prefabPath, animPos);
                anim.completeListeners += RemoveAnimation;
                
                if (anim != null)
                    anim.PlayAnimation("action", false);
            }
            
            private void RemoveAnimation()
            {
                button.gameObject.SetActive(true);
                Destroy(anim.gameObject);
            }
        }
    }
}