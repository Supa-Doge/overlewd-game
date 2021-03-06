using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.Linq;

namespace Overlewd
{
    public class UIEvent
    {
        public Type type = Type.None;

        public BaseScreen uiSender;
        public System.Type uiSenderType;

        public enum Type
        {
            None,
            RestoreScreenFocusAfterPopup,
            RestoreScreenFocusAfterOverlay
        }
    }

    public class UserInputLocker
    {
        private MonoBehaviour mbLocker;
        private ScreenTransition stLocker;
        private UnityWebRequest uwrLocker;

        public UserInputLocker(MonoBehaviour mbObj)
        {
            mbLocker = mbObj;
        }

        public UserInputLocker(ScreenTransition stObj)
        {
            stLocker = stObj;
        }

        public UserInputLocker(UnityWebRequest uwrObj)
        {
            uwrLocker = uwrObj;
        }

        public bool Equals(UserInputLocker other) 
        {
            return other != null &&
                mbLocker == other.mbLocker &&
                stLocker == other.stLocker &&
                uwrLocker == other.uwrLocker;
        }

        public bool IsNull()
        {
            return mbLocker == null &&
                stLocker == null &&
                uwrLocker == null;
        }
    }

    public static class UIManager
    {
        private static Vector2 currentResolution;
        private static float currentAspectRatio;

        private static GameObject uiRootCanvasGO;
        private static CanvasScaler uiRootCanvasGO_canvasScaler;
        private static RectTransform uiRootCanvasGO_rectTransform;
        private static Canvas uiRootCanvasGO_canvas;
        private static GameObject uiRootScreenLayerGO;
        private static AspectRatioFitter uiRootScreenLayerGO_aspectRatioFitter;

        private static GameObject uiEventSystem;
        private static EventSystem uiEventSystem_eventSystem;
        private static StandaloneInputModule uiEventSystem_standaloneInputModule;
        private static BaseInput uiEventSystem_baseInput;

        private static GameObject uiScreenLayerGO;
        private static GameObject uiPopupLayerGO;
        private static GameObject uiSubPopupLayerGO;
        private static GameObject uiOverlayLayerGO;
        private static GameObject uiNotificationLayerGO;
        private static GameObject uiDialogLayerGO;

        private static BaseFullScreen prevScreen;
        private static BaseFullScreen currentScreen;
        private static BasePopup prevPopup;
        private static BasePopup currentPopup;
        private static BaseSubPopup prevSubPopup;
        private static BaseSubPopup currentSubPopup;
        private static BaseOverlay prevOverlay;
        private static BaseOverlay currentOverlay;
        private static BaseNotification prevNotification;
        private static BaseNotification currentNotification;
        private static GameObject currentDialogBoxGO;

        private static BaseMissclick prevOverlayMissclick;
        private static BaseMissclick overlayMissclick;
        private static BaseMissclick prevPopupMissclick;
        private static BaseMissclick popupMissclick;
        private static BaseMissclick prevSubPopupMissclick;
        private static BaseMissclick subPopupMissclick;
        private static BaseMissclick prevNotificationMissclick;
        private static BaseMissclick notificationMissclick;

        private static List<UserInputLocker> userInputLockers = new List<UserInputLocker>();

        public static void AddUserInputLocker(UserInputLocker locker)
        {
            if (!locker.IsNull())
            {
                if (!userInputLockers.Exists(item => item.Equals(locker)))
                {
                    userInputLockers.Add(locker);
                }
            }

            if (userInputLockers.Count > 0)
            {
                uiEventSystem.SetActive(false);
            }
        }

        public static void RemoveUserInputLocker(UserInputLocker locker)
        {
            if (!locker.IsNull())
            {
                var removeItem = userInputLockers.Find(item => item.Equals(locker));
                if (removeItem != null)
                {
                    userInputLockers.Remove(removeItem);
                }
            }

            if (userInputLockers.Count == 0)
            {
                uiEventSystem.SetActive(true);
            }
        }


