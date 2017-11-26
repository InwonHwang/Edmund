using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    static internal int _StageNum { get; set; }
    internal Stage _Stage { get; set; }
    internal GameObject _MiniGame { get; set; }
    Inventory _inventory;
    EdmundController _edmundController;
    

    void Awake()
    {
        PlayerPrefs.DeleteAll();
        GUIAgent.Instance.registerObject("UI", "inventory");
        GUIAgent.Instance.registerObject("UI", "inventory/Panel");
        GUIAgent.Instance.registerObject("UI", "inventory/Panel/photo_albums");
        GUIAgent.Instance.registerObject("UI", "inventory/Panel/base");
        GUIAgent.Instance.registerObject("UI", "inventory/Panel/base/space");
        GUIAgent.Instance.registerObject("UI", "inventory/Panel/preview");
        GUIAgent.Instance.registerObject("UI", "inventory/Panel/preview/name");
        GUIAgent.Instance.registerObject("UI", "preview/alarm_photo_albums");
        GUIAgent.Instance.registerObject("UI", "preview/alarm_inventory");
        GUIAgent.Instance.registerObject("UI", "preview/image_item");
        GUIAgent.Instance.registerObject("UI", "photo_albums");
        GUIAgent.Instance.registerObject("UI", "photo_albums/Panel");
        GUIAgent.Instance.registerObject("UI", "photo_albums/Panel/photo_albums");
        GUIAgent.Instance.registerObject("UI", "outline");
        GUIAgent.Instance.registerObject("UI", "Menu");
        GUIAgent.Instance.registerObject("UI", "Option");        

        _StageNum = 1;
        LoadStage.Instance.loadStageImmediate(_StageNum);

        _inventory = Utility.Instance.findChild("UI", "inventory").GetComponent<Inventory>();
        _edmundController = GameObject.Find("Edmund").GetComponent<EdmundController>();

        //스테이지 넘어갈때마다 찾아야함
        _Stage = GameObject.Find("Stage" + _StageNum.ToString()).GetComponent<Stage>();
        _MiniGame = GameObject.Find("UI_Stage" + _StageNum.ToString());
    }

    void Update()
    {
        _inventory.chooseItem();
        if (!GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].activeSelf &&
            !GUIAgent.Instance.GuiObjects["UI/photo_albums/Panel"].activeSelf &&
            _MiniGame && !_MiniGame.GetComponent<Canvas>().enabled &&
            _Stage && !_Stage.DoesAct) _edmundController.move();

        if (Input.GetKeyUp(KeyCode.Space) && _MiniGame && !_MiniGame.GetComponent<Canvas>().enabled)
        {
            if (!GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].activeSelf &&
                !GUIAgent.Instance.GuiObjects["UI/Menu"].activeSelf) _Stage.action();
            if (!_Stage.DoesAct) _inventory.selectItem();           
        }
        if (!GUIAgent.Instance.GuiObjects["UI/Menu"].activeSelf &&
            !GUIAgent.Instance.GuiObjects["UI/Option"].activeSelf &&
            _Stage && _Stage.Intro && Input.GetKeyUp(KeyCode.Space))
            _Stage.skipIntro();

        if (Input.GetKeyUp(KeyCode.Z) && _Stage && !_Stage.DoesAct) _inventory.openInventory();
        if (Input.GetKeyUp(KeyCode.X)) _inventory.selectToBeAssembled();
        if (Input.GetKeyUp(KeyCode.C)) _inventory.disassemble();
        if (Input.GetKeyUp(KeyCode.Escape))
            _inventory.closeInvetory();

        if (_Stage && _Stage.Clear)
        {
            _Stage = null;
            _inventory.clearItem();
            LoadStage.Instance.loadStage(++_StageNum);            
        }
    }
}
