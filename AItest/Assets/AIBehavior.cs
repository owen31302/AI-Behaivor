using UnityEngine;
using System.Collections;


public class AIData
{
	public float fSpeed = 0.1f;
	public float fMaxSpeed = 2.0f; 
	public float fCurrentR = 0.0f;
	public float fMaxRotate = 1.0f;
	public float fColProbe = 3.0f;
	public float fRadius = 1.0f;
	public GameObject targetPoint;
	public Obstacle [] m_Obs;
}

public class AIBehavior {

	public static bool SeekBehavior (GameObject go, AIData data)
	{
		Vector3 tpos = data.targetPoint.transform.position;
		Vector3 cPos = go.transform.position;
		Vector3 tVec = tpos - cPos;
		tVec.y = 0.0f;
		float fDist = tVec.magnitude;
		if (fDist <= data.fSpeed*Time.deltaTime) {
			go.transform.position = tpos;
			data.fSpeed = 0.0f;
			return false;
		}

		tVec.Normalize ();
		Vector3 vFor = go.transform.forward;

		/*Vector3 vRotate = tVec - vFor;
		float fdist = vRotate.magnitude;
		if (fdist < fMaxRotate) {
			this.transform.forward = tVec;
		} else {
			vRotate.Normalize ();
			fCurrentR += 0.01f;
			if(fCurrentR > fMaxRotate) {
				fCurrentR = fMaxRotate;
			}
			vFor = vFor + vRotate*fCurrentR;
			vFor.Normalize ();
			this.transform.forward = vFor;
		}
*/

		//Vector3 vRotate = this.transform.right*fSign;


		// Rotate by degree.

		float fDot = Vector3.Dot (tVec, vFor);
		if (fDot < -1.0f) {
			fDot = -1.0f;
		} else if (fDot > 1.0f) {
			fDot = 1.0f;
		}
		float fForwardForce = fDot;
		float fTheta = Mathf.Acos (fDot);
		float fTurnForce = Mathf.Sin(fTheta);
		float fDegree = fTheta * Mathf.Rad2Deg;
		//Debug.Log("BBb" + fDegree + " : " + fMaxRotate + ":DD" + fTheta);
		if (fDegree < data.fMaxRotate) {
			go.transform.forward = tVec;
			//Debug.Log("AAA");
		} else {

			float fSign = fTurnForce;
			Vector3 vC = Vector3.Cross (vFor, tVec);
			if (vC.y < 0) {
				fSign = -fTurnForce;
			}
			if(data.fCurrentR*fSign < 0) {
				data.fCurrentR = 0.0f;
			} else {
				data.fCurrentR = data.fCurrentR + fSign*0.3f;
				if(data.fCurrentR > 0) {
					if(data.fCurrentR > data.fMaxRotate) {
						data.fCurrentR = data.fMaxRotate;
					}
				} else {
					if(data.fCurrentR < -data.fMaxRotate) {
						data.fCurrentR = -data.fMaxRotate;
					}
				}
				//Debug.Log("Turn" + fCurrentR);
			}

			go.transform.Rotate (0.0f, data.fCurrentR, 0.0f);
		}

		if (AIBehavior.OBABehavior (go, data, true)) {
			go.transform.forward = vFor;
		}

		float fMaxSpeed = data.fMaxSpeed;
		//float fMaxSpeed = (fDist/10.0f)*data.fMaxSpeed;
		//fMaxSpeed = Mathf.Min (fMaxSpeed, data.fMaxSpeed);
		//fMaxSpeed = Mathf.Max (fMaxSpeed, 0.1f);
		data.fSpeed += 0.1f;//fForwardForce;
		if (data.fSpeed > fMaxSpeed) {
			data.fSpeed = fMaxSpeed;
		} else if (data.fSpeed < 0.1f) {
			data.fSpeed = 0.1f;
		}

		return true;
		//Vector3 vNPos = cPos + go.transform.forward*data.fSpeed*Time.deltaTime;
		//go.transform.position = vNPos;

	}

	public static void MoveForward(GameObject go, AIData data)
	{
		Vector3 vNPos = go.transform.position + go.transform.forward*data.fSpeed*Time.deltaTime;
		go.transform.position = vNPos;
	}

	public static bool OBABehavior (GameObject go, AIData data, bool bTest)
	{

		Obstacle [] objs = data.m_Obs;
		int inObj = objs.Length;
		int i;
		Vector3 tpos;
		Vector3 cpos = go.transform.position;
		Vector3 vfor = go.transform.forward;
		Vector3 vec;
		Vector3 vecMin = Vector3.zero;
		float fDot = 0.0f;
		float fMinDot = 0.0f;
		float fDist = 0.0f;
		float fProjLen = 0.0f;
		float fRadian;
		float fDegree;
		float fTotalR = 0.0f;
		float fTotalRMin = 0.0f;
		float fMinDistance = 10000.0f;
		Obstacle m_Col = null;
		for(i = 0; i < inObj; i++) {
			tpos = objs[i].transform.position;
			vec = tpos - cpos;
			fDist = vec.magnitude;
			fTotalR = data.fRadius + objs[i].m_fRadius;
			// Check obstacle in range.
			if(fDist > data.fColProbe + fTotalR) {
				continue;
			}

			vec.Normalize();
			fDot = Vector3.Dot(vfor, vec);
			if(fDot < 0.01f) {
				continue;
			} else if(fDot > 1.0f) {
				fDot = 1.0f;
			}

			fRadian = Mathf.Acos(fDot);
			fProjLen = fDist*Mathf.Sin(fRadian);

			if(fProjLen > fTotalR) {
				continue;
			}
			if(bTest) {
				return true;
			}

			if(fDist < fMinDistance) {
				m_Col = objs[i];
				fMinDistance = fDist;
				fMinDot = fDot;
				vecMin = vec;
				fTotalRMin = fTotalR;
			}

		}

		if (m_Col != null) {
			float fForwardForce = fMinDot;
			float fTurnForce = fTotalRMin/(fProjLen + 0.01f);
			Debug.Log("!!!" + fTurnForce);
			float fSign = fTurnForce*0.1f;
			Vector3 vC = Vector3.Cross (vfor, vecMin);
			if (vC.y > 0) {
				fSign = -fTurnForce;
			}
			if(data.fCurrentR*fSign < 0) {
				data.fCurrentR = 0.0f;
			} else {
				data.fCurrentR = data.fCurrentR + fSign*2.0f;
				if(data.fCurrentR > 0) {
					if(data.fCurrentR > data.fMaxRotate) {
						data.fCurrentR = data.fMaxRotate;
					}
				} else {
					if(data.fCurrentR < -data.fMaxRotate) {
						data.fCurrentR = -data.fMaxRotate;
					}
				}
				//Debug.Log("Turn" + fCurrentR);
			}
			
			go.transform.Rotate (0.0f, data.fCurrentR, 0.0f);

			//float fMaxSpeed = data.fMaxSpeed;
			//float fMaxSpeed = (fDist/10.0f)*data.fMaxSpeed;
			//fMaxSpeed = Mathf.Min (fMaxSpeed, data.fMaxSpeed);
			//fMaxSpeed = Mathf.Max (fMaxSpeed, 0.1f);
			data.fSpeed -= fForwardForce;

			if (data.fSpeed < 0.0f) {
				data.fSpeed = 0.0f;
			}
			return true;
		}
		return false;
	}


}
