using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Menu : VerticalMenu {
	
    void OnEnable()
    {
        Time.timeScale = 0;
    }

	protected override void Start () {
        base.Start();

        _bar = transform.FindChild("Bar").gameObject;
        _items.Add(0, new Tuple<GameObject, UnityAction>(transform.FindChild("Option").gameObject, () => {
            Utility.Instance.findChild("UI", "Option").SetActive(true);
            gameObject.SetActive(false);
        }));
        _items.Add(1, new Tuple<GameObject, UnityAction>(transform.FindChild("Quit").gameObject, () =>
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;            
        }));
    }	
	
}
