using UnityEngine;
using System.Collections;
using System;

public sealed class Stage2 : Stage {

    MatryoshkaPuzzle matryoshkaPuzzle;
    GameObject speechBubbleOfSkull;
    GameObject speechBubbleOfSpider;
    bool secondfloor;


    protected override void Start()
    {       
        base.Start();

        SoundManager.Instance.playSound("Lasting_Hope", 0.3f);
        
        matryoshkaPuzzle = Utility.Instance.findChild("UI_Stage2", "Matryoshka").GetComponent<MatryoshkaPuzzle>();
        speechBubbleOfSkull = Utility.Instance.findChild("Stage2", "해골_말풍선2");
        speechBubbleOfSpider = Utility.Instance.findChild("Stage2", "거미말풍선");
        speechBubbleOfSkull.SetActive(false);
        speechBubbleOfSpider.SetActive(false);

        Utility.Instance.findChild("UI_Stage2", "Puzzle").SetActive(false);
        Utility.Instance.findChild("UI_Stage2", "Matryoshka").SetActive(false);
        Utility.Instance.findChild("UI_Stage2", "Matryoshka2").SetActive(false);
        Utility.Instance.findChild("Stage2", "Object/1층다리뼈").GetComponent<BoxCollider2D>().enabled = false;
        Utility.Instance.findChild("Stage2", "Object/3층비밀").GetComponent<BoxCollider2D>().enabled = false;
        GameObject.Find("UI_Stage2").GetComponent<Canvas>().enabled = false;


        StartCoroutine(intro());
    }
    
    void Update()
    {
        if (_edmund.HitObject != null)
        {
            if (_edmund.HitObject.name.CompareTo("2층거미") == 0 && _inventory.Selected != null && _inventory.Selected.name.CompareTo("visitation_rights") == 0)
            {
                Utility.Instance.findChild("Stage2", "Object/2층거미").GetComponent<EdgeCollider2D>().enabled = false;
                Utility.Instance.findChild("Stage2", "Object/2층거미/막기").GetComponent<EdgeCollider2D>().enabled = false;
                GUIAgent.Instance.GuiObjects["UI/preview/image_item"].SetActive(false);
                _inventory.Selected = null;
                _edmund.HitObject = null;                      
            }
            else if (_edmund.HitObject.name.CompareTo("2층거미") == 0)
            {
                AnimationAgent.Instance.setBool("2층거미", "Take", true);
                speechBubbleOfSpider.SetActive(true);                
                SoundManager.Instance.playSoundEffectLoop("아라크네목소리");
                SoundManager.Instance.stopSoundEffect("아라크네_적기_기본");
                

            }

        }
        else
        {
            AnimationAgent.Instance.setBool("2층거미", "Take", false);
            if (secondfloor)
            {
                SoundManager.Instance.stopSoundEffect("아라크네목소리");
                SoundManager.Instance.playSoundEffectLoop("아라크네_적기_기본");
            }
            else
            {
                SoundManager.Instance.stopSoundEffect("아라크네목소리");
                SoundManager.Instance.stopSoundEffect("아라크네_적기_기본");
            }
            speechBubbleOfSpider.SetActive(false);
            speechBubbleOfSkull.SetActive(false);

        }

        if (!matryoshkaPuzzle.Clear && _inventory.Selected && _inventory.Selected.name.CompareTo("matryoshka_doll3") == 0)
        {
            GameObject.Find("UI_Stage2").GetComponent<Canvas>().enabled = true;
            Utility.Instance.findChild("UI_Stage2", "Matryoshka").SetActive(true);
        }

    }

