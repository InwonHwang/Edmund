using UnityEngine;
using System.Collections;

public class LoadStage : Singleton<LoadStage> {

    GameObject stage;
    GameObject ui;
    GameObject item;

    internal void loadStageImmediate(int numOfStage)
    {

        internalLoadStageImmediate(numOfStage);
    }

    internal void loadStage(int numOfStage)
    {

        StartCoroutine("internalLoadStage", numOfStage);
    }

    #region private method

    IEnumerator internalLoadStage(int numOfStage)
    {
        Utility.Instance.fade(Utility.Instance.findChild("UI", "fade").GetComponent<UnityEngine.UI.Image>(), new Color(0, 0, 0, 1), 2f);
        yield return new WaitForSeconds(6f);
        internalLoadStageImmediate(numOfStage);
        Utility.Instance.fade(Utility.Instance.findChild("UI", "fade").GetComponent<UnityEngine.UI.Image>(), new Color(0, 0, 0, 0), 2f);
        yield return new WaitForSeconds(6f);
    
    }

    void internalLoadStageImmediate(int numOfStage)
    {
        if (stage != null) DestroyImmediate(stage);
        if (ui != null) DestroyImmediate(ui);
        if (item != null)
        {
            unregisterUIObject();
            DestroyImmediate(item);
        }

        string nameOfStage = "Stage" + numOfStage.ToString();
        string nameOfUI = "UI_Stage" + numOfStage.ToString();
        string nameOfItem = "item" + numOfStage.ToString();

        stage = Instantiate(ResourcesManager.Instance.prefabs[nameOfStage]);
        ui = Instantiate(ResourcesManager.Instance.prefabs[nameOfUI]);
        item = Instantiate(ResourcesManager.Instance.prefabs[nameOfItem]);

        stage.name = nameOfStage;
        ui.name = nameOfUI;
        item.name = "item";
        item.transform.SetParent(GameObject.Find("UI").transform);
        item.SetActive(false);

        registerUIObject();

        GameObject.Find("GameManager").GetComponent<GameManager>()._Stage = GameObject.Find("Stage" + numOfStage.ToString()).GetComponent<Stage>();
        GameObject.Find("GameManager").GetComponent<GameManager>()._MiniGame = GameObject.Find("UI_Stage" + numOfStage.ToString());
    }

    void registerUIObject()
    {

        GUIAgent.Instance.registerObject("UI", "item");

        for (int i = 0; i < item.transform.childCount; i++)
        {            
            GUIAgent.Instance.registerObject("UI", "item/" + item.transform.GetChild(i).name);            
            GUIAgent.Instance.GuiObjects.Add("UI/inventory/Panel/base/space/" + item.transform.GetChild(i).name,
                Utility.Instance.findChild("UI", "item/" + item.transform.GetChild(i).name));
        }
    }

    void unregisterUIObject()
    {       
        GUIAgent.Instance.unregisterObject("UI", "item");

        for (int i = 0; i < item.transform.childCount; i++)
        {
            GUIAgent.Instance.unregisterObject("UI", "item/" + item.transform.GetChild(i).name);
            GUIAgent.Instance.GuiObjects.Remove("UI/inventory/Panel/base/space/" + item.transform.GetChild(i).name);
        }
    }

    #endregion
}
