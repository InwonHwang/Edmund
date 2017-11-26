using UnityEngine;
using System.Collections;

public class MatryoshkaPuzzle2 : MiniGame {

    GameObject[] _matryoshka;
    GameObject _arrow;

    Inventory _inventory;
    Stage _stage;
    int _countOfActivated;
    int _index;    

    void OnEnable()
    {
        if (_matryoshka == null) return;       

        _index = 0;
        while (_index < _matryoshka.Length && _matryoshka[_index].activeSelf) _index++;
        
        _arrow.transform.localPosition = new Vector3(_matryoshka[_index].transform.localPosition.x, _arrow.transform.localPosition.y, 0);
    }
       

    void Start()
    {

        _inventory = Utility.Instance.findChild("UI", "inventory").GetComponent<Inventory>();
        _stage = GameObject.Find("Stage2").GetComponent<Stage>();
        _arrow = transform.FindChild("Arrow").gameObject;
        _countOfActivated = 0;
        _index = 0;
        

        GameObject parent = transform.FindChild("Matryoshka").gameObject;

        _matryoshka = new GameObject[parent.transform.childCount];

        for (int i = 0; i < _matryoshka.Length; i++)
        {
            _matryoshka[i] = parent.transform.GetChild(i).gameObject;
            _matryoshka[i].SetActive(false);
        }
    }

    void Update()
    {
        logic();
    }

    void logic()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            int i = _index + 1;
            while(i < _matryoshka.Length && _matryoshka[i].activeSelf) i++;

            if(i < _matryoshka.Length) _index = i;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            int i = _index - 1;
            while (i > -1 && _matryoshka[i].activeSelf) i--;

            if (i > -1) _index = i;
        }

        _arrow.transform.localPosition = new Vector3(_matryoshka[_index].transform.localPosition.x, _arrow.transform.localPosition.y, 0);

        if(Input.GetKeyUp(KeyCode.Space))
        {
            _countOfActivated++;
            _matryoshka[_index].SetActive(true);
            _matryoshka[_index].GetComponent<UnityEngine.UI.Image>().sprite = ResourcesManager.Instance.sprites["3_" + _inventory.Selected.name];
            _inventory.discardItem(_inventory.Selected.name);
            _inventory.Selected = null;

            if (check() && Clear == false)
            {
                Utility.Instance.delayAction(0.5f, () =>
                {
                    AnimationAgent.Instance.setBool("3층비밀", "Take", true);
                    SoundManager.Instance.playSoundEffect("문열리는소리");
                    Utility.Instance.delayAction(AnimationAgent.Instance.Animators["3층비밀"].GetCurrentAnimatorStateInfo(0).length, () =>
                        SoundManager.Instance.stopSoundEffect("문열리는소리"));
                });
                Utility.Instance.findChild("Stage2", "Object/3층비밀").GetComponent<BoxCollider2D>().enabled = true;
                Clear = true;
            }


            else if (_countOfActivated == 4)
            {
                for (int i = 0; i < 4; i++)
                    _inventory.getItem("matryoshka_doll" + (i + 1).ToString());

                Utility.Instance.delayAction(1f, () =>
                {
                    for (int i = 0; i < 4; i++)
                    {
                        _matryoshka[i].SetActive(false);
                    }
                });

                _countOfActivated = 0;

            }

            Utility.Instance.delayAction(0.5f, () =>
            {
                _stage.DoesAct = false;
                gameObject.SetActive(false);
                transform.parent.GetComponent<Canvas>().enabled = false;
            });
        }
    }

    bool check()
    {
        if (_countOfActivated != 4) return false;

        if (_matryoshka[0].GetComponent<UnityEngine.UI.Image>().sprite.name.CompareTo("3_matryoshka_doll3") != 0) return false;
        if (_matryoshka[1].GetComponent<UnityEngine.UI.Image>().sprite.name.CompareTo("3_matryoshka_doll4") != 0) return false;
        if (_matryoshka[2].GetComponent<UnityEngine.UI.Image>().sprite.name.CompareTo("3_matryoshka_doll2") != 0) return false;
        if (_matryoshka[3].GetComponent<UnityEngine.UI.Image>().sprite.name.CompareTo("3_matryoshka_doll1") != 0) return false;

        return true;
    }
}
