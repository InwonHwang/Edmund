using UnityEngine;
using System.Collections;

public sealed class Stage1 : Stage
{
    int _countOfStone;
    int _indexOfMobile;
    GameObject[] _mobiles;
    Vector3[] _initialPosOfMobiles;
    GameObject _sppechBubbleOfPoble;

    protected override void Start()
    {
        base.Start();

        _cur = 0;
        _numSkipIntro = 4;
        _skipIntro = new bool[_numSkipIntro];
        Camera.main.GetComponent<CameraController>().Boundary = 3.55f;
        SoundManager.Instance.playSound("Stage1배경음", 0.3f);
        SoundManager.Instance.playSoundEffectLoop("모형새", 0.05f);

        _mobiles = new GameObject[4];
        _initialPosOfMobiles = new Vector3[4];

        _countOfStone = 1;
        _indexOfMobile = 0;
        _sppechBubbleOfPoble = Utility.Instance.findChild("Stage1", "Object/퀘스트포블말풍선");
        _sppechBubbleOfPoble.SetActive(false);
        GameObject.Find("UI_Stage1").GetComponent<Canvas>().enabled = false;
        Utility.Instance.findChild("UI_Stage1", "Clock").SetActive(false);
        _mobiles[0] = Utility.Instance.findChild("Stage1", "Background/상어");
        _mobiles[1] = Utility.Instance.findChild("Stage1", "Background/꽃");
        _mobiles[2] = Utility.Instance.findChild("Stage1", "Background/보석");
        _mobiles[3] = Utility.Instance.findChild("Stage1", "Background/모형새");
        _initialPosOfMobiles[0] = new Vector3(-20f, 20f, -1f);
        _initialPosOfMobiles[1] = new Vector3(-10f, 20f, -1f);
        _initialPosOfMobiles[2] = new Vector3(10f, 20f, -1f);
        _initialPosOfMobiles[3] = new Vector3(20f, 20f, -1f);
        Camera.main.GetComponent<Animator>().enabled = false;

        StartCoroutine(intro());
    }

    void Update()
    {
        if(_edmund.HitObject == null)
        {
            _sppechBubbleOfPoble.SetActive(false);
            SoundManager.Instance.stopSoundEffect("포블_퀘스트");            
        }

        if (_edmundController.Action == null && _inventory.Selected && _inventory.Selected.name.Contains("sling") &&
            _inventory.Selected.name.CompareTo("sling_glass") != 0)
        {
            createSlingMotion();
            _edmundController.Action = controlSlingMotion;
        }
        else if(_edmundController.Action != null && Input.GetKeyUp(KeyCode.Escape))
        {
            _edmundController.Action = null;
            Utility.Instance.findChild("Edmund", "Base").SetActive(true);
            Utility.Instance.findChild("Edmund", "새총모션").SetActive(false);
        }
    }