        private static Vector2 SelectResolution()
        {
            var aspectRatio = (float)Screen.width / (float)Screen.height;
            var resoulutions = new List<Vector2>()
            {
                new Vector2(1920, 1080),
                new Vector2(2160, 1080),
                new Vector2(2400, 1080)
            };
            return resoulutions.OrderBy(res => Mathf.Abs(res.x / res.y - aspectRatio)).First();
        }

        public static void ChangeResolution()
        {
            currentResolution = SelectResolution();
            currentAspectRatio = currentResolution.x / currentResolution.y;

            uiRootCanvasGO_canvasScaler.referenceResolution = currentResolution;
            uiRootScreenLayerGO_aspectRatioFitter.aspectRatio = currentAspectRatio;
        }

        public static Rect GetScreenWorldRect()
        {
            return uiRootScreenLayerGO.GetComponent<RectTransform>().WorldRect();
        }

        public static float GetScreenScaleFactor()
        {
            return uiRootCanvasGO_canvas.scaleFactor;
        }

        private static void ConfigureRootScreenLayer()
        {
            uiRootScreenLayerGO = new GameObject("UIRootScreenLayer");
            uiRootScreenLayerGO.layer = 5;
            var uiRootScreenLayerGO_rectTransform = uiRootScreenLayerGO.AddComponent<RectTransform>();
            uiRootScreenLayerGO_rectTransform.SetParent(uiRootCanvasGO.transform, false);
            UITools.SetStretch(uiRootScreenLayerGO_rectTransform);

            //resolution fixed aspect
            uiRootScreenLayerGO_aspectRatioFitter = uiRootScreenLayerGO.AddComponent<AspectRatioFitter>();
            uiRootScreenLayerGO_aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            uiRootScreenLayerGO_aspectRatioFitter.aspectRatio = currentAspectRatio;

            //cut screen content outside
            var uiRootScreenLayerGO_rectMask2D = uiRootScreenLayerGO.AddComponent<RectMask2D>();
        }

        private static GameObject ConfigureLayer(string name, int siblingIndex)
        {
            var layerGO = new GameObject(name);
            layerGO.layer = 5;
            var layerGO_rectTransform = layerGO.AddComponent<RectTransform>();
            layerGO_rectTransform.SetParent(uiRootScreenLayerGO.transform, false);
            UITools.SetStretch(layerGO_rectTransform);

            layerGO.transform.SetSiblingIndex(siblingIndex);

            return layerGO;
        }

        //Missclick Instantiate
        private static T GetMissclickInstance<T>(Transform parent) where T : BaseMissclick
        {
            var missclickGO = new GameObject(typeof(T).Name);
            var missclickGO_screenRectTransform = missclickGO.AddComponent<RectTransform>();
            missclickGO_screenRectTransform.SetParent(parent, false);
            missclickGO_screenRectTransform.SetAsFirstSibling();
            UITools.SetStretch(missclickGO_screenRectTransform);
            return missclickGO.AddComponent<T>();
        }

        public static T GetPopupMissclickInstance<T>() where T : PopupMissclick
        {
            return GetMissclickInstance<T>(uiPopupLayerGO.transform);
        }

        public static T GetSubPopupMissclickInstance<T>() where T : SubPopupMissclick
        {
            return GetMissclickInstance<T>(uiSubPopupLayerGO.transform);
        }

        public static T GetOverlayMissclickInstance<T>() where T : OverlayMissclick
        {
            return GetMissclickInstance<T>(uiOverlayLayerGO.transform);
        }

        public static T GetNotificationMissclickInstance<T>() where T : NotificationMissclick
        {
            return GetMissclickInstance<T>(uiNotificationLayerGO.transform);
        }

