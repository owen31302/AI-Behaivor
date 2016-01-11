using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (this.transform.position, 1.0f);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine (this.transform.position, this.transform.position+this.transform.forward*2.0f);
	}
}
