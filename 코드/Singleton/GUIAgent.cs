using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class GUIAgent : Singleton<GUIAgent>
{
    Dictionary<string, GameObject> guiObjects = new Dictionary<string, GameObject>();

    #region public method

    public Dictionary<string, GameObject> GuiObjects { get { return guiObjects; } }

    public void registerObject(string parent, string path)
    {
        internalRegisterObject(parent, path);
    }

    public void registerObject(string name)
    {
        internalRegisterObject(name);
    }

    public void unregisterObject(string parent, string path)
    {
        internalUnregisterObject(parent, path);
    }

    public void unregisterObject(string name)
    {
        internalUnregisterObject(name);
    }  
              
    public void setSprite(string parent, string path, Sprite sprite)
    {
        var key = parent + "/" + path;

        internalSetSprite(key, sprite);
    }

    public void setSprite(string key, Sprite sprite)
    {
        internalSetSprite(key, sprite);
    }

    public void setText(string parent, string path, string text)
    {
        var key = parent + "/" + path;

        guiObjects[key].GetComponent<Text>().text = text;
    }

    public void setText(string key, string text)
    {
        guiObjects[key].GetComponent<Text>().text = text;
    }

    public void setEnabled(string parent, string path, bool value)
    {
        var key = parent + "/" + path;

        internalSetEnabled(key, value);
    }

    public void setEnabled(string key, bool value)
    {
        internalSetEnabled(key, value);
    }

    public void delaySetEnabled(string parent, string path, bool value, float time = 0)
    {
        var key = parent + "/" + path;

        internalDelaySetEnabled(key, value, time);
    }

    public void delaySetEnabled(string key, bool value, float time = 0)
    {
        internalDelaySetEnabled(key, value, time);
    }

    public void setButtonSpriteState(string parent, string path, Sprite highlighted, Sprite pressed, Sprite disabled)
    {
        var key = parent + "/" + path;

        internalSetSpriteState(key, highlighted, pressed, disabled);
    }

    public void setButtonSpriteState(string key, Sprite highlighted, Sprite pressed, Sprite disabled)
    {
        internalSetSpriteState(key, highlighted, pressed, disabled);
    }

    public void addListener(string parent, string path, UnityAction call)
    {
        var key = parent + "/" + path;
        internalAddListener(key, call);
    }

    public void addListener(string key, UnityAction call)
    {
        internalAddListener(key, call);
    }

    public void setRaycastTarget(string parent, string path, bool value)
    {
        var key = parent + "/" + path;

        internalSetRaycastTarget(key, value);
    }

    public void setRaycastTarget(string name, bool value)
    {
        internalSetRaycastTarget(name, value);
    }

    public GameObject createSub(GameObject prefab, Vector2 anchoredPosition, Sprite sprite, string name, GameObject parent, bool active)
    {
        return internalCreateSub(prefab, anchoredPosition, sprite, name, parent, active);
    }


    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////
   
    public void setSprite(GameObject gameobj, Sprite sprite)
    {
        internalSetSprite(gameobj, sprite);
    }
    
    public void setText(GameObject gameobj, string text)
    {
        internalSetText(gameobj, text);
    }
   

    public void setEnabled(GameObject gameobj, bool value)
    {
        internalSetEnabled(gameobj, value);
    }
  

    public void addListener(GameObject gameobj, UnityAction call)
    {
        internalAddListener(gameobj, call);
    }

    public void setRaycastTarget(GameObject gameobj, bool value)
    {
        internalSetRaycastTarget(gameobj, value);
    }

    #endregion public mathod

    #region private method

    void internalRegisterObject(string parent, string path)
    {
        if (parent == null || path == null) return;

        string key = parent + "/" + path;

        var guiObject = Utility.Instance.findChild(parent, path);

        if (guiObjects.ContainsKey(key))
        {
            Debug.Log(key + " already exists");
            return;
        }

        guiObjects.Add(key, guiObject);
    }

    void internalRegisterObject(string name)
    {
        if (name == null) return;
                        
        var guiObject = GameObject.Find(name);

        if (guiObjects.ContainsKey(name))
        {
            Debug.Log("already exists");
            return;
        }

        guiObjects.Add(name, guiObject);
    }

    void internalUnregisterObject(string parent, string path)
    {
        if (parent == null || path == null) return;

        string key = parent + "/" + path;

        if (!guiObjects.ContainsKey(key))
        {
            Debug.Log("does not exists");
            return;
        }

        guiObjects.Remove(key);
    }

    void internalUnregisterObject(string name)
    {
        if (name == null) return;

        if (!guiObjects.ContainsKey(name))
        {
            Debug.Log("does not exists");
            return;
        }

        guiObjects.Remove(name);
    }

    void internalSetSprite(string key, Sprite sprite)
    {
        if (!guiObjects.ContainsKey(key)) return;
        guiObjects[key].GetComponent<Image>().sprite = sprite;
    }

    void internalSetText(string key, string text)
    {
        if (!guiObjects.ContainsKey(key)) return;

        guiObjects[key].GetComponent<Text>().text = text;
    }

    void internalSetEnabled(string key, bool value)
    {
        if (!guiObjects.ContainsKey(key)) return;

        if (guiObjects[key].GetComponent<Canvas>())
            guiObjects[key].GetComponent<Canvas>().enabled = value;
    }

    IEnumerator internalDelaySetEnabled(string key, bool value, float time = 0)
    {
        yield return new WaitForSeconds(time);

        if (!guiObjects.ContainsKey(key)) yield break;

        if (guiObjects[key].GetComponent<Canvas>())
            guiObjects[key].GetComponent<Canvas>().enabled = value;
    }

    void internalSetSpriteState(string key, Sprite highlighted, Sprite pressed, Sprite disabled)
    {
        if (!guiObjects.ContainsKey(key) || !guiObjects[key].GetComponent<Button>()) return;            

        var spriteState = new SpriteState();
        spriteState.highlightedSprite = highlighted;
        spriteState.pressedSprite = pressed;
        spriteState.disabledSprite = disabled;

        if (guiObjects[key].GetComponent<Button>())
            guiObjects[key].GetComponent<Button>().spriteState = spriteState;
    }

    void internalAddListener(string key, UnityAction call)
    {
            
        if (!guiObjects.ContainsKey(key) || !guiObjects[key].GetComponent<Button>()) return;

        guiObjects[key].GetComponent<Button>().onClick.AddListener(call);
    }

    GameObject internalCreateSub(GameObject prefab, Vector2 anchoredPosition, Sprite sprite, string name, GameObject parent, bool active)
    {
        if (!parent) return null;

        var sub = Instantiate<GameObject>(prefab);
        if (sub)
        {
            sub.transform.SetParent(parent.transform, false);
            sub.GetComponent<Image>().sprite = sprite;
            sub.GetComponent<Image>().SetNativeSize();
            sub.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
            sub.SetActive(active);
            sub.name = name;
        }

        return sub;
    }

    void internalSetRaycastTarget(string key, bool value)
    {
        if (!guiObjects.ContainsKey(key) || !guiObjects[key].GetComponent<Button>()) return;

        guiObjects[key].GetComponent<Image>().raycastTarget = value;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void internalSetSprite(GameObject gameobj, Sprite sprite)
    {
        if (!guiObjects.ContainsValue(gameobj)) return;
        gameobj.GetComponent<Image>().sprite = sprite;
    }

    void internalSetText(GameObject gameobj, string text)
    {
        if (!guiObjects.ContainsValue(gameobj)) return;

        gameobj.GetComponent<Text>().text = text;
    }

    void internalSetEnabled(GameObject gameobj, bool value)
    {
        if (!guiObjects.ContainsValue(gameobj)) return;

        if (gameobj.GetComponent<Canvas>())
            gameobj.GetComponent<Canvas>().enabled = value;
    }

    void internalAddListener(GameObject gameobj, UnityAction call)
    {

        if (!guiObjects.ContainsValue(gameobj) || !gameobj.GetComponent<Button>()) return;

        gameobj.GetComponent<Button>().onClick.AddListener(call);
    }
   

    void internalSetRaycastTarget(GameObject gameobj, bool value)
    {
        if (!guiObjects.ContainsValue(gameobj) || !gameobj.GetComponent<Image>()) return;

        gameobj.GetComponent<Image>().raycastTarget = value;
    }

    #endregion private method
}   //class GUIObject

