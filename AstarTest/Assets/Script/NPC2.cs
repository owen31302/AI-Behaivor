using UnityEngine;
using System.Collections;

public class NPC2 : MonoBehaviour {

    int countDown = 1;

    // Use this for initialization
    void Start () {
		
        
    }
	
	// Update is called once per frame
	void Update () {
        
<<<<<<< HEAD
        if (AIBehaivor2.seek(this.gameObject, Astar.mAIdata2) && countDown < Astar.length) {
=======
        if (AIBehaivor2.seek(this.gameObject, Astar.mAIdata2)) {
>>>>>>> origin/master
            int index = Astar.findIndex(Astar.wayPoints[Astar.length - 1 - countDown].x, Astar.wayPoints[Astar.length - 1 - countDown].y);
            Astar.mAIdata2.tarTrams = BuildGrid.FindingGrid.GetCellCenter(index);
            countDown++;
        }
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);

        // draw forwardCollisionProbe
        if (Astar.mAIdata2 != null)
        {
            Gizmos.color = Color.blue;
            Vector3 origin = this.transform.position;
            Vector3 right = this.transform.right * Astar.mAIdata2.boundingSphereRadius;
            Vector3 forward = this.transform.forward * Astar.mAIdata2.forwardCollisionProbeLength;
            Gizmos.DrawLine(origin + right, origin + right + forward);
            Gizmos.DrawLine(origin - right, origin - right + forward);
            Gizmos.DrawLine(origin + right + forward, origin - right + forward);
        }
    }
}
