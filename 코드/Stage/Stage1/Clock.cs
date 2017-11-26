using UnityEngine;
using System.Collections;

public class Clock : MiniGame {

    GameObject _selected;
    GameObject _minuteHand;
    GameObject _hourHand;
    Inventory _inventory;
    Stage _stage;
	
	void Start () {
        Active = true;
        _inventory = Utility.Instance.findChild("UI", "inventory").GetComponent<Inventory>();
        _stage = GameObject.Find("Stage1").GetComponent<Stage1>();        
        _minuteHand = transform.FindChild("minute_hand").gameObject;
        _hourHand = transform.FindChild("hour_hand").gameObject;

        if (!GUIAgent.Instance.GuiObjects.ContainsValue(_minuteHand))
            GUIAgent.Instance.registerObject("UI_Stage1/Clock/minute_hand");

        if (!GUIAgent.Instance.GuiObjects.ContainsValue(_hourHand))
            GUIAgent.Instance.registerObject("UI_Stage1/Clock/hour_hand");

        _minuteHand.SetActive(false);
        _selected = _hourHand;
    }

    void Update()
    {
        if (_stage != null && _stage.DoesAct == false) _stage.DoesAct = true;

        selectHandOfClock();
        rotateHandOfClock();
        check();

        if (Input.GetKeyUp(KeyCode.Escape))
        {            
            GameObject.Find("UI_Stage1").GetComponent<Canvas>().enabled = false;
            gameObject.SetActive(false);
            Active = false;
            _stage.DoesAct = false;                
        }

        if (!_minuteHand.activeSelf && _inventory.Selected && _inventory.Selected.name.Contains("minute_hand") && Input.GetKeyUp(KeyCode.Space))
        {
            _minuteHand.SetActive(true);
            SoundManager.Instance.playSoundEffect("시계");            
            Utility.Instance.findChild("Stage1", "Object/시계").GetComponent<SpriteRenderer>().sprite = ResourcesManager.Instance.sprites["시계_0"];            
            Utility.Instance.findChild("UI", "inventory").GetComponent<Inventory>().discardItem("minute_hand");
        }
        
    }

    void selectHandOfClock()
    {
        if (!_minuteHand.activeSelf) return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            GUIAgent.Instance.setSprite(_hourHand, ResourcesManager.Instance.sprites["hour_hand_"]);
            GUIAgent.Instance.setSprite(_minuteHand, ResourcesManager.Instance.sprites["minute_hand1"]);
            _selected = _hourHand;
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            GUIAgent.Instance.setSprite(_hourHand, ResourcesManager.Instance.sprites["hour_hand"]);
            GUIAgent.Instance.setSprite(_minuteHand, ResourcesManager.Instance.sprites["minute_hand_"]);
            _selected = _minuteHand;
        }
    }

    void check()
    {
        if (!_minuteHand.activeSelf) return;

        if (Input.GetKeyUp(KeyCode.Space))
        {

            if (_minuteHand.transform.eulerAngles.z > 200 &&
                _minuteHand.transform.eulerAngles.z < 220 &&
                _hourHand.transform.eulerAngles.z > 80 &&
                _hourHand.transform.eulerAngles.z < 100)
            {
                Utility.Instance.findChild("Stage1", "Object/시계").GetComponent<Animator>().enabled = true;
                Utility.Instance.delayAction(0.5f, () => {                    
                    GameObject.Find("UI_Stage1").GetComponent<Canvas>().enabled = false;
                    gameObject.SetActive(false);
                    AnimationAgent.Instance.setBool("시계", "Take", true);
                    Utility.Instance.delayAction(AnimationAgent.Instance.Animators["시계"].GetCurrentAnimatorStateInfo(0).length, () =>
                    {                        
                        Clear = true;
                        Active = false;
                        _stage.DoesAct = false;
                    });
                });
            }
        }
    }

    void rotateHandOfClock()
    {
        if (!_selected) return;

        if (Input.GetKey(KeyCode.RightArrow))
        {            
            SoundManager.Instance.playSoundEffectLoop("시계바늘_움직이기");
            _selected.transform.Rotate(Vector3.forward, -5f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {            
            SoundManager.Instance.playSoundEffectLoop("시계바늘_움직이기");
            _selected.transform.Rotate(Vector3.forward, 5f);
        }
        else
        {
            SoundManager.Instance.stopSoundEffect("시계바늘_움직이기");
        }
    }
}
