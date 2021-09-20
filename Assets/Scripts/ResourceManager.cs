using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ResourceManager
{
    private static ResourcesMeta runtimeResourcesMeta = null;
    private static Dictionary<string, Texture2D> loadTextures = new Dictionary<string, Texture2D>();
    private static Dictionary<string, AudioClip> loadSounds = new Dictionary<string, AudioClip>();
    private static Dictionary<string, AssetBundle> loadAssetBundles = new Dictionary<string, AssetBundle>();

    public static string GetRootPath()
    {
        return Caching.currentCacheForWriting.path;
    }

    public static string GetRootFilePath(string fileName)
    {
        return Path.Combine(GetRootPath(), fileName);
    } 

    public static string GetResourcesPath()
    {
        return Path.Combine(GetRootPath(), "Resources");
    }

    public static string GetResourcesFilePath(string fileName)
    {
        return Path.Combine(GetResourcesPath(), fileName);
    }

    public static List<string> GetDirectoryFileNames(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            return new List<string>();
        }

        var directoryFilePaths = Directory.GetFiles(directoryPath);

        var result = new List<string>();
        foreach (var filePath in directoryFilePaths)
        {
            result.Add(Path.GetFileName(filePath));
        }

        return result;
    }

    public static List<string> GetResourcesFileNames()
    {
        return GetDirectoryFileNames(GetResourcesPath());
    }

    public static long GetSpaceFreeBytes()
    {
        return Caching.currentCacheForWriting.spaceFree;
    }

    public static void InitializeCache()
    {
        var cachePath = Path.Combine(Application.persistentDataPath, "OverlewdCache");
        if (!Directory.Exists(cachePath))
        {
            Directory.CreateDirectory(cachePath);
        }

        Cache newCache = Caching.AddCache(cachePath);

        if (newCache.valid)
        {
            Caching.currentCacheForWriting = newCache;
        }

        //Resources directory init
        if (!Directory.Exists(GetResourcesPath()))
        {
            Directory.CreateDirectory(GetResourcesPath());
        }
    }

    public static void WriteText(string filePath, string text)
    {
        File.WriteAllText(filePath, text);
    }

    public static string LoadText(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    private static void WriteBinary(string filePath, byte[] data)
    {
        File.WriteAllBytes(filePath, data);
    }

    public static bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }

    public static void Delete(string filePath)
    {
        File.Delete(filePath);
    }

    /*private static Texture2D LoadTexture(string fileName)
    {
        var filePath = Path.Combine(GetResourcesPath(), fileName);
        var texture = new Texture2D(1, 1);
        var data = File.ReadAllBytes(filePath);
        texture.LoadImage(data, true);
        return texture;
    }

    private static AssetBundle LoadAssetBundle(string fileName)
    {
        var filePath = Path.Combine(GetResourcesPath(), fileName);
        return AssetBundle.LoadFromFile(filePath);
    }*/

    public static IEnumerator LoadTexture(string fileName, Action<Texture2D> doLoad = null)
    {
        if (loadTextures.ContainsKey(fileName))
        {
            doLoad?.Invoke(loadTextures[fileName]);
            yield break;
        }

        var filePath = "file://" + GetResourcesFilePath(fileName);
        using (var request = UnityWebRequestTexture.GetTexture(filePath))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                doLoad?.Invoke(null);
                yield break;
            }
            var texture = DownloadHandlerTexture.GetContent(request);
            loadTextures.Add(fileName, texture);
            doLoad?.Invoke(texture);
        }
    }

    public static IEnumerator LoadTexture(string fileName, string bundleName, Action<Texture2D> doLoad = null)
    {
        var textureKey = Path.Combine(bundleName, fileName);
        if (loadTextures.ContainsKey(textureKey))
        {
            doLoad?.Invoke(loadTextures[textureKey]);
            yield break;
        }

        AssetBundle assetBundle = null;
        yield return LoadAssetBundle(bundleName, (_assetBundle) => { assetBundle = _assetBundle; });

        if (assetBundle == null)
        {
            doLoad?.Invoke(null);
            yield break;
        }

        var request = assetBundle.LoadAssetAsync<Texture2D>(fileName);
        yield return request;

        if (request.asset == null)
        {
            doLoad?.Invoke(null);
            yield break;
        }

        var texture = (Texture2D)(request.asset);
        loadTextures.Add(textureKey, texture);
        doLoad?.Invoke(texture);
    }

    public static IEnumerator LoadAudioClip(string fileName, AudioType audioType, Action<AudioClip> doLoad = null)
    {
        if (loadSounds.ContainsKey(fileName))
        {
            doLoad?.Invoke(loadSounds[fileName]);
            yield break;
        }

        var filePath = "file://" + GetResourcesFilePath(fileName);
        using (var request = UnityWebRequestMultimedia.GetAudioClip(filePath, audioType))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                doLoad?.Invoke(null);
                yield break;
            }
            var sound = DownloadHandlerAudioClip.GetContent(request);
            loadSounds.Add(fileName, sound);
            doLoad?.Invoke(sound);
        }
    }

    public static IEnumerator LoadAudioClip(string fileName, string bundleName, Action<AudioClip> doLoad = null)
    {
        var soundKey = Path.Combine(bundleName, fileName);
        if (loadSounds.ContainsKey(soundKey))
        {
            doLoad?.Invoke(loadSounds[soundKey]);
            yield break;
        }

        AssetBundle assetBundle = null;
        yield return LoadAssetBundle(bundleName, (_assetBundle) => { assetBundle = _assetBundle; });

        if (assetBundle == null)
        {
            doLoad?.Invoke(null);
            yield break;
        }

        var request = assetBundle.LoadAssetAsync<AudioClip>(fileName);
        yield return request;

        if (request.asset == null)
        {
            doLoad?.Invoke(null);
            yield break;
        }

        var sound = (AudioClip)(request.asset);
        loadSounds.Add(soundKey, sound);
        doLoad?.Invoke(sound);
    }

    public static IEnumerator LoadAssetBundle(string fileName, Action<AssetBundle> doLoad = null)
    {
        if (loadAssetBundles.ContainsKey(fileName))
        {
            doLoad?.Invoke(loadAssetBundles[fileName]);
            yield break;
        }

        var filePath = "file://" + GetResourcesFilePath(fileName);
        using (var request = UnityWebRequestAssetBundle.GetAssetBundle(filePath))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                doLoad?.Invoke(null);
                yield break;
            }
            var assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            loadAssetBundles.Add(fileName, assetBundle);
            doLoad?.Invoke(assetBundle);
        }
    }

    public static IEnumerator GetServerResourcesMeta(Action<ResourcesMeta> success)
    {
        yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/resources", NetworkHelper.tokens.accessToken, s =>
        {
            var resources = JsonUtility.FromJson<ResourcesMeta>(s);
            success?.Invoke(resources);
        });
    }

    public static ResourcesMeta GetLocalResourcesMeta()
    {
        var metaFilePath = GetRootFilePath("ResourcesMeta");
        if (Exists(metaFilePath))
        {
            var metaJson = LoadText(metaFilePath);
            var meta = JsonUtility.FromJson<ResourcesMeta>(metaJson);
            return meta;
        }

        return null;
    }

    public static void SaveLocalResourcesMeta(ResourcesMeta meta)
    {
        if (meta != null)
        {
            var metaFilePath = GetRootFilePath("ResourcesMeta");
            var metaJson = JsonUtility.ToJson(meta);
            WriteText(metaFilePath, metaJson);
        }
    }

    public static void InitRuntimeResourcesMeta(ResourcesMeta meta)
    {
        runtimeResourcesMeta = meta;
    }

    public static bool HasFreeSpaceForNewResources(ResourcesMeta serverResourcesMeta)
    {
        var localResourcesMeta = GetLocalResourcesMeta();

        var deleteSize = 0;
        foreach (var localItem in localResourcesMeta.items)
        {
            if (!serverResourcesMeta.items.Exists(serverItem => serverItem.hash == localItem.hash))
            {
                deleteSize += localItem.size;
            }
        }

        var downloadSize = 0;
        foreach (var serverItem in serverResourcesMeta.items)
        {
            if (!localResourcesMeta.items.Exists(localItem => serverItem.hash == localItem.hash))
            {
                downloadSize += serverItem.size;
            }
        }

        return (GetSpaceFreeBytes() + deleteSize - downloadSize) > 0;
    }

    public static IEnumerator ActualizeResources(ResourcesMeta resourcesMeta, Action<NetworkResource> downloadItem, Action done)
    {
        var existingFiles = GetResourcesFileNames();

        //DeleteNotRelevantResources
        foreach (var fileName in existingFiles)
        {
            if (!resourcesMeta.items.Exists(item => item.hash == fileName))
            {
                Delete(GetResourcesFilePath(fileName));
            }
            yield return fileName;
        }

        //DownloadMissingResources
        foreach (var resourceMeta in resourcesMeta.items)
        {
            if (!existingFiles.Exists(item => item == resourceMeta.hash))
            {
                downloadItem?.Invoke(resourceMeta);
                yield return NetworkHelper.LoadTextureFromServer(resourceMeta.url, (tex, data) =>
                {
                    WriteBinary(GetResourcesFilePath(resourceMeta.hash), data);
                });
            }
        }

        done?.Invoke();
    }

    [Serializable]
    public class NetworkResource
    {
        public int id;
        public string type;
        public string internalFormat;
        public string hash;
        public int size;
        public string url;
    }

    [Serializable]
    public class ResourcesMeta
    {
        public List<NetworkResource> items;
    }
}
