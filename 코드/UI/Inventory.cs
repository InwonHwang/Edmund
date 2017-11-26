using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    internal GameObject Selected { get; set; }
    GameObject toBeAssembled;
    GameObject cur;
            
    int index = 0;
    Stage stage;

    void Start()
    {
        stage = GameObject.Find("Stage" + GameManager._StageNum.ToString()).GetComponent<Stage>();        

        var item = Utility.Instance.findChild("UI", "item");
        for (int i = 0; i < item.transform.childCount; i++)
            item.transform.GetChild(i).gameObject.SetActive(false);

        GUIAgent.Instance.GuiObjects["UI/preview/image_item"].SetActive(false);
        GUIAgent.Instance.GuiObjects["UI/preview/alarm_inventory"].SetActive(false);
        GUIAgent.Instance.GuiObjects["UI/preview/alarm_photo_albums"].SetActive(false);
        GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].SetActive(false);
    }

    #region internal method

    internal void clearItem()
    {
        internalClearItem();
    }

    internal void getItem(string name)
    {
        internalGetItem(name);
    }

    internal void discardItem(string name)
    {
        internalDiscardItem(name);
    }

    internal void openInventory()
    {
        internalOpenInventory();
    }

    internal void selectToBeAssembled()
    {
        internalSelectToBeAssembled();
    }

    internal void assemble()
    {
        internalAssemble();
    }

    internal void disassemble()
    {
        internalDisassemble();
    }

    internal void selectItem()
    {
        internalSelectItem();
    }

    internal void chooseItem()
    {
        internalChooseItem();
    }

    internal void closeInvetory()
    {
        internalCloseInventory();
    }


    #endregion

    #region private method

    void internalClearItem()
    {
        var parent = GUIAgent.Instance.GuiObjects["UI/item"];
        var temp = Utility.Instance.findChild("UI", "inventory/Panel/base/space");

        for(int i = 0; i < temp.transform.childCount; i++)
        {
            temp.transform.GetChild(i).SetParent(parent.transform);
        }

        GUIAgent.Instance.GuiObjects["UI/preview/image_item"].SetActive(false);
        index = 0;
    }

    void internalGetItem(string name)
    {
        var parent = GUIAgent.Instance.GuiObjects["UI/inventory/Panel/base/space"];

        if (parent.transform.childCount == 8) return;

        GUIAgent.Instance.GuiObjects["UI/preview/alarm_inventory"].SetActive(true);

        GUIAgent.Instance.GuiObjects["UI/item/" + name].SetActive(true);
        GUIAgent.Instance.GuiObjects["UI/item/" + name].transform.SetParent(parent.transform, false);
    }

    void internalDiscardItem(string name)
    {
        var parent = GUIAgent.Instance.GuiObjects["UI/item"];
        var item = Utility.Instance.findChild("UI", "inventory/Panel/base/space/" + name);

        if (item == null) return;

        item.SetActive(false);
        item.transform.SetParent(parent.transform, false);
        GUIAgent.Instance.GuiObjects["UI/preview/image_item"].SetActive(false);
        index = 0;
    }

    void internalOpenInventory()
    {
        GUIAgent.Instance.GuiObjects["UI/preview/alarm_inventory"].SetActive(false);

        //Sound        
        SoundManager.Instance.playSoundEffect("지퍼", 0.5f);        

        //Edmund Animation
        AnimationAgent.Instance.Animators["Edmund"].Play("inventory1");

        var parent = GUIAgent.Instance.GuiObjects["UI/inventory/Panel/base/space"];

        Utility.Instance.delayAction(0.3f, () =>
        {            
            GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].SetActive(true);            

            //set sprite
            GUIAgent.Instance.setSprite("UI", "inventory/Panel/photo_albums", ResourcesManager.Instance.sprites["ui_photo_albums_0"]);
            GUIAgent.Instance.setSprite("UI", "inventory/Panel/base", ResourcesManager.Instance.sprites["ui_base_selected"]);

            if (parent.transform.childCount > 0) // 인벤토리에 아이템이 1개 이상일때
            {
                var name = parent.transform.GetChild(0).name;

                //미리보기 sprite 및 text 변경
                GUIAgent.Instance.GuiObjects["UI/inventory/Panel/preview"].GetComponent<Image>().enabled = true;
                GUIAgent.Instance.GuiObjects["UI/inventory/Panel/preview/name"].GetComponent<Text>().enabled = true;
                GUIAgent.Instance.setSprite("UI", "inventory/Panel/preview", ResourcesManager.Instance.sprites[name]);
                GUIAgent.Instance.setText("UI", "inventory/Panel/preview/name", stage.getName(name));

                //선택된 아이템 sprite 변경
                for (int i = 0; i < parent.transform.childCount; i++)
                    GUIAgent.Instance.setSprite(parent.transform.GetChild(i).gameObject, ResourcesManager.Instance.sprites[parent.transform.GetChild(i).gameObject.name]);
                
                index = 0;                    
                GUIAgent.Instance.setSprite(parent.transform.GetChild(0).gameObject, ResourcesManager.Instance.sprites[parent.transform.GetChild(0).name + "_selected"]);
                
                cur = parent.transform.GetChild(index).gameObject;
            }
            else
            {
                GUIAgent.Instance.GuiObjects["UI/inventory/Panel/preview"].GetComponent<Image>().enabled = false;
                GUIAgent.Instance.GuiObjects["UI/inventory/Panel/preview/name"].GetComponent<Text>().enabled = false;
                cur = null;
            }
        });
    }

    void internalSelectToBeAssembled()
    {
        if (GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].activeSelf == false) return;

        var parent = GUIAgent.Instance.GuiObjects["UI/inventory/Panel/base/space"];

        if (parent.transform.childCount == 0) return;

        //Sound
        SoundManager.Instance.playSoundEffect("인벤토리_조합분해_통일", 0.1f);        

        toBeAssembled = parent.transform.GetChild(index).gameObject;           

        GUIAgent.Instance.setSprite(parent.transform.GetChild(index).gameObject, ResourcesManager.Instance.sprites[parent.transform.GetChild(index).name + "_assembled"]);
    }

    void internalAssemble()
    {
        if (GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].activeSelf == false) return;

        var parent = Utility.Instance.findChild("UI", "inventory/Panel/base/space");    
        
        if(Selected == null || toBeAssembled == null) return;
                
        var item = new List<string>();
        item.Add(Selected.name);
        item.Add(toBeAssembled.name);

        if (item.Contains("kinds_of_trees") && item.Contains("sling_grass"))
        {
            discardItem("kinds_of_trees");
            discardItem("sling_grass");
            getItem("sling1");
        }        
        if (item.Contains("stone1") && item.Contains("sling1"))
        {
            discardItem("stone1");
            discardItem("sling1");

            getItem("sling2");
        }
        if (item.Contains("stone2") && item.Contains("sling1"))
        {
            discardItem("stone2");
            discardItem("sling1");

            getItem("sling2");
        }
        if (item.Contains("stone3") && item.Contains("sling1"))
        {
            discardItem("stone3");
            discardItem("sling1");

            getItem("sling2");
        }

        if (item.Contains("matches") && item.Contains("spiderweb"))
        {
            discardItem("matches");
            discardItem("spiderweb");

            getItem("visitation_rights");
            getItem("key1");
        }

        if (item.Contains("matryoshka_doll1") && item.Contains("key1"))
        {         
            discardItem("key1");
            discardItem("matryoshka_doll1");
            getItem("matryoshka_doll1_used");
            getItem("matryoshka_doll2");
        }

        if (item.Contains("matryoshka_doll2") && item.Contains("key2"))
        {            
            discardItem("key2");
            discardItem("matryoshka_doll2");
            getItem("matryoshka_doll2_used");
            getItem("matryoshka_doll3");
        }

        item.Clear();
        Selected = null;
        toBeAssembled = null;

        //선택된 아이템 sprite 변경
        index = 0;
        for (int i = 0; i < parent.transform.childCount; i++)
            GUIAgent.Instance.setSprite(parent.transform.GetChild(i).gameObject.gameObject, ResourcesManager.Instance.sprites[parent.transform.GetChild(i).gameObject.name]);

        GUIAgent.Instance.setSprite(parent.transform.GetChild(index).gameObject, ResourcesManager.Instance.sprites[parent.transform.GetChild(index).name + "_selected"]);
        GUIAgent.Instance.setSprite("UI", "inventory/Panel/preview", ResourcesManager.Instance.sprites[parent.transform.GetChild(index).name]);
        GUIAgent.Instance.setText("UI", "inventory/Panel/preview/name", stage.getName(parent.transform.GetChild(index).gameObject.name));
    }

    void internalDisassemble()
    {
        if (GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].activeSelf == false) return;

        var parent = GUIAgent.Instance.GuiObjects["UI/inventory/Panel/base/space"];

        if (parent.transform.childCount == 0) return;

        SoundManager.Instance.playSoundEffect("분해", 0.1f);

        if (parent.transform.GetChild(index).name.CompareTo("stick") == 0)
        {
            parent.transform.GetChild(index).gameObject.SetActive(false);
            parent.transform.GetChild(index).SetParent(GUIAgent.Instance.GuiObjects["UI/item"].transform);

            getItem("kinds_of_trees");
            Selected = null;
            index = 0;
            for (int i = 0; i < parent.transform.childCount; i++)            
                GUIAgent.Instance.setSprite(parent.transform.GetChild(i).gameObject, ResourcesManager.Instance.sprites[parent.transform.GetChild(i).gameObject.name]);
            

            GUIAgent.Instance.setSprite(parent.transform.GetChild(0).gameObject, ResourcesManager.Instance.sprites[parent.transform.GetChild(index).gameObject.name + "_selected"]);
        }
    }

    void internalSelectItem()
    {
        if (GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].activeSelf == false) return;

        if(Utility.Instance.findChild("UI", "inventory/Panel/base").GetComponent<Image>().sprite == ResourcesManager.Instance.sprites["ui_base"])
        {
            GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].SetActive(false);
            GUIAgent.Instance.GuiObjects["UI/photo_albums/Panel"].SetActive(true);
            GUIAgent.Instance.GuiObjects["UI/preview/alarm_photo_albums"].SetActive(false);
            cur = null;
            return;
        }

        if (cur)
        {                        
            Selected = cur;
            if (toBeAssembled)
                assemble();
            else
            {
                AnimationAgent.Instance.Animators["Edmund"].Play("inventory2");
                GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].SetActive(false);
                GUIAgent.Instance.GuiObjects["UI/preview/image_item"].SetActive(true);
                GUIAgent.Instance.setSprite("UI", "preview/image_item", ResourcesManager.Instance.sprites[Selected.name]);
            }
        }
    }

    void internalChooseItem()
    {
        if (GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].activeSelf == false) return;

        var parent = GUIAgent.Instance.GuiObjects["UI/inventory/Panel/base/space"];
       
        if (parent.transform.childCount == 0)
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                SoundManager.Instance.playSoundEffect("인벤토리_이동");
                GUIAgent.Instance.setSprite("UI", "inventory/Panel/base", ResourcesManager.Instance.sprites["ui_base"]);
                GUIAgent.Instance.setSprite("UI", "inventory/Panel/photo_albums", ResourcesManager.Instance.sprites["ui_photo_albums_0_selected"]);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                SoundManager.Instance.playSoundEffect("인벤토리_이동");
                GUIAgent.Instance.setSprite("UI", "inventory/Panel/base", ResourcesManager.Instance.sprites["ui_base_selected"]);
                GUIAgent.Instance.setSprite("UI", "inventory/Panel/photo_albums", ResourcesManager.Instance.sprites["ui_photo_albums_0"]);                
            }
        }
        else
        {
            var prev = cur;
            var sprite = Utility.Instance.findChild("UI", "inventory/Panel/base").GetComponent<Image>().sprite;

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {                
                if (sprite == ResourcesManager.Instance.sprites["ui_base_selected"] && (index % 4 == 3 || index == parent.transform.childCount - 1))
                {
                    SoundManager.Instance.playSoundEffect("인벤토리_이동");
                    GUIAgent.Instance.setSprite("UI", "inventory/Panel/base", ResourcesManager.Instance.sprites["ui_base"]);
                    GUIAgent.Instance.setSprite("UI", "inventory/Panel/photo_albums", ResourcesManager.Instance.sprites["ui_photo_albums_0_selected"]);
                    GUIAgent.Instance.setSprite(prev, ResourcesManager.Instance.sprites[prev.name]);                         
                }
                else if(index < parent.transform.childCount - 1 && sprite == ResourcesManager.Instance.sprites["ui_base_selected"])
                    index++;
            }
            
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                if (sprite == ResourcesManager.Instance.sprites["ui_base"] && (index % 4 == 3 || index == parent.transform.childCount - 1))
                {
                    SoundManager.Instance.playSoundEffect("인벤토리_이동");
                    GUIAgent.Instance.setSprite("UI", "inventory/Panel/base", ResourcesManager.Instance.sprites["ui_base_selected"]);
                    GUIAgent.Instance.setSprite("UI", "inventory/Panel/photo_albums", ResourcesManager.Instance.sprites["ui_photo_albums_0"]);
                    GUIAgent.Instance.setSprite(prev, ResourcesManager.Instance.sprites[prev.name + "_selected"]);                    
                }
                else if(index > 0 && index != 4 && sprite == ResourcesManager.Instance.sprites["ui_base_selected"])
                    index--;
            }
            if (Input.GetKeyUp(KeyCode.UpArrow) && index - 4 > -1 && sprite == ResourcesManager.Instance.sprites["ui_base_selected"])
            {
                index -= 4;
            }
            if (Input.GetKeyUp(KeyCode.DownArrow) && index + 4 < parent.transform.childCount && sprite == ResourcesManager.Instance.sprites["ui_base_selected"])
            {
                index += 4;
            }

            if(sprite == ResourcesManager.Instance.sprites["ui_base_selected"])
                cur = parent.transform.GetChild(index).gameObject;

            if(prev && prev != cur)
            {
                SoundManager.Instance.playSoundEffect("인벤토리_이동");
                if (!prev.GetComponent<Image>().sprite.name.Contains("assembled"))
                    GUIAgent.Instance.setSprite(prev, ResourcesManager.Instance.sprites[prev.name]);
                if (!cur.GetComponent<Image>().sprite.name.Contains("assembled"))
                    GUIAgent.Instance.setSprite(cur, ResourcesManager.Instance.sprites[cur.name + "_selected"]);
                GUIAgent.Instance.setSprite("UI", "inventory/Panel/preview", ResourcesManager.Instance.sprites[cur.name]);
                GUIAgent.Instance.setText("UI", "inventory/Panel/preview/name", stage.getName(cur.name));
                
            }
        }
    }

    void internalCloseInventory()        
    {
        if (GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].activeSelf == false && Selected)
        {
            Selected = null;
            GUIAgent.Instance.GuiObjects["UI/preview/image_item"].SetActive(false);
            return;
        }
        else if(GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].activeSelf == false && !Selected)
        {
            GUIAgent.Instance.GuiObjects["UI/Menu"].SetActive(true);
            return;
        }

        if (toBeAssembled && toBeAssembled.GetComponent<Image>().sprite.name.Contains("assembled"))
        {
            toBeAssembled.GetComponent<Image>().sprite = ResourcesManager.Instance.sprites[toBeAssembled.name];
            toBeAssembled = null;
        }
        else
        {
            AnimationAgent.Instance.Animators["Edmund"].Play("inventory2");
            GUIAgent.Instance.GuiObjects["UI/inventory/Panel"].SetActive(false);
            Utility.Instance.findChild("UI", "inventory/Panel/Info").SetActive(false);
        }
    }

    #endregion private method
}
