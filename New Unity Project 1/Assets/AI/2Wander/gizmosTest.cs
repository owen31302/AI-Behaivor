using UnityEngine;
using System.Collections;

public class gizmosTest : MonoBehaviour {

    private Vector3 center;
    private float radius;
    private Vector3 rand_center;
    private float rand_theta;
    private int count;

    // Use this for initialization
    void Start () {
        rand_theta = 0.0f;
        count = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("U");
        rand_position();
	}

    void rand_position() {
        if (count == 10)
        {
            float rand = (Random.value - 0.5f)*2;
            rand_theta += rand*20;
            Debug.Log(rand_theta);
            if (rand_theta >= 360) {
                rand_theta -= 360;
            } else if (rand_theta <= -360) {
                rand_theta += 360;
            }
            count++;
        }
        else if (count > 29)
        {
            count = 0;
        }
        else {
            count++;
        }
    }

    void OnDrawGizmos()
    {
        //Debug.Log("G");
        radius = 2.0f;
        center = this.transform.position + this.transform.forward * 4.0f;
        rand_center = center + new Vector3(radius * Mathf.Sin(Mathf.PI * rand_theta / 180), 0, radius * Mathf.Cos(Mathf.PI * rand_theta / 180)); ;
        Gizmos.color = Color.green; //換色
        Gizmos.DrawLine(this.transform.position, center); //畫出正面線
        Gizmos.DrawWireSphere(center, radius);
        Gizmos.DrawSphere(rand_center, 0.1f);

    }
}
