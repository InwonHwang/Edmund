using UnityEngine;
using System.Collections;

public abstract class Stage : MonoBehaviour {    

    protected Inventory _inventory;    
    protected Edmund _edmund;
    protected EdmundController _edmundController;
    protected bool[] _skipIntro;
    protected int _numSkipIntro;
    protected int _cur;
    internal bool Intro { get; set; }
    internal bool Clear { get; set; }
    internal bool DoesAct { get; set; }    

    virtual protected void Start()
    {
        _inventory = Utility.Instance.findChild("UI", "inventory").GetComponent<Inventory>();
        _edmund = GameObject.Find("Edmund").GetComponent<Edmund>();
        _edmundController = GameObject.Find("Edmund").GetComponent<EdmundController>();

        var animation = transform.FindChild("Animation");
        for (int i = 0; i < animation.childCount; i++)
        {
            var anim = animation.GetChild(i).GetComponent<Animator>();

            if (anim)
            {
                AnimationAgent.Instance.registerAnimator(anim.gameObject.name, anim);
                anim.gameObject.SetActive(false);
            }
        }

        var obj = transform.FindChild("Object");
        for (int i = 0; i < obj.childCount; i++)
        {
            var child = obj.GetChild(i);
            var anim = child.GetComponent<Animator>();
            if (anim)
                AnimationAgent.Instance.registerAnimator(anim.name, anim);
        }
    }
    virtual internal void action() { StartCoroutine(internalAction()); }

    abstract internal string getName(string name);              //hierarchy 에 있는 아이템이름은 영어, 게임상에 표시할 아이템이름은 한글 영어로부터 한글을 알아낼 필요가 있음.
    abstract protected IEnumerator internalAction();            //각 object 마다 취해야할 애니메이션, 소리 등등
    abstract protected IEnumerator intro();
    internal void skipIntro()
    {
        if(!GUIAgent.Instance.GuiObjects["UI/Menu"].activeSelf)
        {
            _skipIntro[_cur] = true;
            _cur++;
        }        
    }
}
