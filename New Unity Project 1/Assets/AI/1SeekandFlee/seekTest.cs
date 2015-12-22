using UnityEngine;
using System.Collections;

public class seekTest : MonoBehaviour {

    public Transform target;
    private float Vmax;
    private Rigidbody myrb;

    // Use this for initialization
    void Start () {
        myrb = this.gameObject.GetComponent<Rigidbody>();
        //myrb.velocity = new Vector3(0, 0, -1)*2;
        Vmax = 20;
        //myrb.AddForce( new Vector3(1,0,0), ForceMode.VelocityChange);
    }
	
	// Update is called once per frame
	void Update () {

        // 有兩種方式
        // 一種是利用 transform.position 見 seekAction4();
        // 另一種利用 rigidbody.velocity 見 seekAction3();
        seekAction4();
    }

    // define velocity and position
    void seekAction4() {
        Vector3 v1 = target.transform.position - this.transform.position;
        float d = v1.magnitude;
        v1.Normalize();
        Vector3 v = Vector3.RotateTowards(this.transform.forward, v1, 0.1f, 0.1f);
        this.transform.forward = v.normalized;
        if (d < 0.1f)
        {
            this.transform.position = target.transform.position;
        }
        else {
            this.transform.position = this.transform.position + this.transform.forward * 0.1f;
        }
    }
    //如果使用老師的系統，未來的速度就必須一致綁在物體上，並且是用位移來表示速度