    protected override IEnumerator intro()
    {
        DoesAct = true;
        Intro = true;

        GUIAgent.Instance.setSprite("UI", "outline", ResourcesManager.Instance.sprites["시작이미지_스테이지1"]);

        while (!_skipIntro[0]) yield return null;

        GUIAgent.Instance.setSprite("UI", "outline", ResourcesManager.Instance.sprites["outline"]);

        var anim = _edmund.GetComponent<Animator>();
        anim.enabled = true;
        AnimationAgent.Instance.setInteger("Edmund", "State", 3);
        Utility.Instance.findChild("Stage1", "Object/퀘스트포블").GetComponent<SetLayer>().enabled = false;

        var sr = Utility.Instance.findChild("Edmund", "Base").GetComponent<SpriteRenderer>();

        sr.sortingOrder = 0;        
        yield return new WaitForSeconds(10f);
        //sr.sortingOrder = 0;
        //yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        sr.sortingOrder = 4;
        Utility.Instance.findChild("Stage1", "Object/퀘스트포블").GetComponent<SetLayer>().enabled = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        AnimationAgent.Instance.setInteger("Edmund", "State", 0);

        var menu1 = Utility.Instance.findChild("Stage1", "Background/설명1");
        var menu2 = Utility.Instance.findChild("Stage1", "Background/설명2");
        var skip1 = Utility.Instance.findChild("Stage1", "Background/설명1/넘어가기");
        var skip2 = Utility.Instance.findChild("Stage1", "Background/설명2/넘어가기");
        var srMenu1 = menu1.GetComponent<SpriteRenderer>();
        var srMenu2 = menu2.GetComponent<SpriteRenderer>();
        var srSkip1 = skip1.GetComponent<SpriteRenderer>();
        var srSkip2 = skip2.GetComponent<SpriteRenderer>();
        var color = new Color(1,1,1,1);

        Utility.Instance.fade(srMenu1, color, 2f);
        //Utility.Instance.fade(srSkip1, color, 1f);
        while (!_skipIntro[1] && srMenu1.color != color) yield return null;
        Utility.Instance.StopAllCoroutines();
        srMenu1.color = color;
        //srSkip1.color = color;

        Utility.Instance.fade(srMenu2, color, 2f);
        //Utility.Instance.fade(srSkip2, color, 1f);
        while (!_skipIntro[2] && srMenu2.color != color) yield return null;
        Utility.Instance.StopAllCoroutines();
        srMenu2.color = color;
        srSkip2.color = color;

        while (!_skipIntro[3]) yield return null;

        menu1.SetActive(false);
        menu2.SetActive(false);
        skip1.SetActive(false);
        skip2.SetActive(false);

        anim.enabled = false;
        DoesAct = false;
        Intro = false;
    }

