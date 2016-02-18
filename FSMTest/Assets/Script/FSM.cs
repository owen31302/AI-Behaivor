using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour {

    public GameObject Target;


    enum eNPCState
    {
        NONE = -1,
        IDLE = 0,
        CHASE,
        ATTACK
    }

    private GameObject thisGameObject;
    private eNPCState _currentState;
    private AIdata2 _AIdata2;

    void Start () {
        _currentState = eNPCState.IDLE;
        _AIdata2 = new AIdata2();
        thisGameObject = gameObject;
    }
	
	
	void Update () {
        
        // check current state
        // do state
        // transition
        switch (_currentState) {
            case eNPCState.IDLE:
                // do idle

                // check IDLE state
                if (CheckEnemy()) {
                    _currentState = eNPCState.CHASE;
                }
                Debug.Log("Current State: " + _currentState);
                break;

            case eNPCState.CHASE:
                // do chase
                _AIdata2.tarTrams = Target.transform;
                bool reachGoal = false;
                reachGoal = AIBehaivor2.seek( gameObject, _AIdata2);

                // check CHASE state
                if (!CheckEnemy())
                {
                    _currentState = eNPCState.IDLE;
                } else if (reachGoal) {
                    _currentState = eNPCState.ATTACK;
                }
                Debug.Log("Current State: " + _currentState);
                break;

            case eNPCState.ATTACK:
                // do attack
                Color c = Color.black;
                c.r = Random.Range(0.0f, 1.0f);
                c.g = Random.Range(0.0f, 1.0f);
                c.b = Random.Range(0.0f, 1.0f);
                this.GetComponent<Renderer>().material.color = c;

                // check ATTACK state
                if (!AIBehaivor2.seek( thisGameObject, _AIdata2))
                {
                    _currentState = eNPCState.CHASE;
                }
                else if (!CheckEnemy()) {
                    _currentState = eNPCState.IDLE;
                }
                Debug.Log("Current State: " + _currentState);
                break;
        }
	}

    bool CheckEnemy() {
        float detectLength = 5.0f;

        // check distance
        Vector3 vdistance = Target.transform.position - this.transform.position;
        if (vdistance.magnitude > detectLength) return false;

        // check angle
        float dotResult = Vector3.Dot(vdistance.normalized, this.transform.forward);
        if (dotResult < 0) return false;

        return true;
    }
}
