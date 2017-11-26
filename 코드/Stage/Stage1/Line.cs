using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {

    Inventory _inventory;
    LineRenderer lineRenderer;
    GameObject stone;
    GameObject stone1;

    void Start () {
        _inventory = GameObject.Find("UI/inventory").GetComponent<Inventory>();
        lineRenderer = GetComponent<LineRenderer>();
        stone = Utility.Instance.findChild("Edmund", "새총모션/중심/오른팔중심/돌멩이").gameObject;
        stone1 = Utility.Instance.findChild("Edmund", "새총모션/중심/오른팔중심/돌멩이/돌멩이").gameObject;
    }
	
	void Update () {
        if(stone == null) stone = Utility.Instance.findChild("Edmund", "새총모션/중심/오른팔중심/돌멩이").gameObject;
        if (stone1 == null) stone1 = Utility.Instance.findChild("Edmund", "새총모션/중심/오른팔중심/돌멩이/돌멩이").gameObject;
        lineRenderer.SetPosition(0, stone.transform.position + new Vector3(0,0,-5));
        lineRenderer.SetPosition(1, transform.position + new Vector3(0, 0, -5));

        if (Input.GetKeyUp(KeyCode.Space) && GameObject.Find("Edmund/새총모션").activeSelf)    
            StartCoroutine(temp());
    }

    IEnumerator temp()
    {
        if (stone != null)
        {
            var joint = stone.GetComponent<HingeJoint2D>();
            joint.enabled = false;
            stone.GetComponent<Rigidbody2D>().AddForce((transform.position - stone.transform.position) * 500);
        }
        if (stone1 != null)
        {
            stone1.GetComponent<Rigidbody2D>().AddForce((transform.position - stone.transform.position) * 500);
        }

        yield return new WaitForSeconds(1f);

        GameObject.Find("Edmund").GetComponent<EdmundController>().Action = null;
        Utility.Instance.findChild("Edmund", "Base").SetActive(true);
        Utility.Instance.findChild("Edmund", "새총모션").SetActive(false);
        _inventory.Selected = null;
        GUIAgent.Instance.GuiObjects["UI/preview/image_item"].SetActive(false);

        if (stone1.activeSelf)
        {
            _inventory.getItem("sling1");
            _inventory.discardItem("sling2");
        }       
    }
}