    protected override IEnumerator internalAction()
    {
        if (DoesAct || _edmund.HitObject == null || AnimationAgent.Instance.getInteger(_edmund.gameObject.name, "State") != 0) yield break;

        DoesAct = true;
        string name = _edmund.HitObject.name;
        Vector2 temp;

        switch (name)
        {
            case "사진":
                _edmund.HitObject = null;
                AnimationAgent.Instance.setInteger("Edmund", "State", 7);
                SoundManager.Instance.playSoundEffect("아이템줍기");
                yield return new WaitForSeconds(0.5f);
                Utility.Instance.findChild("Stage1", "Item/사진").SetActive(false);
                AnimationAgent.Instance.setInteger("Edmund", "State", 0);
                Utility.Instance.findChild("UI", "photo_albums").GetComponent<PhotoAlbums>().HasPhoto = true;
                GUIAgent.Instance.setSprite("UI/photo_albums/Panel/photo_albums", ResourcesManager.Instance.sprites["ui_photo_albums_2"]);
                Clear = true;
                break;

            case "시계":
                if (Utility.Instance.findChild("UI_Stage1", "Clock").GetComponent<Clock>().Clear == false)
                {
                    GameObject.Find("UI_Stage1").GetComponent<Canvas>().enabled = true;
                    Utility.Instance.findChild("UI_Stage1", "Clock").SetActive(true);
                }
                else if (Utility.Instance.findChild("UI_Stage1", "Clock").GetComponent<Clock>().Clear == true)
                {
                    temp = transform.FindChild("Animation/에드먼드_레버당기기/point").transform.position;
                    _edmundController.setDirection(temp);
                    MoveAgent.Instance.MoveTo(_edmund.gameObject, temp, 1f);
                    yield return new WaitForSeconds(1f);

                    Utility.Instance.findChild("Stage1", "Animation/에드먼드_레버당기기").SetActive(true);
                    Utility.Instance.findChild("Stage1", "Object/시계").SetActive(false);
                    _edmund.gameObject.SetActive(false);

                    yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_레버당기기"].GetCurrentAnimatorStateInfo(0).length / 2);
                    SoundManager.Instance.playSoundEffect("레버", 0.5f);
                    SoundManager.Instance.playSoundEffect("모빌움직이는거", 0.5f);

                    var windRigidbody = Utility.Instance.findChild("Stage1", "Background/Wind.Rigidbody/Wind.Remote.Control").GetComponent<WindRigidbody>();
                    windRigidbody.windStrengthMin = 2f;
                    windRigidbody.windStrengthMax = 2f;

                    var mobile = _mobiles[_indexOfMobile];
                    MoveAgent.Instance.MoveTo(mobile, new Vector3(-0.8f, 7.3f * 0.83f, -0.83f),
                        AnimationAgent.Instance.Animators["에드먼드_레버당기기"].GetCurrentAnimatorStateInfo(0).length / 2);
                    yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_레버당기기"].GetCurrentAnimatorStateInfo(0).length / 2);
                    Utility.Instance.findChild("Stage1", "Animation/에드먼드_레버당기기").SetActive(false);
                    Utility.Instance.findChild("Stage1", "Object/시계").SetActive(true);
                    _edmund.gameObject.SetActive(true);
                    _edmundController.setDirection(1);
                    _edmund.HitObject = null;

                    mobile.transform.position = new Vector3(_initialPosOfMobiles[_indexOfMobile].x * 0.83f, _initialPosOfMobiles[_indexOfMobile].y * 0.83f,
                                                            _initialPosOfMobiles[_indexOfMobile].z * 0.83f);
                    _indexOfMobile = (_indexOfMobile + 1) % 4;
                    mobile = _mobiles[_indexOfMobile];
                    mobile.transform.position = new Vector3(4.2f * 0.83f, 7.1f * 0.83f, -0.83f);

                    if (_indexOfMobile == 3)
                    {
                        Utility.Instance.findChild("Stage1", "Object/모빌").SetActive(true);
                        SoundManager.Instance.playSoundEffectLoop("모형새", 0.3f);
                    }
                    else
                    {
                        Utility.Instance.findChild("Stage1", "Object/모빌").SetActive(false);
                        SoundManager.Instance.playSoundEffectLoop("모형새", 0.05f);
                    }

                    MoveAgent.Instance.MoveTo(mobile.gameObject, new Vector3(1.3f * 0.83f, 7.1f * 0.83f, -0.83f), 1);

                    yield return new WaitForSeconds(1f);

                    windRigidbody.windStrengthMin = -2f;
                    windRigidbody.windStrengthMax = 2f;


                }
                break;
            case "산호":
                _edmund.HitObject = null;
                temp = transform.FindChild("Animation/에드먼드_새총풀뽑기/point").transform.position;
                _edmundController.setDirection(temp);
                MoveAgent.Instance.MoveTo(_edmund.gameObject, temp, 1f);
                yield return new WaitForSeconds(1f);

                _inventory.getItem("sling_grass");
                AnimationAgent.Instance.setBool("새총풀", "Take", true);
                Utility.Instance.findChild("Stage1", "Object/새총풀").transform.position = new Vector3(5.52f * 0.83f, 0.36f * 0.83f, 0);
                Utility.Instance.findChild("Stage1", "Animation/에드먼드_새총풀뽑기").SetActive(true);
                _edmund.gameObject.SetActive(false);
                SoundManager.Instance.playSoundEffect("새총풀뜯기");
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_새총풀뽑기"].GetCurrentAnimatorStateInfo(0).length);
                Utility.Instance.findChild("Stage1", "Animation/에드먼드_새총풀뽑기").SetActive(false);
                _edmund.gameObject.SetActive(true);
                _edmundController.setDirection(0);
                Utility.Instance.findChild("Stage1", "Object/산호").GetComponent<Collider2D>().enabled = false;

                break;

            case "새총나무":
                _edmund.HitObject = null;
                temp = transform.FindChild("Animation/에드먼드_막대기뽑기/point").transform.position;
                _edmundController.setDirection(temp);
                MoveAgent.Instance.MoveTo(_edmund.gameObject, temp, 1f);
                yield return new WaitForSeconds(1f);

                _inventory.getItem("stick");
                Utility.Instance.findChild("Stage1", "Animation/에드먼드_막대기뽑기").SetActive(true);
                Utility.Instance.findChild("Stage1", "Object/새총나무").SetActive(false);
                _edmund.gameObject.SetActive(false);
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_막대기뽑기"].GetCurrentAnimatorStateInfo(0).length / 2);
                SoundManager.Instance.playSoundEffect("막대기뽑기");
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_막대기뽑기"].GetCurrentAnimatorStateInfo(0).length / 2);
                Utility.Instance.findChild("Stage1", "Animation/에드먼드_막대기뽑기").SetActive(false);
                Utility.Instance.findChild("Stage1", "Object/새총나무").SetActive(true);
                Utility.Instance.findChild("Stage1", "Object/새총나무").GetComponent<SpriteRenderer>().sprite = ResourcesManager.Instance.sprites["새총나무_후"];
                _edmund.gameObject.SetActive(true);
                _edmundController.setDirection(0);
                Utility.Instance.findChild("Stage1", "Object/새총나무").GetComponent<Collider2D>().enabled = false;

                break;

            case "퀘스트포블":
                if (_inventory.Selected == null || _inventory.Selected.name.CompareTo("flower") != 0)
                {
                    _sppechBubbleOfPoble.SetActive(true);
                    SoundManager.Instance.playSoundEffectLoop("포블_퀘스트", 0.4f);
                    DoesAct = false;
                    yield break;
                }

                _edmund.HitObject = null;
                _inventory.discardItem("flower");
                AnimationAgent.Instance.setBool("퀘스트포블", "Take", true);
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["퀘스트포블"].GetCurrentAnimatorStateInfo(0).length);

                Clear = true;
                DoesAct = false;
                SoundManager.Instance.stopSoundEffect("모형새");

                break;

            case "시침포블":
                _edmund.HitObject = null;
                temp = transform.FindChild("Animation/에드먼드_시침뽑기/point").transform.position;
                _edmundController.setDirection(temp);
                MoveAgent.Instance.MoveTo(_edmund.gameObject, temp, 1f);
                yield return new WaitForSeconds(1f);

                _inventory.getItem("minute_hand");
                Utility.Instance.findChild("Stage1", "Animation/에드먼드_시침뽑기").SetActive(true);
                Utility.Instance.findChild("Stage1", "Object/시침포블").SetActive(false);
                _edmund.gameObject.SetActive(false);
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_시침뽑기"].GetCurrentAnimatorStateInfo(0).length / 3);
                SoundManager.Instance.playSoundEffect("분침뽑기");
                yield return new WaitForSeconds(AnimationAgent.Instance.Animators["에드먼드_시침뽑기"].GetCurrentAnimatorStateInfo(0).length / 3 * 2);
                Utility.Instance.findChild("Stage1", "Animation/에드먼드_시침뽑기").SetActive(false);
                Utility.Instance.findChild("Stage1", "Background/시침포블_그림자").SetActive(true);
                Utility.Instance.findChild("Stage1", "Object/시침포블").SetActive(true);
                AnimationAgent.Instance.setBool("시침포블", "Take", true);
                _edmund.gameObject.SetActive(true);
                _edmundController.setDirection(1);
                Utility.Instance.findChild("Stage1", "Object/시침포블").GetComponent<Collider2D>().enabled = false;
                break;

            case "돌멩이상자":
                if (AnimationAgent.Instance.getBool("돌멩이상자", "Take") == false)
                {
                    AnimationAgent.Instance.setBool("돌멩이상자", "Take", true);
                    SoundManager.Instance.playSoundEffect("상자열리는소리");
                }
                else if (_countOfStone < 4)
                {
                    _inventory.getItem("stone" + (_countOfStone++).ToString());
                    SoundManager.Instance.playSoundEffect("돌멩이얻기");
                }
                else
                {
                    Utility.Instance.findChild("Stage1", "Object/돌멩이상자").GetComponent<Collider2D>().enabled = false;
                    _edmund.HitObject = null;
                }

                break;

            case "꽃":
                _edmund.HitObject = null;
                AnimationAgent.Instance.setInteger("Edmund", "State", 7);
                SoundManager.Instance.playSoundEffect("아이템줍기");
                yield return new WaitForSeconds(0.5f);
                _inventory.getItem("flower");
                Utility.Instance.findChild("Stage1", "Item/꽃").SetActive(false);
                AnimationAgent.Instance.setInteger("Edmund", "State", 0);

                break;
        }

