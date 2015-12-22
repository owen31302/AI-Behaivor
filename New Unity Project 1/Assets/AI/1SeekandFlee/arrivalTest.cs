using UnityEngine;
using System.Collections;

public class arrivalTest : MonoBehaviour
{

    public Transform target;
    private float Vmax;
    public Rigidbody myrb;

    // Use this for initialization
    void Start()
    {
        Vmax = 20;
    }

    // Update is called once per frame
    void Update()
    {
        arriva1();
    }

    // 注意，因為我現在的設計，物體的面向被速度方向所決定
    // 公式算出來的速度結果如果是負的，我的面向會忽然180度轉
    // 所以我目前的設計，只能用在沒有面向的東西上面。
    // 如果要放在遊戲AI人物，要設計只能往前轉彎
    private void arriva1()
    {
        float maxRadiansDelta = 0.01f; // turning force, 太小竟然會誤差更大喔(0.005f 會錯很大)
        float turn_interval_threshold = 0.2f; //depend on turn_interval
        float accelerate_interval = 0.05f; // accelerate or deaccelerate
        float accelerate_interval_threshold = 0.2f; // depend on turn_interval and accelerate_interval
        float slowing_distance = 20.0f;

        Vector3 target_offset = target.position - myrb.transform.position;
        float distance = target_offset.magnitude;
        if (distance > accelerate_interval_threshold) //0.4為測試出來的，不知道怎麼算
        {

            // if initial velocity is zero, using this formula will fail.
            // Therefore, we need to set the ini velocity if vel = 0.
            if (myrb.velocity.magnitude == 0)
            {
                myrb.velocity = myrb.transform.forward*1;
            }
            else
            {
                float ramped_speed = Vmax * (distance / slowing_distance);
                float clipped_speed = minimum(ramped_speed, Vmax);
                Vector3 desired_velocity = clipped_speed * target_offset.normalized;
                Vector3 steering = desired_velocity - myrb.velocity;

                Vector3 vel_norm = myrb.velocity.normalized;
                float accelerate = Vector3.Dot(steering, vel_norm);
                Vector3 v_turn;
                //Debug.Log((steering - accelerate * myrbvnorm).magnitude);
                if ((steering - accelerate * vel_norm).magnitude > turn_interval_threshold)
                {
                    v_turn = Vector3.RotateTowards(myrb.velocity, steering - accelerate * vel_norm, maxRadiansDelta, 0.0f);
                    Debug.Log("2: " + v_turn);
                }
                else {
                    v_turn = target_offset;
                }
                // v = v0 + at, here, t = 1/30

                myrb.transform.forward = v_turn.normalized;
                Debug.Log("1: "+v_turn.normalized);
                myrb.velocity = v_turn.normalized * myrb.velocity.magnitude;
                myrb.velocity = myrb.velocity + myrb.velocity.normalized * accelerate * accelerate_interval;
                //Debug.Log(myrb.velocity.magnitude);
                // v = v0 + at, here, t = 1/30
            }
        }
        else {
            // avoid Oscillation
            Debug.Log("arrived");
            myrb.velocity = new Vector3(0, 0, 0);
            myrb.transform.position = target.transform.position;
        }
    }

    float minimum(float v1, float v2) {
        if (v1 <= v2) {
            return v1;
        }
        return v2;
    }
}