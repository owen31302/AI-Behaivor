using UnityEngine;
using System.Collections;

public class obstacleAvoidance : MonoBehaviour {

    private BoxCollider mBoxCollider;
    Rigidbody myrb;

    // Use this for initialization
    void Start () {
        mBoxCollider = this.gameObject.GetComponentsInChildren<BoxCollider>()[1];
        myrb = this.GetComponent<Rigidbody>();
        myrb.velocity = myrb.transform.forward;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("1: "+mBoxCollider.name);
    }

    // 碰撞問題搞了好久
    // http://www.coolsun.idv.tw/modules/xhnewbb/viewtopic.php?topic_id=1532
    // is trigger 打開就會讓物體穿過去
    // 無法用來空物件的物體碰撞偵測
    /*void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log(this.gameObject.name);
    }*/

    void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        // 我用flee來完成這件事
        flee2(other.transform);
    }

    void flee2(Transform target)
    {
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
            if (myrb.velocity.magnitude == 0)
            {
                myrb.velocity = v_turn * accelerate_factor;
            }
            else
            {
                myrb.velocity = v_turn * myrb.velocity.magnitude;
            }
            //myrb.velocity = myrb.velocity + myrb.transform.forward * accelerate * accelerate_factor;
            // v = v0 + at, here, t = 1/30

        }
    }

    void OnDrawGizmos()
    {
        //Debug.Log("G");
        Gizmos.color = Color.green; //換色
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward*3.5f); //畫出正面線
        Gizmos.DrawLine(this.transform.position + this.transform.right * 0.5f, this.transform.position + this.transform.forward * 3.5f+ this.transform.right*0.5f);
        Gizmos.DrawLine(this.transform.position - this.transform.right * 0.5f, this.transform.position + this.transform.forward * 3.5f - this.transform.right * 0.5f);
        Gizmos.DrawLine(this.transform.position + this.transform.forward * 3.5f + this.transform.right * 0.5f, this.transform.position + this.transform.forward * 3.5f - this.transform.right * 0.5f);
    }
}