        DoesAct = false;

        yield return null;
    }

    internal override string getName(string name)
    {
        if (name == null) return null;

        switch (name)
        {
            case "stone1":
                return "돌맹이";
            case "stone2":
                return "돌맹이";
            case "stone3":
                return "돌맹이";
            case "sling_grass":
                return "새총풀";
            case "stick":
                return "막대기";
            case "minute_hand":
                return "분침";
            case "sling1":
                return "새총";
            case "sling2":
                return "새총";
            case "kind_of_trees":
                return "나무막대";
            case "flower":
                return "꽃";
        }

        return null;
    }

    void createSlingMotion()
    {
        var destroyed = _edmund.transform.FindChild("새총모션");
        if (destroyed) GameObject.Destroy(destroyed.gameObject);        
        Utility.Instance.findChild("Edmund", "Base").SetActive(false);
        var temp = Instantiate(ResourcesManager.Instance.prefabs["새총모션"]) as GameObject;
        temp.name = "새총모션";        
        temp.transform.SetParent(_edmund.transform);
        temp.transform.localPosition = Vector3.zero;

        float x = _edmund.transform.localScale.x < 0 ? -temp.transform.localScale.x : temp.transform.localScale.x;
        temp.transform.localScale = new Vector3(x, temp.transform.localScale.y, temp.transform.localScale.z);

        if (_inventory.Selected && _inventory.Selected.name.CompareTo("sling1") == 0)
        {
            temp.transform.FindChild("중심/오른팔중심/돌멩이/돌멩이").gameObject.SetActive(false);            
        }
    }

    void controlSlingMotion()
    {
        float horizental = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        if(vertical != 0)
        {
            var child = Utility.Instance.findChild("Edmund", "새총모션/중심/얼굴중심");
            float rotationZ = child.transform.eulerAngles.z;

            if (rotationZ > 10f && rotationZ < 11f)            
                child.transform.eulerAngles = new Vector3(0, 0, 9.9f);    
            else if(rotationZ < 350f && rotationZ > 349f)
                child.transform.eulerAngles = new Vector3(0, 0, 350.1f);
            else            
                child.transform.Rotate(Vector3.forward, vertical * 6 * Time.deltaTime);

            child = Utility.Instance.findChild("Edmund", "새총모션/중심/오른팔중심");
            rotationZ = child.transform.eulerAngles.z;

            if (rotationZ > 16 && rotationZ < 17)
                child.transform.eulerAngles = new Vector3(0, 0, 15.9f);
            else if (rotationZ < 343 && rotationZ > 342f)
                child.transform.eulerAngles = new Vector3(0, 0, 343.1f);
            else
                child.transform.Rotate(Vector3.forward, vertical * 10 * Time.deltaTime);

            child = Utility.Instance.findChild("Edmund", "새총모션/중심/왼팔중심");
            rotationZ = child.transform.eulerAngles.z;
            if (rotationZ > 30f && rotationZ < 31f)
                child.transform.eulerAngles = new Vector3(0, 0, 29.9f);
            else if (rotationZ < 326 && rotationZ > 325f)
                child.transform.eulerAngles = new Vector3(0, 0, 326.1f);
            else
                child.transform.Rotate(Vector3.forward, vertical * 20 * Time.deltaTime);
        }


        if (horizental < 0)
            _edmundController.setDirection(0);
        else if(horizental > 0)
            _edmundController.setDirection(1);
    }

    

    
}