        //Screen Instantiate
        private static T GetScreenInstance<T>(Transform parent) where T : BaseScreen
        {
            var screenGO = new GameObject(typeof(T).Name);
            screenGO.layer = 5;
            var screenGO_rectTransform = screenGO.AddComponent<RectTransform>();
            var screenGO_rectMask2D = screenGO.AddComponent<RectMask2D>();
            screenGO_rectTransform.SetParent(parent, false);
            UITools.SetStretch(screenGO_rectTransform);
            return screenGO.AddComponent<T>();
        }

        public static T GetScreenInstance<T>() where T : BaseFullScreen
        {
            return GetScreenInstance<T>(uiScreenLayerGO.transform);
        }

        public static T GetPopupInstance<T>() where T : BasePopup
        {
            return GetScreenInstance<T>(uiPopupLayerGO.transform); 
        }

        public static T GetSubPopupInstance<T>() where T : BaseSubPopup
        {
            return GetScreenInstance<T>(uiSubPopupLayerGO.transform);
        }

        public static T GetOverlayInstance<T>() where T : BaseOverlay
        {
            return GetScreenInstance<T>(uiOverlayLayerGO.transform);
        }

        public static T GetNotificationInstance<T>() where T : BaseNotification
        {
            return GetScreenInstance<T>(uiNotificationLayerGO.transform);
        }

        public static void Initialize()
        {
#if UNITY_ANDROID
            Application.targetFrameRate = 60;
#endif

            currentResolution = SelectResolution();
            currentAspectRatio = currentResolution.x / currentResolution.y;

            uiRootCanvasGO = new GameObject("UIManagerRootCanvas");
            uiRootCanvasGO.layer = 5;
            uiRootCanvasGO_rectTransform = uiRootCanvasGO.AddComponent<RectTransform>();
            uiRootCanvasGO_canvas = uiRootCanvasGO.AddComponent<Canvas>();
            uiRootCanvasGO_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            uiRootCanvasGO_canvasScaler = uiRootCanvasGO.AddComponent<CanvasScaler>();
            uiRootCanvasGO_canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            uiRootCanvasGO_canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            uiRootCanvasGO_canvasScaler.referenceResolution = currentResolution;
            var uiRootCanvasGO_graphicRaycaster = uiRootCanvasGO.AddComponent<GraphicRaycaster>();

            ConfigureRootScreenLayer();

            uiScreenLayerGO = ConfigureLayer("UIScreenLayer", 0);
            uiPopupLayerGO = ConfigureLayer("UIPopupLayer", 1);
            uiSubPopupLayerGO = ConfigureLayer("UISubPopupLayer", 2);
            uiOverlayLayerGO = ConfigureLayer("UIOverlayLayer", 3);
            uiNotificationLayerGO = ConfigureLayer("UINotificationLayer", 4);
            uiDialogLayerGO = ConfigureLayer("UIDialogLayer", 5);

            uiEventSystem = new GameObject("UIManagerEventSystem");
            uiEventSystem_eventSystem = uiEventSystem.AddComponent<EventSystem>();
            uiEventSystem_standaloneInputModule = uiEventSystem.AddComponent<StandaloneInputModule>();
            uiEventSystem_baseInput = uiEventSystem.AddComponent<BaseInput>();
        }

        public static void ThrowGameDataEvent(GameDataEvent eventData)
        {
            currentScreen?.OnGameDataEvent(eventData);
            currentPopup?.OnGameDataEvent(eventData);
            currentSubPopup?.OnGameDataEvent(eventData);
            currentOverlay?.OnGameDataEvent(eventData);
        }

        public static void ThrowUIEvent(UIEvent eventData)
        {
            currentScreen?.OnUIEvent(eventData);
            currentPopup?.OnUIEvent(eventData);
            currentSubPopup?.OnUIEvent(eventData);
            currentOverlay?.OnUIEvent(eventData);
        }

