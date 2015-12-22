using UnityEngine;
using System.Collections;

public class fleeTest : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        flee2();
    }

    void flee1() {
        Vector3 v1 = target.transform.position - this.transform.position;
        float d = v1.magnitude;
        v1.Normalize();
        Vector3 v = Vector3.RotateTowards(this.transform.forward, -v1, 0.1f, 0.1f);
        this.transform.forward = v.normalized;
        if (d < 1000.0f){
            this.transform.position = this.transform.position + this.transform.forward * 0.1f;
        }
    }

    public Rigidbody myrb;
    void flee2() {
        float Vmax = 5;
        float turn_factor = 0.01f; // turning force
        float accelerate_factor = 0.01f; // accelerate or deaccelerate

        Vector3 Vnorm = target.position - myrb.transform.position;
        if (Vnorm.magnitude < 1000.0f) 
        {
            Vnorm = -Vnorm;
            Vector3 steering = Vnorm.normalized * Vmax - myrb.velocity;

            Vector3 facing = myrb.transform.forward;
            float accelerate = Vector3.Dot(steering, facing);
            Vector3 v_turn = facing + (steering - accelerate * facing) * turn_factor;
            v_turn.Normalize();
            // v = v0 + at, here, t = 1/30
            myrb.transform.forward = v_turn;
            if (myrb.velocity.magnitude == 0) {
                myrb.velocity = v_turn * accelerate_factor;
            }
            else
            {
                myrb.velocity = v_turn * myrb.velocity.magnitude;
            }
            myrb.velocity = myrb.velocity + myrb.transform.forward * accelerate * accelerate_factor;
            // v = v0 + at, here, t = 1/30

        }
    }
}
