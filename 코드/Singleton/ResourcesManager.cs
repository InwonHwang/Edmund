using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ResourcesManager : Singleton<ResourcesManager>
{
    public Dictionary<string, Sprite> sprites { get; private set; }
    public Dictionary<string, GameObject> prefabs { get; private set; }
    public Dictionary<string, AudioClip> audioClips { get; private set; }

    private ResourcesManager()
    {
        sprites = new Dictionary<string, Sprite>();
        prefabs = new Dictionary<string, GameObject>();
        audioClips = new Dictionary<string, AudioClip>();
        setDictionary<Sprite>(sprites, "Images");
        setDictionary<GameObject>(prefabs, "Prefabs");
        setDictionary<AudioClip>(audioClips, "Sounds");
    }

    private void setDictionary<T>(Dictionary<string, T> dictionary,string path) where T : Object
    {
        var sprites = Resources.LoadAll<T>(path) as T[];
        if (sprites == null) return;

        for (int i = 0; i < sprites.Length; i++)
        {
            if (dictionary.ContainsKey(sprites[i].name)) continue;

            dictionary.Add(sprites[i].name, sprites[i]);
        }
    }

    public int getCount<T>(Dictionary<string, T> dictionary, string contains, string trim = null) where T : Object
    {
        int retValue = 0;

        foreach (var key in dictionary.Keys)
        {
            if ((key.Contains(contains) && trim == null) ||

                (key.Contains(contains) && !key.Contains(trim)))
                retValue++;
        }

        return retValue;
    }

    public T[] getValues<T>(Dictionary<string, T> dictionary, string contains, string trim = null) where T : Object
    {
        int count = getCount<T>(dictionary, contains, trim);
        var retValue = new T[count];
        int i = 0;

        foreach (var prefab in dictionary.Values)
        {
            if ((prefab.name.Contains(contains) && trim == null) ||
                (prefab.name.Contains(contains) && !prefab.name.Contains(trim)))
            {
                retValue[i] = prefab;
                i++;
            }
        }

        return retValue;
    }        
}