        //transition tools
        private static async Task WaitScreensPrepare(List<BaseScreen> screens)
        {
            foreach (var screen in screens)
            {
                var screenTr = screen?.GetTransition();
                if (screenTr != null) await screenTr.PrepareDataAsync();
            }
            foreach (var screen in screens)
            {
                var screenTr = screen?.GetTransition();
                if (screenTr != null) await screenTr.PrepareMakeAsync();
            }
        }

        private static async Task WaitScreenTransitions(List<BaseScreen> screens, List<BaseMissclick> missclicks)
        {
            var prepareTasks = new List<Task>();
            foreach (var screen in screens)
            {
                var screenTr = screen?.GetTransition();
                if (screenTr != null) prepareTasks.Add(screenTr.PrepareAsync());
            }
            await Task.WhenAll(prepareTasks);

            var processTasks = new List<Task>();
            foreach (var screen in screens)
            {
                var screenTr = screen?.GetTransition();
                if (screenTr != null) processTasks.Add(screenTr.ProgressAsync());
            }
            foreach (var missclick in missclicks)
            {
                var missclickTr = missclick?.GetTransition();
                if (missclickTr != null) processTasks.Add(missclickTr.ProgressAsync());
            }
            await Task.WhenAll(processTasks);
        }

        private static void MakeSubPopup(BaseSubPopup subPopup)
        {
            prevSubPopup = currentSubPopup;
            prevSubPopup?.Hide();
            currentSubPopup = subPopup;
            currentSubPopup?.Show();
            currentSubPopup?.MakeMissclick();
            if (currentSubPopup == null)
            {
                prevSubPopupMissclick = subPopupMissclick;
                prevSubPopupMissclick?.Hide();
                subPopupMissclick = null;
            }
        }

        private static void MakePopup(BasePopup popup)
        {
            prevPopup = currentPopup;
            prevPopup?.Hide();
            currentPopup = popup;
            currentPopup?.Show();
            currentPopup?.MakeMissclick();
            if (currentPopup == null)
            {
                prevPopupMissclick = popupMissclick;
                prevPopupMissclick?.Hide();
                popupMissclick = null;
            }
        }

        private static void MakeOverlay(BaseOverlay overlay)
        {
            prevOverlay = currentOverlay;
            prevOverlay?.Hide();
            currentOverlay = overlay;
            currentOverlay?.Show();
            currentOverlay?.MakeMissclick();
            if (currentOverlay == null)
            {
                prevOverlayMissclick = overlayMissclick;
                prevOverlayMissclick?.Hide();
                overlayMissclick = null;
            }
        }

        private static void MakeNotification(BaseNotification notification)
        {
            prevNotification = currentNotification;
            prevNotification?.Hide();
            currentNotification = notification;
            currentNotification?.Show();
            currentNotification?.MakeMissclick();
            if (currentNotification == null)
            {
                prevNotificationMissclick = notificationMissclick;
                prevNotificationMissclick?.Hide();
                notificationMissclick = null;
            }
        }

        private static void MakeScreen(BaseFullScreen screen)
        {
            prevScreen = currentScreen;
            prevScreen?.Hide();
            currentScreen = screen;
            currentScreen.Show();
        }

        //Screen Layer
        public static T GetScreen<T>() where T : BaseFullScreen
        {
            return currentScreen as T;
        }

        public static BaseFullScreenInData currentScreenInData =>
            currentScreen?.baseInputData;
        public static BaseFullScreenInData prevScreenInData =>
            prevScreen?.baseInputData;

        public static bool HasScreen<T>() where T : BaseFullScreen
        {
            return currentScreen?.GetType() == typeof(T);
        }

        public static async void ShowScreenProcess()
        {
            await WaitScreensPrepare(new List<BaseScreen> 
            { 
                prevSubPopup,
                prevPopup,
                prevOverlay,
                prevScreen,
                currentScreen
            });
            await WaitScreenTransitions(new List<BaseScreen> { prevSubPopup },
                                        new List<BaseMissclick> { prevSubPopupMissclick });
            await WaitScreenTransitions(new List<BaseScreen> { prevPopup, prevOverlay, prevScreen },
                                        new List<BaseMissclick> { prevPopupMissclick, prevOverlayMissclick });
            await WaitScreenTransitions(new List<BaseScreen> { currentScreen }, new List<BaseMissclick>());
            MemoryOprimizer.ChangeScreen();
        }

