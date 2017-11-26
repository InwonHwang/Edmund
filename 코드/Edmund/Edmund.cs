using UnityEngine;
using System.Collections;

public class Edmund : MonoBehaviour {

    enum AnimationState : int { IDLE_1, IDLE_2, INVENTORY, WALKING };

    internal GameObject HitObject { get; set; }
    internal GameObject SubEdmund { get; set; }
    GameObject exclamationMark;

    void Start()
    {
        exclamationMark = transform.FindChild("ExclamationMark").gameObject;
                
        exclamationMark.SetActive(false);
    }

    void Update()
    {
        if(!HitObject)
            exclamationMark.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HitObject = other.gameObject;

        if(!HitObject.name.Contains("문"))
            exclamationMark.SetActive(true);
    }


    void OnTriggerExit2D(Collider2D other)
    {
        HitObject = null;       
    }    

    
    IEnumerator idle()
    {
        while (true)
        {
            int animationState = Random.Range((int)AnimationState.IDLE_1, (int)AnimationState.IDLE_2);
            yield return null;
        }
    }
}
