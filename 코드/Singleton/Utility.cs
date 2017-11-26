using UnityEngine;
using System.Collections;

public class Utility : Singleton<Utility> {

    Coroutine inst = null;

    #region public method

    public GameObject findChild(string parent, string path)
    {
        return internalFindChild(parent, path);
    }

    public void delayAction(float time, UnityEngine.Events.UnityAction action)
    {
        inst = StartCoroutine(internalDelayAction(time, action));
    }

    public void stopDelayAction()
    {
        StopCoroutine(inst);
    }

    #endregion

    #region private method

    GameObject internalFindChild(string parent, string path)
    {
        if (parent == null)
        {
            Debug.LogError("error, null reference");
            return null;
        }
        if (path == null)
            return GameObject.Find(parent);

        var p = GameObject.Find(parent);

        if (!p)
        {
            Debug.LogError("error, null reference: " + parent);
            return null;
        }

        var retValue = p.transform.FindChild(path);


        if (!retValue)
        {
            Debug.Log(parent);
            Debug.LogError("error, null reference: " + path);
            return null;
        }

        return retValue.gameObject;
    }

    IEnumerator internalDelayAction(float time, UnityEngine.Events.UnityAction action)
    {
        if (action == null) yield break;

        yield return new WaitForSeconds(time);

        action();
    }

    public void fade(UnityEngine.UI.Image image, Color color, float speed, UnityEngine.Events.UnityAction action = null)
    {
        inst = StartCoroutine(internalFade(image, color, speed, action));
    }

    public void fade(SpriteRenderer sr, Color color, float speed, UnityEngine.Events.UnityAction action = null)
    {
        inst = StartCoroutine(internalFade(sr, color, speed, action));
    }

    IEnumerator internalFade(UnityEngine.UI.Image image, Color color, float speed, UnityEngine.Events.UnityAction action = null)
    {
        while (image.color != color)
        {
            if (action != null) action();

            Color temp = Color.Lerp(image.color, color, Time.deltaTime * speed);
            image.color = temp;
            yield return 0;
        }
    }

    IEnumerator internalFade(SpriteRenderer sr, Color color, float speed, UnityEngine.Events.UnityAction action = null)
    {       

        while (sr.color != color)
        {
            if (action != null) action();

            Color temp = Color.Lerp(sr.color, color, Time.deltaTime * speed);
            sr.color = temp;
            yield return 0;
        }
    }



    #endregion
}