        public static T MakeScreen<T>() where T : BaseFullScreen
        {
            MemoryOprimizer.PrepareChangeScreen();
            MakeSubPopup(null);
            MakePopup(null);
            MakeOverlay(null);
            MakeScreen(GetScreenInstance<T>());
            return currentScreen as T;
        }

        public static T ShowScreen<T>() where T : BaseFullScreen
        {
            MakeScreen<T>();
            ShowScreenProcess();
            return currentScreen as T;
        }

        //Popup Layer
        public static T GetPopupMissclick<T>() where T : PopupMissclick
        {
            return popupMissclick as T;
        }

        public static bool HasPopupMissclick<T>() where T : PopupMissclick
        {
            return popupMissclick?.GetType() == typeof(T);
        }

        public static T MakePopupMissclick<T>() where T : PopupMissclick
        {
            if (!HasPopupMissclick<T>())
            {
                prevPopupMissclick = popupMissclick;
                prevPopupMissclick?.Hide();
                popupMissclick = GetPopupMissclickInstance<T>();
                popupMissclick.Show();
            }
            return popupMissclick as T;
        }

        public static T GetPopup<T>() where T : BasePopup
        {
            return currentPopup as T;
        }

        public static bool HasPopup<T>() where T : BasePopup
        {
            return currentPopup?.GetType() == typeof(T);
        }

        public static async void ShowPopupProcess()
        {
            await WaitScreensPrepare(new List<BaseScreen> 
            {
                prevSubPopup,
                prevPopup,
                currentPopup
            });
            await WaitScreenTransitions(new List<BaseScreen> { prevSubPopup },
                                        new List<BaseMissclick> { prevSubPopupMissclick });
            await WaitScreenTransitions(new List<BaseScreen> { prevPopup }, 
                                        new List<BaseMissclick> { prevPopupMissclick });
            await WaitScreenTransitions(new List<BaseScreen> { currentPopup },
                                        new List<BaseMissclick> { popupMissclick });
        }

        public static T MakePopup<T>() where T : BasePopup
        {
            MakeSubPopup(null);
            MakePopup(GetPopupInstance<T>());
            return currentPopup as T;
        }

        public static T ShowPopup<T>() where T : BasePopup
        {
            MakePopup<T>();
            ShowPopupProcess();
            return currentPopup as T;
        }

        private static async void HidePopupProcess()
        {
            var uiEventData = new UIEvent
            {
                type = UIEvent.Type.RestoreScreenFocusAfterPopup,
                uiSenderType = prevPopup.GetType()
            };

            await WaitScreensPrepare(new List<BaseScreen> 
            {
                prevSubPopup,
                prevPopup
            });
            await WaitScreenTransitions(new List<BaseScreen> { prevSubPopup },
                                        new List<BaseMissclick> { prevSubPopupMissclick });
            await WaitScreenTransitions(new List<BaseScreen> { prevPopup },
                                        new List<BaseMissclick> { prevPopupMissclick });

            ThrowUIEvent(uiEventData);
        }

        public static void HidePopup()
        {
            MakeSubPopup(null);
            MakePopup(null);
            HidePopupProcess();
        }

        //SubPopup Layer
        public static T GetSubPopupMissclick<T>() where T : SubPopupMissclick
        {
            return subPopupMissclick as T;
        }

        public static bool HasSubPopupMissclick<T>() where T : SubPopupMissclick
        {
            return subPopupMissclick?.GetType() == typeof(T);
        }

