using UnityEngine;
using System.Collections;

public class gizmos : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green; //換色
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 5.0f); //畫出正面線
        Gizmos.DrawWireSphere(this.transform.position, 30.0f); //畫出球狀
    }
}
