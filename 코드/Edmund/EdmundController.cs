using UnityEngine;
using System.Collections;

public class EdmundController : MonoBehaviour
{
    enum Direction : int { LEFT, RIGHT };

    internal UnityEngine.Events.UnityAction Action;
    Rigidbody2D rb2D;

    void Start()
    {
        AnimationAgent.Instance.registerAnimator("Edmund", transform.GetChild(0).GetComponent<Animator>());

        rb2D = GetComponent<Rigidbody2D>();
    }

    internal void move()
    {
        if(Action != null)
        {
            Action();
        }
        else
            internalMove();
    }

    internal void setDirection(int direction)
    {
        internalSetDirection(direction);
    }

    internal void setDirection(Vector2 position)
    {
        if (Mathf.Abs(transform.position.x - position.x) < 0.05f) return;

        if (transform.position.x < position.x)
            setDirection((int)Direction.RIGHT);
        else
            setDirection((int)Direction.LEFT);
    }

    void internalMove()
    {
        if (AnimationAgent.Instance.getInteger("Edmund", "State") == 7) return;

        float horizental = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float amtMove = 5000 * Time.deltaTime;
        rb2D.velocity = Vector2.zero;

        if (horizental != 0 || vertical != 0)
        {
            SoundManager.Instance.playSoundEffectLoop("walk");                      

            if (horizental < 0)
            {
                setDirection((int)Direction.LEFT);
            }
            else if (horizental > 0)
            {
                setDirection((int)Direction.RIGHT);
            }
            
            
           rb2D.AddForce(Vector3.up * amtMove * vertical);
           rb2D.AddForce(Vector3.right * amtMove * horizental);
           AnimationAgent.Instance.setInteger("Edmund", "State", 3);
        }
        else
        {            
            AnimationAgent.Instance.setInteger("Edmund", "State", 0);
            SoundManager.Instance.stopSoundEffect("walk");
        }
    }

    void internalSetDirection(int direction)
    {
        
        Vector3 scale = transform.localScale;
        
        if(direction == (int)Direction.LEFT)
            scale.x = -Mathf.Abs(scale.x);    
        else
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
}
