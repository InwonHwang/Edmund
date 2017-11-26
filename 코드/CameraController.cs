using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    internal float Boundary { get; set; }
    Transform target;    

    void Start()
    {
        target = GameObject.Find("Edmund").transform;
    }

    void Update()
    {
        followTarget();
    }

    void followTarget()
    {
        if (target == null) return;

        float x = transform.position.x;
        
        if (transform.position.x > Boundary)
        {            
            x = Boundary;            
        }
        else if (transform.position.x < -Boundary)
        {           
            x = -Boundary;
        }
        else if (target.position.x < transform.position.x - 1.5f && transform.position.x != -Boundary)
        {           
            x = Mathf.Lerp(transform.position.x, target.position.x + 1.5f, Time.deltaTime);
        }
        else if (target.transform.position.x > transform.position.x + 1.5f && transform.position.x != Boundary)
        {
            x = Mathf.Lerp(transform.position.x, target.position.x - 1.5f, Time.deltaTime);
        }
        


        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    internal void setTarget(Transform target)
    {
        StartCoroutine(internalSetTarget(target));
    }

    IEnumerator internalSetTarget(Transform target)
    {
        if (target == null) yield break;

        this.target = null;
        MoveAgent.Instance.MoveTo(gameObject, new Vector3(target.position.x, transform.position.y, transform.position.z), 1f);
        yield return new WaitForSeconds(1f);
        this.target = target;

    }
}
