using UnityEngine;
using System.Collections;

public class MiniGame : MonoBehaviour {
    internal bool Clear { get; set; }
    internal bool Active { get; set; }

    protected virtual void OnEnable()
    {
        Active = true;
    }

}
