using UnityEngine;
using System.Collections;

public class NPC2 : MonoBehaviour {

	public GameObject target;
	AIdata2 mAIdata2;

	// Use this for initialization
	void Start () {
		mAIdata2 = new AIdata2 ();
		mAIdata2.target = target;
		mAIdata2.m_Obs = SceneManager.m_Instance.m_Obs;
	}
	
	// Update is called once per frame
	void Update () {
		if (!AIBehaivor2.obstacleAvoidance (this.gameObject, mAIdata2)) {
			AIBehaivor2.seek (this.gameObject, mAIdata2);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine (this.transform.position, this.transform.position + this.transform.forward * 2.0f);

		// draw forwardCollisionProbe
		if( mAIdata2 != null){
			Gizmos.color = Color.blue;
			Vector3 origin = this.transform.position;
			Vector3 right =  this.transform.right * mAIdata2.boundingSphereRadius;
			Vector3 forward = this.transform.forward * mAIdata2.forwardCollisionProbeLength;
			Gizmos.DrawLine (origin + right, origin + right + forward);
			Gizmos.DrawLine (origin -right, origin -right + forward);
			Gizmos.DrawLine (origin + right + forward, origin -right + forward);
		}
	}
}
