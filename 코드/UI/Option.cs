using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Option : VerticalMenu
{
    UnityEngine.UI.Slider _soundSlider;
    GameObject _menu;

    protected override void Start()
    {
        base.Start();

        _soundSlider = Utility.Instance.findChild("UI", "Option/SoundSlider").GetComponent<UnityEngine.UI.Slider>();
        _menu = Utility.Instance.findChild("UI", "Menu");
        _bar = transform.FindChild("Bar").gameObject;
        _items.Add(0, new Tuple<GameObject, UnityAction>(transform.FindChild("WindowSize").gameObject, null));
        _items.Add(1, new Tuple<GameObject, UnityAction>(transform.FindChild("1920x1080").gameObject, () => { Screen.SetResolution(1920, 1080, true); }));
        _items.Add(2, new Tuple<GameObject, UnityAction>(transform.FindChild("1600x900").gameObject, () => { Screen.SetResolution(1600, 900, false); }));
        _items.Add(3, new Tuple<GameObject, UnityAction>(transform.FindChild("1366x768").gameObject, () => { Screen.SetResolution(1366, 768, false); }));
        _items.Add(4, new Tuple<GameObject, UnityAction>(transform.FindChild("1360x768").gameObject, () => { Screen.SetResolution(1360, 768, false); }));
        _items.Add(5, new Tuple<GameObject, UnityAction>(transform.FindChild("1280x720").gameObject, () => { Screen.SetResolution(1280, 720, false); }));
        _items.Add(6, new Tuple<GameObject, UnityAction>(transform.FindChild("Sound").gameObject, null));
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Tuple<GameObject, UnityAction> temp;
            _items.TryGetValue(_index, out temp);
            if (temp.First.name.CompareTo("Sound") == 0)
            {                
                _soundSlider.value += 0.01f;
                SoundManager.Instance.setVolume();
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Tuple<GameObject, UnityAction> temp;
            _items.TryGetValue(_index, out temp);
            if (temp.First.name.CompareTo("Sound") == 0)
            {
                _soundSlider.value -= 0.01f;
                SoundManager.Instance.setVolume();
            }
        }
        else if(Input.GetKey(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            _menu.SetActive(true);
        }
    }
}