    protected override IEnumerator internalAction()
    {
        if (_edmund.HitObject == null || AnimationAgent.Instance.getInteger(_edmund.gameObject.name, "State") != 0) yield break;

        DoesAct = true;
        string name = _edmund.HitObject.name;
        Vector2 temp;

        switch (name)
        {
            case "1층다리뼈":
                _edmund.HitObject = null;
                temp = transform.FindChild("Animation/에드먼드_다리뼈/point").transform.position;
                _edmundController.setDirection(temp);
                MoveAgent.Instance.MoveTo(_edmund.gameObject, temp, 1f);
                yield return new WaitForSeconds(1f);

                _inventory.getItem("leg_bone");
                Utility.Instance.findChild("Stage2", "Object/1층다리뼈").SetActive(false);
                Utility.Instance.findChild("Stage2", "Animation/에드먼드_다리뼈").SetActive(true);
                _edmund.gameObject.SetActive(false);
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_다리뼈"].GetCurrentAnimatorStateInfo(0).length / 2);
                SoundManager.Instance.playSoundEffect("다리뼈뽑기", 0.5f);
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_다리뼈"].GetCurrentAnimatorStateInfo(0).length / 2);
                _edmund.gameObject.SetActive(true);
                _edmundController.setDirection(0);
                Utility.Instance.findChild("Stage2", "Animation/에드먼드_다리뼈").SetActive(false);
                
                break;

            case "1층해골":
                if (_inventory.Selected == null || _inventory.Selected.name.CompareTo("leg_bone") != 0)
                {
                    speechBubbleOfSkull.SetActive(true);
                    Utility.Instance.findChild("Stage2", "Object/1층다리뼈").GetComponent<BoxCollider2D>().enabled = true;
                    DoesAct = false;
                    yield break;
                }

                _edmund.HitObject = null;

                temp = transform.FindChild("Animation/에드먼드_다리뼈주기/point").transform.position;
                _edmundController.setDirection(temp);
                MoveAgent.Instance.MoveTo(_edmund.gameObject, temp, 1f);
                Utility.Instance.findChild("Stage2", "1층막대1").GetComponent<SpriteRenderer>().sortingOrder = 6;
                yield return new WaitForSeconds(1f);

                _inventory.discardItem("leg_bone");

                Utility.Instance.findChild("Stage2", "Object/1층해골").GetComponent<BoxCollider2D>().enabled = false;
                Camera.main.gameObject.GetComponent<CameraController>().setTarget(Utility.Instance.findChild("Stage2", "Object/1층해골").transform);
                AnimationAgent.Instance.setBool("1층해골", "Take", true);

                _edmund.gameObject.SetActive(false);
                Utility.Instance.findChild("Stage2", "Animation/에드먼드_다리뼈주기").SetActive(true);
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_다리뼈주기"].GetCurrentAnimatorStateInfo(0).length);
                Utility.Instance.findChild("Stage2", "Animation/에드먼드_다리뼈주기").SetActive(false);
                _edmund.gameObject.SetActive(true);
                _edmundController.setDirection(0);
                
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["1층해골"].GetCurrentAnimatorStateInfo(0).length);
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["1층해골"].GetCurrentAnimatorStateInfo(0).length);
                SoundManager.Instance.playSoundEffectLoop("해골걷기");
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["1층해골"].GetCurrentAnimatorStateInfo(0).length);
                SoundManager.Instance.stopSoundEffect("해골걷기");
                Utility.Instance.findChild("Stage2", "1층레버").SetActive(false);
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["1층해골"].GetCurrentAnimatorStateInfo(0).length/4);
                SoundManager.Instance.playSoundEffect("레버", 0.5f);
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["1층해골"].GetCurrentAnimatorStateInfo(0).length /4 * 3);

                iTween.MoveAdd(Utility.Instance.findChild("Stage2", "1층막대1"), iTween.Hash("y", 17f, "time", 4f));
                SoundManager.Instance.playSoundEffect("철창");
                yield return new WaitForSeconds(2f);
                Utility.Instance.findChild("Stage2", "1층막대1").SetActive(false);
                Camera.main.gameObject.GetComponent<CameraController>().setTarget(_edmund.gameObject.transform);
                Utility.Instance.findChild("Stage2", "Object/1층해골").GetComponent<BoxCollider2D>().enabled = false;

                Utility.Instance.findChild("Stage2", "1층막대1").GetComponent<SpriteRenderer>().sortingOrder = 3;
                _inventory.Selected = null;
                break;

            case "1층마트료시카":
                _edmund.HitObject = null;
                AnimationAgent.Instance.setInteger("Edmund", "State", 7);
                SoundManager.Instance.playSoundEffect("아이템줍기");
                _inventory.getItem("matryoshka_doll1");
                yield return new WaitForSeconds(0.5f);
                Utility.Instance.findChild("Stage2", "Item/1층마트료시카").SetActive(false);
                AnimationAgent.Instance.setInteger("Edmund", "State", 0);
                
                break;

            case "1층성냥막대":
                Utility.Instance.findChild("Stage2", "Object/1층성냥막대").GetComponent<BoxCollider2D>().enabled = false;
                _edmund.HitObject = null;

                temp = transform.FindChild("Animation/에드먼드_성냥/point").transform.position;
                _edmundController.setDirection(temp);
                MoveAgent.Instance.MoveTo(_edmund.gameObject, temp, 1f);
                yield return new WaitForSeconds(1f);

                Utility.Instance.findChild("Stage2", "Animation/에드먼드_성냥").SetActive(true);
                Utility.Instance.findChild("Stage2", "Item/1층성냥").SetActive(false);
                Utility.Instance.findChild("Stage2", "1층성냥막대").SetActive(false);
                Utility.Instance.findChild("Stage2", "1층성냥받침대").SetActive(false);
                _edmund.gameObject.SetActive(false);

                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_성냥"].GetCurrentAnimatorStateInfo(0).length);
                SoundManager.Instance.playSoundEffect("성냥떨어지는소리");

                _edmund.gameObject.SetActive(true);
                _edmundController.setDirection(0);
                Utility.Instance.findChild("Stage2", "Animation/에드먼드_성냥").SetActive(false);
                Utility.Instance.findChild("Stage2", "Item/1층성냥").SetActive(true);
                Utility.Instance.findChild("Stage2", "1층성냥막대").SetActive(true);
                Utility.Instance.findChild("Stage2", "1층성냥받침대").SetActive(true);

                Utility.Instance.findChild("Stage2", "Item/1층성냥").transform.localPosition = new Vector3(3.288f, -15.66f, 0);
                Utility.Instance.findChild("Stage2", "1층성냥막대").transform.localPosition = new Vector3(4.62f, -13.54f, -1);
                Utility.Instance.findChild("Stage2", "1층성냥막대").transform.rotation = Quaternion.Euler(0, 0, 3.2f);
                Utility.Instance.findChild("Stage2", "1층성냥받침대").transform.rotation = Quaternion.Euler(0, 0, 3.5f);
                Utility.Instance.findChild("Stage2", "Item/1층성냥").transform.rotation = Quaternion.Euler(0, 0, -45f);
                               
                break;

            case "1층성냥":
                _edmund.HitObject = null;
                SoundManager.Instance.playSoundEffect("아이템줍기");
                AnimationAgent.Instance.setInteger("Edmund", "State", 7);
                _inventory.getItem("matches");
                yield return new WaitForSeconds(0.5f);
                Utility.Instance.findChild("Stage2", "Item/1층성냥").SetActive(false);
                AnimationAgent.Instance.setInteger("Edmund", "State", 0);
                
                break;

            case "1층문":
                secondfloor = true;
                _edmund.HitObject = null;
                AnimationAgent.Instance.setInteger("Edmund", "State", 3);
                yield return new WaitForSeconds(1f);
                
                Utility.Instance.findChild("Stage2", "Object/1층성냥막대").GetComponent<BoxCollider2D>().enabled = true;
                _edmund.transform.position = Utility.Instance.findChild("Stage2", "Object/2층문1").transform.position + new Vector3(0, 1.8f, 0);
                _edmundController.setDirection(1);
                iTween.MoveTo(Camera.main.gameObject, new Vector3(Camera.main.transform.position.x, 0.25f, Camera.main.transform.position.z), 1);
                AnimationAgent.Instance.setInteger("Edmund", "State", 0);                
                
                break;

            case "2층문1":
                secondfloor = false;
                _edmund.HitObject = null;
                AnimationAgent.Instance.setInteger("Edmund", "State", 3);
                yield return new WaitForSeconds(1f);

                Utility.Instance.findChild("Stage2", "Object/1층성냥막대").GetComponent<BoxCollider2D>().enabled = true;
                _edmund.transform.position = Utility.Instance.findChild("Stage2", "Object/1층문").transform.position + new Vector3(0, 1.8f, 0);
                _edmundController.setDirection(1);
                iTween.MoveTo(Camera.main.gameObject, new Vector3(Camera.main.transform.position.x, -12.455f, Camera.main.transform.position.z), 1);
                AnimationAgent.Instance.setInteger("Edmund", "State", 0);

                break;

            case "2층거미뭉치":
                _edmund.HitObject = null;
                temp = transform.FindChild("Animation/에드먼드_거미줄/point").transform.position;
                _edmundController.setDirection(temp);
                MoveAgent.Instance.MoveTo(_edmund.gameObject, temp, 1f);
                yield return new WaitForSeconds(1f);
                
                _inventory.getItem("spiderweb");
                Utility.Instance.findChild("Stage2", "Object/2층거미뭉치").SetActive(false);
                Utility.Instance.findChild("Stage2", "Animation/에드먼드_거미줄").SetActive(true);
                _edmund.gameObject.SetActive(false);
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_거미줄"].GetCurrentAnimatorStateInfo(0).length / 4);
                SoundManager.Instance.playSoundEffect("거미줄뽑기");
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_거미줄"].GetCurrentAnimatorStateInfo(0).length / 4 * 3);
                _edmund.gameObject.SetActive(true);
                _edmundController.setDirection(1);
                Utility.Instance.findChild("Stage2", "Animation/에드먼드_거미줄").SetActive(false);                

                break;

            case "2층바닥퍼즐":
                GameObject.Find("UI_Stage2").GetComponent<Canvas>().enabled = true;
                Utility.Instance.findChild("UI_Stage2", "Puzzle").SetActive(true);

                break;

            case "2층열쇠":
                _edmund.HitObject = null;
                temp = transform.FindChild("Animation/에드먼드_열쇠줍기/point").transform.position;
                _edmundController.setDirection(temp);
                MoveAgent.Instance.MoveTo(_edmund.gameObject, temp, 1f);

                yield return new WaitForSeconds(1f);

                _edmund.gameObject.SetActive(false);
                Utility.Instance.findChild("Stage2", "Animation/에드먼드_열쇠줍기").SetActive(true);
                _inventory.getItem("key2");
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_열쇠줍기"].GetCurrentAnimatorStateInfo(0).length / 2);
                Utility.Instance.findChild("Stage2", "Object/2층열쇠").SetActive(false);
                SoundManager.Instance.playSoundEffect("열쇠줍기");
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_열쇠줍기"].GetCurrentAnimatorStateInfo(0).length / 2);
                Utility.Instance.findChild("Stage2", "Animation/에드먼드_열쇠줍기").SetActive(false);
                _edmund.gameObject.SetActive(true);
                _edmundController.setDirection(0);               

                break;

            case "2층문2":
                secondfloor = false;
                _edmund.HitObject = null;
                AnimationAgent.Instance.setInteger("Edmund", "State", 3);
                yield return new WaitForSeconds(1f);
                
                _edmund.transform.position = Utility.Instance.findChild("Stage2", "Object/3층문").transform.position + new Vector3(0, 1.8f, 0);
                _edmundController.setDirection(1);
                iTween.MoveTo(Camera.main.gameObject, new Vector3(Camera.main.transform.position.x, 12.5f, Camera.main.transform.position.z), 1);
                AnimationAgent.Instance.setInteger("Edmund", "State", 0);
                break;

            case "3층문":
                secondfloor = true;
                _edmund.HitObject = null;
                AnimationAgent.Instance.setInteger("Edmund", "State", 3);
                yield return new WaitForSeconds(1f);

                _edmund.transform.position = Utility.Instance.findChild("Stage2", "Object/2층문2").transform.position + new Vector3(0, 1.8f, 0);
                _edmundController.setDirection(1);
                iTween.MoveTo(Camera.main.gameObject, new Vector3(Camera.main.transform.position.x, 0.25f, Camera.main.transform.position.z), 1);
                AnimationAgent.Instance.setInteger("Edmund", "State", 0);
                break;

            case "3층미니게임":
                if(_inventory.Selected == null || (_inventory.Selected != null && !_inventory.Selected.name.Contains("matryoshka_doll")))
                {
                    DoesAct = false;

                    yield break;
                }
                

                GameObject.Find("UI_Stage2").GetComponent<Canvas>().enabled = true;
                Utility.Instance.findChild("UI_Stage2", "Matryoshka2").SetActive(true);
                break;
        }

        DoesAct = false;

        yield return null;
    }

    protected override IEnumerator intro()
    {
        DoesAct = true;

        GUIAgent.Instance.setSprite("UI", "outline", ResourcesManager.Instance.sprites["시작이미지_스테이지2"]);

        while (Input.GetKeyUp(KeyCode.Space) == false) yield return null;

        GUIAgent.Instance.GuiObjects["UI/outline"].SetActive(false);

        Camera.main.GetComponent<Animator>().enabled = true;
        Camera.main.GetComponent<CameraController>().Boundary = 2.805f;

        _edmundController.transform.position = new Vector3(12f, -14f, 0);
        _edmundController.setDirection(0);

        yield return new WaitForSeconds(6f);
        Camera.main.transform.GetComponent<Animator>().enabled = false;
        

        DoesAct = false;
    }

    internal override string getName(string name)
    {
        if (name == null) return null;

        switch (name)
        {
            case "leg_bone":
                return "다리뼈";
            case "matches":
                return "성냥";
            case "key1":
                return "열쇠1";
            case "key2":
                return "열쇠2";
            case "matryoshka_doll1":
                return "마트료시카1";
            case "matryoshka_doll2":
                return "마트료시카2";
            case "matryoshka_doll3":
                return "마트료시카3";
            case "matryoshka_doll4":
                return "마트료시카4";
            case "matryoshka_doll1_used":
                return "마트료시카1";
            case "matryoshka_doll2_used":
                return "마트료시카2";
            case "matryoshka_doll3_used":
                return "마트료시카3";
            case "matryoshka_doll4_used":
                return "마트료시카4";
            case "spiderweb":
                return "거미줄뭉치";
            case "visitation_rights":
                return "면회권";
        }

        return null;
    }
}
