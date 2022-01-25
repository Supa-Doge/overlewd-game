using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseScreen : MonoBehaviour
    {
        public virtual void UpdateGameData()
        {

        }

        public virtual void ShowMissclick()
        {

        }

        public virtual ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenTopShow>();
        }

        public virtual ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenTopHide>();
        }

        public virtual async Task BeforeShowAsync()
        {
            await Task.CompletedTask;
        }

        public virtual void StartShow()
        {

        }

        public virtual void AfterShow()
        {

        }

        public virtual async Task BeforeHideAsync()
        {
            await Task.CompletedTask;
        }

        public virtual void StartHide()
        {

        }

        public virtual void AfterHide()
        {

        }

        public bool IsTransitionState()
        {
            return GetComponent<ScreenTransition>() != null;
        }

        public bool IsShowTransitionState()
        {
            return GetComponent<ScreenShow>() != null;
        }

        public bool IsHideTransitionState()
        {
            return GetComponent<ScreenHide>() != null;
        }

        public ScreenTransition GetTransition()
        {
            return GetComponent<ScreenTransition>();
        }
    }
}
