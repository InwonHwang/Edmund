using UnityEngine;
using System.Collections;

public class Stone : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name.CompareTo("모빌") == 0)
        {
            SoundManager.Instance.playSoundEffect("새맞는소리");           
            
            var bird = Utility.Instance.findChild("Stage1", "Background/모형새/모형새/4모형새/모형새");
            bird.GetComponent<Animator>().SetBool("Take", true);

            var flower = Utility.Instance.findChild("Stage1", "Item/꽃");
            flower.SetActive(true);                        
            Vector3 newPos = new Vector3(flower.transform.position.x, -4f, flower.transform.position.z);
            MoveAgent.Instance.MoveTo(flower, newPos, 2);

            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
