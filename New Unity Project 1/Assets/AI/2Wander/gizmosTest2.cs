using UnityEngine;
using System.Collections;

public class gizmosTest2 : MonoBehaviour {

    private Vector3 center;
    public static float distance = 4.0f;
    public static float radius = 3.0f;
    public static Vector3 rand_center;
    private float rand_theta;
    private int count;

    // Use this for initialization
    void Start()
    {
        rand_theta = 0.0f;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("U");
        rand_position();
    }

    void rand_position()
    {
        if (count == 10)
        {
            float rand = (Random.value - 0.5f) * 2;
            rand_theta += rand * 40;
            //Debug.Log(rand_theta);
            if (rand_theta >= 360)
            {
                rand_theta -= 360;
            }
            else if (rand_theta <= -360)
            {
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
        center = this.transform.position + this.transform.forward * distance;
        rand_center = center + new Vector3(radius * Mathf.Sin(Mathf.PI * rand_theta / 180), 0, radius * Mathf.Cos(Mathf.PI * rand_theta / 180)); ;
        Gizmos.color = Color.green; //換色
        Gizmos.DrawLine(this.transform.position, center); //畫出正面線
        Gizmos.DrawWireSphere(center, radius);
        Gizmos.DrawSphere(rand_center, 0.1f);

    }
}
