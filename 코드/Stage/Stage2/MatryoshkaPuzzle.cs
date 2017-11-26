using UnityEngine;
using System.Collections;

public class MatryoshkaPuzzle : MonoBehaviour {

    internal bool Clear { get; set; }

    GameObject[] _matryoshka;
    GameObject _arrow;
    GameObject _selected;
    Stage _stage;
    int _index;
    int _cur;


    void Start()
    {
        _stage = GameObject.Find("Stage2").GetComponent<Stage>();
        _matryoshka = new GameObject[3];
        _matryoshka[0] = transform.FindChild("Panel/Top").gameObject;
        _matryoshka[1] = transform.FindChild("Panel/Mid").gameObject;
        _matryoshka[2] = transform.FindChild("Panel/Bottom").gameObject;
        _arrow = transform.FindChild("Arrow").gameObject;

        _index = 0;
        _cur = 0;
        _selected = _matryoshka[0];
    }

    void Update () {
        if (_stage != null && _stage.DoesAct == false) _stage.DoesAct = true;

        if(!Clear) selectedPart();
        if (!Clear) changeSprite();

        if (Input.GetKeyUp(KeyCode.Escape))
        {            
            _stage.DoesAct = false;
            gameObject.SetActive(false);
            transform.parent.GetComponent<Canvas>().enabled = false;
        }

        if(Input.GetKeyUp(KeyCode.Space) && check() && Clear == false)
        {
            Clear = true;
            Utility.Instance.delayAction(1f, () =>
            {
                gameObject.SetActive(false);
                transform.parent.GetComponent<Canvas>().enabled = false;
                Utility.Instance.findChild("UI", "inventory").GetComponent<Inventory>().discardItem("matryoshka_doll3");
                Utility.Instance.findChild("UI", "inventory").GetComponent<Inventory>().getItem("matryoshka_doll3_used");                
                Utility.Instance.findChild("UI", "inventory").GetComponent<Inventory>().getItem("matryoshka_doll4");
                Utility.Instance.findChild("UI", "inventory").GetComponent<Inventory>().Selected = null;
                Utility.Instance.findChild("UI", "inventory").GetComponent<Inventory>().openInventory();                
                _stage.DoesAct = false;
                gameObject.SetActive(false);
                transform.parent.GetComponent<Canvas>().enabled = false;
                GUIAgent.Instance.GuiObjects["UI/preview/image_item"].SetActive(false);   
                                             
            });            
        }
    }

    void changeSprite()
    {
        int prev = _cur;    
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            while(prev == _cur)
                _cur = Random.Range(0, 3) + 1;
            SoundManager.Instance.playSoundEffect("마트료시카움직이는");
            _selected.GetComponent<UnityEngine.UI.Image>().sprite = ResourcesManager.Instance.sprites[_selected.name.ToLower() + "_" + _cur.ToString()];
        }
        
    }

    void selectedPart()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && _index > 0)
        {
            _index--;
            _selected = _matryoshka[_index];            
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && _index < 2)
        {
            _index++;
            _selected = _matryoshka[_index];            
        }

        if(_selected != null)
            _arrow.transform.localPosition = new Vector3(_arrow.transform.localPosition.x, _selected.transform.localPosition.y, 0);        
    }

    bool check()
    {
        bool ret = true;

        if (_matryoshka[0].GetComponent<UnityEngine.UI.Image>().sprite.name.CompareTo("top_3") != 0) ret = false;
        if (_matryoshka[1].GetComponent<UnityEngine.UI.Image>().sprite.name.CompareTo("mid_3") != 0) ret = false;
        if (_matryoshka[2].GetComponent<UnityEngine.UI.Image>().sprite.name.CompareTo("bottom_3") != 0) ret = false;        

        return ret;
    }
}
