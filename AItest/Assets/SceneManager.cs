using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public Obstacle [] m_Obs = null;

	public static SceneManager m_Instance;

	void Awake () {
		m_Instance = this;
		GameObject [] gos = GameObject.FindGameObjectsWithTag ("Obstacle");
		int inLen = gos.Length;
		//Debug.Log (inLen);
		m_Obs = new Obstacle[inLen];
		for (int i = 0; i < inLen; i++) {
			m_Obs[i] = gos[i].GetComponent<Obstacle>();
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
