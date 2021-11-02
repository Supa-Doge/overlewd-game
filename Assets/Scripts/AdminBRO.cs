using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Overlewd
{
    public static class AdminBRO
    {
        // auth/login; auth/refresh
        public static IEnumerator auth_login(Action success, Action error = null)
        {
            var postData = new WWWForm();
            postData.AddField("deviceId", GetDeviceId());
            yield return NetworkHelper.Post("https://overlude-api.herokuapp.com/auth/login", postData, downloadHandler =>
            {
                tokens = JsonConvert.DeserializeObject<Tokens>(downloadHandler.text);
                success?.Invoke();
            },
            (msg) =>
            {
                error?.Invoke();
            });
        }

        public static IEnumerator auth_refresh(Action success, Action error = null)
        {
            var postData = new WWWForm();
            yield return NetworkHelper.Post("https://overlude-api.herokuapp.com/auth/refresh", postData, downloadHandler =>
            {
                tokens = JsonConvert.DeserializeObject<Tokens>(downloadHandler.text);
                success?.Invoke();
            },
            (msg) =>
            {
                error?.Invoke();
            });
        }

        public static string GetDeviceId()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        [Serializable]
        public class Tokens
        {
            public string accessToken;
            public string refreshToken;
        }

        public static Tokens tokens;

        // GET /me; POST /me
        public static IEnumerator me(Action<PlayerInfo> success, Action error = null)
        {
            yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/me", tokens.accessToken, downloadHandler =>
            {
                var playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(downloadHandler.text);
                success?.Invoke(playerInfo);
            },
            (msg) =>
            {
                error?.Invoke();
            });
        }

        public static IEnumerator me(string name, Action<PlayerInfo> success, Action error = null)
        {
            var formMe = new WWWForm();
            formMe.AddField("name", name);
            yield return NetworkHelper.PostWithToken("https://overlude-api.herokuapp.com/me", formMe, tokens.accessToken, downloadHandler =>
            {
                var playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(downloadHandler.text);
                success?.Invoke(playerInfo);
            },
            (msg) =>
            {
                error?.Invoke();
            });
        }

        [Serializable]
        public class PlayerInfo
        {
            public int id;
            public string name;
            public List<InventoryItem> inventory;
            public List<WalletItem> wallet;
        }

        [Serializable]
        public class InventoryItem
        {
            public int id;
            public string createdAt;
            public string updatedAt;
            public TradableItem tradable;
        }

        [Serializable]
        public class TradableItem
        {
            public int id;
            public string name;
            public string imageUrl;
            public string description;
            public string discount;
            public string specialOfferLabel;
            public string itemPack;
            public string dateStart;
            public string dateEnd;
            public string discountStart;
            public string discountEnd;
            public string sortPriority;
        }


        [Serializable]
        public class WalletItem
        {
            public int id;
            public int amount;
            public string createdAt;
            public string updatedAt;
            public CurrencyItem currency;
        }

        // /markets/{id}
        public static IEnumerator markets(int id, Action<MarketItem> success, Action error = null)
        {
            var url = String.Format("https://overlude-api.herokuapp.com/markets/{0}", id);
            yield return NetworkHelper.GetWithToken(url, tokens.accessToken, (downloadHandler) => 
            {
                var market = JsonConvert.DeserializeObject<MarketItem>(downloadHandler.text);
                success?.Invoke(market);
            },
            (errorMsg) =>
            {
                error?.Invoke();
            });
        }

        [Serializable]
        public class PriceItem
        {
            public int currencyId;
            public int amount;
        }

        [Serializable]
        public class MarketProductItem
        {
            public int id;
            public string name;
            public string imageUrl;
            public string description;
            public List<PriceItem> price;
            public string discount;
            public string specialOfferLabel;
            public string itemPack;
            public string dateStart;
            public string dateEnd;
            public string discountStart;
            public string discountEnd;
            public string sortPriority;
        }

        [Serializable]
        public class MarketItem
        {
            public int id;
            public string name;
            public string description;
            public string bannerImage;
            public string createdAt;
            public string updatedAt;
            public List<MarketProductItem> tradable;
            public List<int> currencies;
        }

        // /currencies
        public static IEnumerator currencies(Action<List<CurrencyItem>> success, Action error = null)
        {
            yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/currencies", tokens.accessToken, (downloadHandler) =>
            {
                var currencies = JsonConvert.DeserializeObject<List<CurrencyItem>>(downloadHandler.text);
                success?.Invoke(currencies);
            },
            (errorMsg) =>
            {
                error?.Invoke();
            });
        }

        [Serializable]
        public class CurrencyItem
        {
            public int id;
            public string name;
            public string iconUrl;
            public string createdAt;
            public string updatedAt;
        }

        // /resources
        public static IEnumerator resources(Action<List<NetworkResource>> success, Action<string> error = null)
        {
            yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/resources", tokens.accessToken, downloadHandler =>
            {
                var resources = JsonConvert.DeserializeObject<ResourcesMetaResponse>(downloadHandler.text);
                success?.Invoke(resources.items);
            },
            (msg) =>
            {
                error?.Invoke(msg);
            });
        }

        [Serializable]
        public class NetworkResource
        {
            public string id;
            public string type;
            public string internalFormat;
            public string hash;
            public int size;
            public string url;
        }

        [Serializable]
        public class ResourcesMetaResponse
        {
            public List<NetworkResource> items;
        }

        // /tradable/{id}/buy
        public static IEnumerator tradable_buy(int tradable_id, Action<TradableBuyStatus> success, Action<string> error = null)
        {
            var form = new WWWForm();
            yield return NetworkHelper.PostWithToken(String.Format("https://overlude-api.herokuapp.com/tradable/{0}/buy", tradable_id), form, tokens.accessToken, downloadHandler =>
            {
                var status = JsonConvert.DeserializeObject<TradableBuyStatus>(downloadHandler.text);
                success?.Invoke(status);
            },
            (msg) =>
            {
                error?.Invoke(msg);
            });
        }

        [Serializable]
        public class TradableBuyStatus
        {
            public bool status;
        }

        // /events
        public static IEnumerator events(Action<List<EventItem>> success, Action<string> error = null)
        {
            yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/events", tokens.accessToken, (downloadHandler) =>
            {
                var events = JsonConvert.DeserializeObject<List<EventItem>>(downloadHandler.text);
                success?.Invoke(events);
            },
            (errorMsg) => {
                error?.Invoke(errorMsg);
            });
        }

        public class EventStageType
        {
            public const string Battle = "battle";
            public const string Boss = "boss";
            public const string Dialog = "dialog";
            public const string SexDialog = "act";
        }

        [Serializable]
        public class EventStageDialogInfo
        {
            public int id;
            public string title;
        }

        [Serializable]
        public class EventStageBattleInfo
        {
            public int id;
            public string title;
            public int? gachaId;
        }

        [Serializable]
        public class EventStageItem
        {
            public int id;
            public string title;
            public string type;
            public List<int> nextStages;
            public EventStageDialogInfo dialog;
            public EventStageBattleInfo battle;
        }

        [Serializable]
        public class EventItem
        {
            public int id;
            public string type;
            public string name;
            public string description;
            public string dateStart;
            public string dateEnd;
            public string backgroundUrl;
            public List<int> currencies;
            public string mapBackgroundUrl;
            public List<int> markets;
            public List<int> quests;
            public string createdAt;
            public string updatedAt;
            public List<EventStageItem> stages;
        }

        // /events/{id}/quests
        public static IEnumerator quests(int eventId, Action<List<QuestItem>> success, Action<string> error = null)
        {
            var url = String.Format("https://overlude-api.herokuapp.com/events/{0}/quests", eventId);
            yield return NetworkHelper.GetWithToken(url, tokens.accessToken, (downloadHandler) =>
            {
                var quests = JsonConvert.DeserializeObject<List<QuestItem>>(downloadHandler.text);
                success?.Invoke(quests);
            },
            (errorMsg) => {
                error?.Invoke(errorMsg);
            });
        }

        [Serializable]
        public class QuestItem
        {
            public int id;
            public string name;
            public string type;
            public string description;
            public int goalCount;
            public string reward;
            public string createdAt;
            public string updatedAt;
        }

        // /i18n
        public static IEnumerator i18n(string locale, Action<List<LocalizationItem>> success, Action<string> error = null)
        {
            var url = String.Format("https://overlude-api.herokuapp.com/i18n?locale={0}", locale);
            yield return NetworkHelper.GetWithToken(url, tokens.accessToken, (downloadHandler) =>
            {
                var dictionary = JsonConvert.DeserializeObject<List<LocalizationItem>>(downloadHandler.text);
                success?.Invoke(dictionary);
            },
            (errorMsg) => {
                error?.Invoke(errorMsg);
            });
        }

        [Serializable]
        public class LocalizationItem
        {
            public int id;
            public string key;
            public string type;
            public string locale;
            public string text;
            public string descripton;
            public string createdAt;
            public string updatedAt;
        }

        // /dialog/{id}
        public static IEnumerator dialog(int id, Action<Dialog> success, Action<string> error = null)
        {
            var url = String.Format("https://overlude-api.herokuapp.com/dialog/{0}", id);
            yield return NetworkHelper.GetWithToken(url, tokens.accessToken, (downloadHandler) =>
            {
                var dialog = JsonConvert.DeserializeObject<Dialog>(downloadHandler.text);
                success?.Invoke(dialog);
            },
            (errorMsg) => {
                error?.Invoke(errorMsg);
            });
        }

        [Serializable]
        public class DialogReplica
        {
            public int id;
            public int sort;
            public string characterName;
            public string message;
            public string sceneOverlayImage;
        }

        [Serializable]
        public class Dialog
        {
            public int id;
            public string title;
            public List<DialogReplica> replicas;
        }

    }
}
