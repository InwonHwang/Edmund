using UnityEngine;
using System.Collections;

public class PhotoAlbums : MonoBehaviour {

    internal bool HasPhoto { get; set; }

	// Use this for initialization
	void Start () {
        GUIAgent.Instance.GuiObjects["UI/photo_albums/Panel"].SetActive(false);
        GUIAgent.Instance.setSprite("UI/photo_albums/Panel/photo_albums", ResourcesManager.Instance.sprites["ui_photo_albums_1"]);
    }
	
	// Update is called once per frame
	void Update () {
        turnAPage();

        if (GUIAgent.Instance.GuiObjects["UI/photo_albums/Panel"].activeSelf && Input.GetKeyUp(KeyCode.Escape))
        {
            AnimationAgent.Instance.Animators["Edmund"].Play("inventory2");
            GUIAgent.Instance.GuiObjects["UI/photo_albums/Panel"].SetActive(false);
        }
    }

    void turnAPage()
    {
        if (HasPhoto && GUIAgent.Instance.GuiObjects["UI/photo_albums/Panel"].activeSelf && Input.GetKeyUp(KeyCode.RightArrow) )
        {
            SoundManager.Instance.playSoundEffect("페이지넘기기");
            GUIAgent.Instance.setSprite("UI/photo_albums/Panel/photo_albums", ResourcesManager.Instance.sprites["ui_photo_albums_1"]);
        }
        if (HasPhoto && GUIAgent.Instance.GuiObjects["UI/photo_albums/Panel"].activeSelf && Input.GetKeyUp(KeyCode.LeftArrow))
        {
            SoundManager.Instance.playSoundEffect("페이지넘기기");
            GUIAgent.Instance.setSprite("UI/photo_albums/Panel/photo_albums", ResourcesManager.Instance.sprites["ui_photo_albums_2"]);            
        }
    }
}