    // use rigidbody
    private void seekAction3()
    {
        float turn_interval = 0.01f; // turning force
        float accelerate_interval = 0.01f; // accelerate or deaccelerate

        Vector3 Vnorm = target.position - myrb.transform.position;
        if (Vnorm.magnitude > 0.2) //0.4為測試出來的，不知道怎麼算
        {
            Vnorm.Normalize();
            Debug.Log("displacement norm: " + Vnorm);
            Vector3 steering = Vnorm * Vmax - myrb.velocity;

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
                Debug.Log("forward: "+myrb.transform.forward);
                myrb.velocity = v_turn.normalized * myrb.velocity.magnitude;
                myrb.velocity = myrb.velocity + myrb.velocity.normalized * accelerate * accelerate_interval;
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

    // 改版: 角度到某個範圍內就會修正成一樣的角度
    private void seekAction5()
    {
        float maxRadiansDelta = 0.01f; // turning force, 太小竟然會誤差更大喔(0.005f 會錯很大)
        float turn_interval_threshold = 0.2f; //depend on turn_interval
        float accelerate_interval = 0.01f; // accelerate or deaccelerate
        float accelerate_interval_threshold = 0.2f; // depend on turn_interval and accelerate_interval

        Vector3 Vnorm = target.position - myrb.transform.position;
        if (Vnorm.magnitude > accelerate_interval_threshold) //0.4為測試出來的，不知道怎麼算
        {
            Vnorm.Normalize();
            Vector3 steering = Vnorm * Vmax - myrb.velocity;

            // if initial velocity is zero, using this formula will fail.
            // Therefore, we need to set the ini velocity if vel = 0.
            if (myrb.velocity.magnitude == 0)
            {
                myrb.velocity = myrb.transform.forward;
            }
            else {
                Vector3 myrbvnorm = myrb.velocity.normalized;
                float accelerate = Vector3.Dot(steering, myrbvnorm);
                Vector3 v_turn;
                if ((steering - accelerate * myrbvnorm).magnitude > turn_interval_threshold)
                {
                    Vector3 newDir = Vector3.RotateTowards(myrb.velocity, steering - accelerate * myrbvnorm, maxRadiansDelta, 0.0f);
                    v_turn = newDir;
                }
                else {
                    v_turn = Vnorm;
                }
                // v = v0 + at, here, t = 1/30

                myrb.transform.forward = v_turn.normalized;
                myrb.velocity = v_turn.normalized * myrb.velocity.magnitude;
                myrb.velocity = myrb.velocity + myrb.velocity.normalized * accelerate * accelerate_interval;
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

    private void seekAction2()
    {
        // 發生角度沒追好，Vnorm.magnitude 到不了0.1以下，所以轉個彎再一次才追到
        // 應該設一個比這個步階大一點點的範圍(R)，如果物體角度轉到落於這個範圍內(目標角度 +- R)
        // 就令物體角度等於目標角度
        Vector3 Vnorm = target.position - myrb.transform.position;
        if (Vnorm.magnitude > 0.1)
        {
            Vnorm.Normalize();
            Vector3 steering = Vnorm * Vmax - myrb.velocity;

            // if initial velocity is zero, using this formula will fail.
            // Therefore, we need to set the ini velocity if vel = 0.
            if (myrb.velocity.magnitude == 0)
            {
                myrb.velocity = myrb.transform.forward;
            }
            else {
                Vector3 myrbvnorm = myrb.velocity.normalized;
                float accelerate = Vector3.Dot(steering, myrbvnorm);
                Vector3 v_turn = myrb.velocity + (steering - accelerate * myrbvnorm) * 0.01f;
                    // v = v0 + at, here, t = 1/30
                myrb.transform.forward = v_turn.normalized;
                myrb.velocity = v_turn.normalized * myrb.velocity.magnitude;
                myrb.velocity = myrb.velocity + myrb.velocity.normalized * accelerate * 0.01f;
                    // v = v0 + at, here, t = 1/30
            }
        }
        else {
            // avoid Oscillation
            myrb.velocity = new Vector3(0, 0, 0);
        }
    }

    // I found Rigidbody.transform.forward is not aligned with local Rigidbody.transform.velocity
    // 也就是速度方向與面相方向不一致
    // Rigidbody.transform.velocity 控制物體座標的移動(但座標方向不會與速度方向一致)
    // Rigidbody.transform.forward 控制物體的面相
    private void seekAction() {
        Vector3 Vnorm = target.position - myrb.transform.position;
        Debug.Log("0.1");
        Debug.Log(target.position);
        Debug.Log("0.2");
        Debug.Log(myrb.transform.position);
        Debug.Log("0.3");
        Debug.Log(Vnorm);
        if (Vnorm.magnitude > 0.01) {
            //Vnorm = Vnorm / Vnorm.magnitude;
            Vnorm.Normalize();
            //Debug.Log(Vnorm);
            Vector3 m_vel = myrb.velocity;
            Vector3 steering = Vnorm * Vmax - m_vel;
            Debug.Log('1');
            Debug.Log(Vnorm);
            Debug.Log('2');
            Debug.Log(Vmax);
            Debug.Log('3');
            Debug.Log(Vnorm* Vmax);
            Debug.Log('4');
            Debug.Log(myrb.velocity);
            Debug.Log('5');
            Debug.Log(steering.magnitude);
            m_vel.Normalize();
            float accelerate = Vector3.Dot(steering, m_vel);
            //float accelerate = Vector3.Dot(steering, myrb.velocity/ myrb.velocity.magnitude);
            Debug.Log('6');
            Debug.Log(steering);
            Debug.Log('7');
            Debug.Log(m_vel);
            Debug.Log('8');
            Debug.Log(accelerate);
            Debug.Log('9');
            Debug.Log(myrb.velocity);
            //myrb.velocity = myrb.velocity + steering;
            myrb.velocity = myrb.velocity + myrb.velocity.normalized * accelerate;

            /*Debug.Log('2');
            Debug.Log(m_vel );
            Debug.Log(accelerate);
            Debug.Log('3');
            Debug.Log(myrb.velocity);*/
            Vector3 v = myrb.velocity + steering * 0.1f;
            myrb.transform.forward = v.normalized;
            //Debug.Log(this.transform.forward);
        }

    }
    
    //老師的作法
    void Update2()
    {
        /*Vector3 v1 = m_trans.transform.position - this.transform.position;
        float d = v1.magnitude;
        v1.Normalize();
        Vector3 v = Vector3.RotateTowards(this.transform.forward, v1, 0.1f, 0.1f);
        this.transform.forward = v.normalized;
        if (d < 0.1f)
        {
            this.transform.position = m_trans.transform.position;
        }
        else {
            this.transform.position = this.transform.position + this.transform.forward * 0.1f;
        }*/
    }

}