        public static T MakeSubPopupMissclick<T>() where T : SubPopupMissclick
        {
            if (!HasSubPopupMissclick<T>())
            {
                prevSubPopupMissclick = subPopupMissclick;
                prevSubPopupMissclick?.Hide();
                subPopupMissclick = GetSubPopupMissclickInstance<T>();
                subPopupMissclick.Show();
            }
            return subPopupMissclick as T;
        }

        public static T GetSubPopup<T>() where T : BaseSubPopup
        {
            return currentSubPopup as T;
        }

        public static bool HasSubPopup<T>() where T : BaseSubPopup
        {
            return currentSubPopup?.GetType() == typeof(T);
        }

        public static async void ShowSubPopupProcess()
        {
            await WaitScreensPrepare(new List<BaseScreen> 
            {
                prevSubPopup,
                currentSubPopup
            });
            await WaitScreenTransitions(new List<BaseScreen> { prevSubPopup },
                                        new List<BaseMissclick> { prevSubPopupMissclick });
            await WaitScreenTransitions(new List<BaseScreen> { currentSubPopup },
                                        new List<BaseMissclick> { subPopupMissclick });
        }

        public static T MakeSubPopup<T>() where T : BaseSubPopup
        {
            MakeSubPopup(GetSubPopupInstance<T>());
            return currentSubPopup as T;
        }

        public static T ShowSubPopup<T>() where T : BaseSubPopup
        {
            MakeSubPopup<T>();
            ShowSubPopupProcess();
            return currentSubPopup as T;
        }

        private static async void HideSubPopupProcess()
        {
            await WaitScreensPrepare(new List<BaseScreen> { prevSubPopup });
            await WaitScreenTransitions(new List<BaseScreen> { prevSubPopup },
                                        new List<BaseMissclick> { prevSubPopupMissclick });
        }

        public static void HideSubPopup()
        {
            MakeSubPopup(null);
            HideSubPopupProcess();
        }

        //Overlay Layer
        public static T GetOverlayMissclick<T>() where T : OverlayMissclick
        {
            return overlayMissclick as T;
        }

        public static bool HasOverlayMissclick<T>() where T : OverlayMissclick
        {
            return overlayMissclick?.GetType() == typeof(T);
        }

        public static T MakeOverlayMissclick<T>() where T : OverlayMissclick
        {
            if (!HasOverlayMissclick<T>())
            {
                prevOverlayMissclick = overlayMissclick;
                prevOverlayMissclick?.Hide();
                overlayMissclick = GetOverlayMissclickInstance<T>();
                overlayMissclick.Show();
            }
            return overlayMissclick as T;
        }

        public static T GetOverlay<T>() where T : BaseOverlay
        {
            return currentOverlay as T;
        }

        public static bool HasOverlay<T>() where T : BaseOverlay
        {
            return currentOverlay?.GetType() == typeof(T);
        }

        public static async void ShowOverlayProcess()
        {
            await WaitScreensPrepare(new List<BaseScreen> 
            {
                prevOverlay,
                currentOverlay
            });
            await WaitScreenTransitions(new List<BaseScreen> { prevOverlay },
                                        new List<BaseMissclick> { prevOverlayMissclick });
            await WaitScreenTransitions(new List<BaseScreen> { currentOverlay },
                                        new List<BaseMissclick> { overlayMissclick });
        }

        public static T MakeOverlay<T>() where T : BaseOverlay
        {
            MakeOverlay(GetOverlayInstance<T>());
            return currentOverlay as T;
        }

        public static T ShowOverlay<T>() where T : BaseOverlay
        {
            MakeOverlay<T>();
            ShowOverlayProcess();
            return currentOverlay as T;
        }

        private static async void HideOverlayProcess()
        {
            var uiEventData = new UIEvent
            {
                type = UIEvent.Type.RestoreScreenFocusAfterOverlay,
                uiSenderType = prevOverlay.GetType()
            };

            await WaitScreensPrepare(new List<BaseScreen> { prevOverlay });
            await WaitScreenTransitions(new List<BaseScreen> { prevOverlay },
                                        new List<BaseMissclick> { prevOverlayMissclick });

            ThrowUIEvent(uiEventData);
        }

