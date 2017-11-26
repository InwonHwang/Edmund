using UnityEngine;
using System.Collections;

public class MoveAgent : Singleton<MoveAgent> {

    public void MoveTo(GameObject gameObj, Vector3 targetPos, float time)
    {
        StartCoroutine(internalMoveTo(gameObj, targetPos, time));
    }

    public void MoveToLocal(GameObject gameObj, Vector3 targetPos, float time)
    {
        StartCoroutine(internalMoveToLocal(gameObj, targetPos, time));
    }

    IEnumerator internalMoveTo(GameObject gameObj, Vector3 targetPos, float time)
    {
        if (gameObj.transform.position == targetPos || getDistance(gameObj.transform.position, targetPos) < 0.05f) yield break;

        float elapsedTime = 0;
        Vector3 startingPos = gameObj.transform.position;

        while (elapsedTime < time)
        {
            if (gameObj.name.CompareTo("Edmund") == 0) AnimationAgent.Instance.setInteger("Edmund", "State", 3);
            gameObj.transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator internalMoveToLocal(GameObject gameObj, Vector3 targetPos, float time)
    {
        if (gameObj.transform.localPosition == targetPos) yield break;

        float elapsedTime = 0;
        Vector3 startingPos = gameObj.transform.localPosition;

        while (elapsedTime < time)
        {
            if (gameObj.name.CompareTo("Edmund") == 0) AnimationAgent.Instance.setInteger("Edmund", "State", 3);
            gameObj.transform.localPosition = Vector3.Lerp(startingPos, targetPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    float getDistance(Vector3 startPos, Vector3 endPos)
    {
        float x = startPos.x - endPos.x;
        float y = startPos.y - endPos.y;

        return Mathf.Sqrt(x * x + y * y);
    }
}
