using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
	public float m_fRadius = 1.0f;
	// Use this for initialization
	void Start () {
	
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (this.transform.position, 1.0f);
	}
}
