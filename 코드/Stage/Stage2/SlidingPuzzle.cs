using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlidingPuzzle : MonoBehaviour {
    
    List<GameObject> _pieces;
    Stage _stage;

    GameObject _empty;
    GameObject _parent;
    GameObject _current;
    GameObject _prev;
    GameObject _selected;
    bool doesSelecteMove;
    bool sync;
    int _width;
    int _height;    
    
    void Start() {
        init();
    }

    void Update() {
        if (_stage == null) return;

        if (_stage.DoesAct == false) _stage.DoesAct = true;

        selectPiece();
        movePiece();

        if(check())
        {
            Utility.Instance.delayAction(1f, () => {
                
                Utility.Instance.findChild("Stage2", "Object/2층가로막기").GetComponent<EdgeCollider2D>().enabled = false;
                Utility.Instance.findChild("Stage2", "Object/2층바닥퍼즐").GetComponent<SpriteRenderer>().sprite = ResourcesManager.Instance.sprites["2층바닥퍼즐_후"];
                Utility.Instance.findChild("Stage2", "Object/2층바닥퍼즐").GetComponent<BoxCollider2D>().enabled = false;
                Utility.Instance.delayAction(0.5f, () =>
                {
                    AnimationAgent.Instance.setBool("2층가로막기", "Take", true);
                    _stage.DoesAct = false;
                });
                gameObject.SetActive(false);
                GameObject.Find("Edmund").GetComponent<Edmund>().HitObject = null;
                transform.parent.GetComponent<Canvas>().enabled = false;
            });
            
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {            
            _stage.DoesAct = false;
            gameObject.SetActive(false);
            transform.parent.GetComponent<Canvas>().enabled = false;
        }
    }

    void init()
    {
        sync = true;
        _stage = GameObject.Find("Stage2").GetComponent<Stage>();
        _width = _height = 4;
        _pieces = new List<GameObject>();
        _parent = transform.FindChild("Piece").gameObject;
   
        for (int i = 0; i < _parent.transform.childCount; i++)
        {
            var child = _parent.transform.GetChild(i);
            _pieces.Add(child.gameObject);
            GUIAgent.Instance.registerObject("UI_Stage2/Puzzle/Piece/" + i.ToString());
        }

        _current = _pieces[0];
        _prev = _current;
        GUIAgent.Instance.setSprite(_current, ResourcesManager.Instance.sprites[_current.name + "_selected"]);
        _empty = _pieces[15];       
        setPuzzle();
    }

    void setPuzzle()
    {
        int leftTop = 0;
        int rightTop = _width - 1;
        int leftBottom = _width * _height - _width;
        int rightBottom = _width * _height - 1;

        for (int i = 0;i < 1; ++i)
        {
            int direction = Random.Range(0, 4);
            int index = _pieces.IndexOf(_empty);
            int target = 0;

            if(index == leftTop) // 왼쪽 위
            {
                target = getRandomIndex(index + 1, index + 4);
            }
            else if(index == rightTop) // 오른쪽 위
            {
                target = getRandomIndex(index - 1, index + 4);
            }
            else if (index == leftBottom) // 왼쪽 아래
            {
                target = getRandomIndex(index + 1, index - 4);
            }
            else if (index == rightBottom) // 오른쪽 아래
            {
                target = getRandomIndex(index - 1, index - 4);
            }
            else if (index > leftTop && index < rightTop) //윗줄
            {
                target = getRandomIndex(index - 1, index + 1, index + 4);
            }
            else if (index > leftBottom && index < rightBottom) // 아래줄
            {
                target = getRandomIndex(index - 1, index + 1, index - 4);
            }
            else if(index % _width == 0) //왼쪽줄
            {
                target = getRandomIndex(index + 1, index + 4, index - 4);
            }            
            else if (index % _width == _width -1) //오른쪽 줄
            {
                target = getRandomIndex(index - 1, index + 4, index - 4);
            }
            else
                target = getRandomIndex(index - 1, index + 1,  index + 4, index - 4);


            swap(index, target);
        }
        
    }

    int getRandomIndex(params int[] args)
    {
        int random = Random.Range(0, args.Length);        

        for (int i = 0; i < args.Length; i++)
        {
            if (random == i) return args[i];
        }

        return _width * _height;
    }    

    void swap(int index1, int index2)
    {
        GameObject temp = _pieces[index1];        
        _pieces[index1] = _pieces[index2];        
        _pieces[index2] = temp;        

        Vector3 pos = _pieces[index1].transform.position;
        _pieces[index1].transform.position = _pieces[index2].transform.position;
        _pieces[index2].transform.position = pos;        
    }

    void swap2(int index1, int index2)
    {
        doesSelecteMove = true;

        GameObject temp = _pieces[index1];
        _pieces[index1] = _pieces[index2];
        _pieces[index2] = temp;

        Vector3 pos = _pieces[index1].transform.position;
        Vector2 pos2 = _pieces[index2].transform.position;

        if(_pieces[index1] != _empty) iTween.MoveTo(_pieces[index1], pos2, 0.5f);
        else _pieces[index1].transform.position = pos2;

        if (_pieces[index2] != _empty) iTween.MoveTo(_pieces[index2], pos, 0.5f);
        else _pieces[index2].transform.position = pos;        

        Utility.Instance.delayAction(0.5f, () => { doesSelecteMove = false; });
    }

    void selectPiece()
    {
        if (!sync || _selected) return;

        if (_prev && _prev != _current) GUIAgent.Instance.setSprite(_prev, ResourcesManager.Instance.sprites[_prev.name]);
        
        int index = _pieces.IndexOf(_current);
        int indexOfCurrent = index;
        
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (index + 1 < 16 && _pieces[indexOfCurrent + 1] == _empty) indexOfCurrent += 2;
            else indexOfCurrent++;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (index - 1 > -1 && _pieces[index - 1] == _empty) indexOfCurrent -= 2;
            else indexOfCurrent--;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (index - 4 > - 1 && _pieces[index - 4] == _empty) indexOfCurrent -= 8;
            else indexOfCurrent -= 4 ;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (index + 4 < 16 && _pieces[index + 4] == _empty) indexOfCurrent += 8;
            else indexOfCurrent += 4;
        }

        if (indexOfCurrent > -1 && indexOfCurrent < 16)
        {
            _prev = _current;
            _current = _pieces[indexOfCurrent];
            GUIAgent.Instance.setSprite(_current, ResourcesManager.Instance.sprites[_current.name + "_selected"]);            
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            sync = false;
            Utility.Instance.delayAction(0.05f, () =>
            {
                _selected = _current;
                _current = null;
                _prev = null;
                sync = true;
            });
                      
        }
    }

    void movePiece()
    {
       

        if (!sync ||_current || doesSelecteMove) return;

        int index = _pieces.IndexOf(_selected);
        int indexOfCurrent = index;

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            indexOfCurrent++;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            indexOfCurrent--;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            indexOfCurrent -= 4;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            indexOfCurrent += 4;
        }

        if (indexOfCurrent > -1 && indexOfCurrent < 16 && _pieces[indexOfCurrent] == _empty)
        {
            SoundManager.Instance.playSoundEffect("퍼즐움직이는소리");
            swap2(index, indexOfCurrent);            
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            sync = false;
            Utility.Instance.delayAction(0.05f, () =>
            {
                _current = _selected;
                _prev = _current;
                _selected = null;
                sync = true;
            });
        }        
    }

    bool check()
    {
        for(int i = 0; i < _pieces.Count - 1; i++)
        {
            if (_pieces[i].name.CompareTo((i+1).ToString()) != 0)
                return false;
        }
        return true;
    }
}