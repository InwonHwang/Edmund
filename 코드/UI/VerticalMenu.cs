using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class VerticalMenu : MonoBehaviour {

    public class Tuple<T0, T1>
    {
        public Tuple(T0 first, T1 second)
        {
            First = first;
            Second = second;
        }
        public T0 First { get; set; }
        public T1 Second { get; set; }
    }

    protected Dictionary<int, Tuple<GameObject, UnityAction>> _items;
    protected GameObject _bar;
    protected int _index;
    	
	protected virtual void Start ()
    {
        _items = new Dictionary<int, Tuple<GameObject, UnityAction>>();
        _index = 0;
    }	
	
	protected virtual void Update ()
    {
	    if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (_index > 0) _index--;            
            if (_bar)
            {
                Tuple<GameObject, UnityAction> temp;
                _items.TryGetValue(_index, out temp);
                _bar.transform.position = new Vector3(_bar.transform.position.x, temp.First.transform.position.y, _bar.transform.position.z);
            }
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (_index < _items.Count - 1) _index++;
            if (_bar)
            {
                Tuple<GameObject, UnityAction> temp;
                _items.TryGetValue(_index, out temp);
                _bar.transform.position = new Vector3(_bar.transform.position.x, temp.First.transform.position.y, _bar.transform.position.z);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            Tuple<GameObject, UnityAction> temp;
            _items.TryGetValue(_index, out temp);
            if(temp.Second != null) temp.Second();
        }
    }
}
