using UnityEngine;
using System.Collections;

public class seek : MonoBehaviour {

    private Vector3 target;
    private float Vmax;
    private Rigidbody myrb;
    private float distance;
    private float radius;

    // Use this for initialization
    void Start () {
        // check rigidbody
        if (this.gameObject.GetComponent<Rigidbody>() == null)
        {
            myrb = this.gameObject.AddComponent<Rigidbody>();
            myrb.useGravity = false;
        }
        else {
            myrb = this.gameObject.GetComponent<Rigidbody>();
        }

        //center
        distance = gizmosTest2.distance; 
        radius = gizmosTest2.radius; 

        //set Vmax
        Vmax = 5;
    }
	
	// Update is called once per frame
	void Update () {
        target = gizmosTest2.rand_center;
        seekAction();
    }

    private void seekAction()
    {
        float turn_interval = 0.01f; // turning force
        float accelerate_interval = 0.01f; // accelerate or deaccelerate

        Vector3 Vnorm = target - myrb.transform.position;
        if (Vnorm.magnitude > 0.2) //0.4為測試出來的，不知道怎麼算
        {
            Vnorm.Normalize();
            //Debug.Log("displacement norm: " + Vnorm);
            /*Debug.Log("1: "+radius);
            Debug.Log("2: " + distance);
            Debug.Log("3: " + target);
            Debug.Log("4: " + this.transform.position);
            Debug.Log("5: " + (target - this.transform.position).magnitude);
            Debug.Log("6: " + (distance + radius));
            Debug.Log("7: "+(target - this.transform.position).magnitude / (distance + radius));*/
            float Vmax2 = Vmax * (target - this.transform.position).magnitude / (distance + radius);
            Vector3 steering = Vnorm * Vmax2  - myrb.velocity;

            // if initial velocity is zero, using this formula will fail.
            // Therefore, we need to set the ini velocity if vel = 0.
            if (myrb.velocity.magnitude == 0)
            {
                myrb.velocity = myrb.transform.forward;
            }
            else {
                Vector3 myrbvnorm = myrb.velocity.normalized;
                float accelerate = Vector3.Dot(steering, myrbvnorm);
                Vector3 v_turn = myrb.velocity + (steering - accelerate * myrbvnorm) * turn_interval;
                // v = v0 + at, here, t = 1/30
                myrb.transform.forward = v_turn.normalized;
                //Debug.Log("forward: " + myrb.transform.forward);
                myrb.velocity = v_turn.normalized * myrb.velocity.magnitude;
                myrb.velocity = myrb.velocity + myrb.velocity.normalized * accelerate * accelerate_interval;
                // v = v0 + at, here, t = 1/30
                //Debug.Log("vel: "+ myrb.velocity.magnitude);
            }
        }
        else {
            // avoid Oscillation
            Debug.Log("arrived");
            myrb.velocity = new Vector3(0, 0, 0);
            myrb.transform.position = target;
        }
    }
}