        public static void HideOverlay()
        {
            MakeOverlay(null);
            HideOverlayProcess();
        }

        //Notification Layer
        public static T GetNotificationMissclick<T>() where T : NotificationMissclick
        {
            return notificationMissclick as T;
        }
        public static bool HasNotificationMissclick<T>() where T : NotificationMissclick
        {
            return notificationMissclick?.GetType() == typeof(T);
        }

        public static T MakeNotificationMissclick<T>() where T : NotificationMissclick
        {
            if (!HasNotificationMissclick<T>())
            {
                prevNotificationMissclick = notificationMissclick;
                prevNotificationMissclick?.Hide();
                notificationMissclick = GetNotificationMissclickInstance<T>();
                notificationMissclick.Show();
            }
            return notificationMissclick as T;
        }

        public static T GetNotification<T>() where T : BaseNotification
        {
            return currentNotification as T;
        }

        public static bool HasNotification<T>() where T : BaseNotification
        {
            return currentNotification?.GetType() == typeof(T);
        }

        public static bool HasAnyNotification()
        {
            return currentNotification != null;
        }

        public static async Task WaitHideNotifications()
        {
            while (currentNotification != null ||
                   prevNotification != null)
            {
                await UniTask.NextFrame();
            }
        }

        public static async void ShowNotificationProcess()
        {
            await WaitScreensPrepare(new List<BaseScreen> 
            {
                prevNotification,
                currentNotification
            });
            await WaitScreenTransitions(new List<BaseScreen> { prevNotification },
                                        new List<BaseMissclick> { prevNotificationMissclick });
            await WaitScreenTransitions(new List<BaseScreen> { currentNotification },
                                        new List<BaseMissclick> { notificationMissclick });
        }

        public static T MakeNotification<T>() where T : BaseNotification
        {
            MakeNotification(GetNotificationInstance<T>());
            return currentNotification as T;
        }

        public static T ShowNotification<T>() where T : BaseNotification
        {
            MakeNotification<T>();
            ShowNotificationProcess();
            return currentNotification as T;
        }

        private static async void HideNotificationProcess()
        {
            await WaitScreensPrepare(new List<BaseScreen> { prevNotification });
            await WaitScreenTransitions(new List<BaseScreen> { prevNotification },
                                        new List<BaseMissclick> { prevNotificationMissclick });
        }

        public static void HideNotification()
        {
            MakeNotification(null);
            HideNotificationProcess();
        }

        //Dialog Layer
        public static void ShowDialogBox(string title, string message, Action yes, Action no = null)
        {
            GameObject.Destroy(currentDialogBoxGO);
            currentDialogBoxGO = new GameObject(typeof(DebugDialogBox).Name);
            currentDialogBoxGO.layer = 5;
            var currentDialogBoxGO_rectTransform = currentDialogBoxGO.AddComponent<RectTransform>();
            currentDialogBoxGO_rectTransform.SetParent(uiDialogLayerGO.transform, false);
            UITools.SetStretch(currentDialogBoxGO_rectTransform);
            var dialogBox = currentDialogBoxGO.AddComponent<DebugDialogBox>();

            dialogBox.title = title;
            dialogBox.message = message;

            dialogBox.yesAction = () =>
            {
                GameObject.Destroy(currentDialogBoxGO);
                currentDialogBoxGO = null;
                yes?.Invoke();
            };

            if (no != null)
            {
                dialogBox.noAction = () =>
                {
                    GameObject.Destroy(currentDialogBoxGO);
                    currentDialogBoxGO = null;
                    no.Invoke();
                };
            }
        }

        public static void HideDialogBox()
        {
            GameObject.Destroy(currentDialogBoxGO);
            currentDialogBoxGO = null;
        }
    }

}
