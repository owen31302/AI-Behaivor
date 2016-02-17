using UnityEngine;
using System.Collections;

public class AIdata2{
	//seek
	public Vector3 tarTrams;
	public float maxSpeed = 5.0f;
	public float current_velocity = 0.0f;
	//public GameObject target;

	//obstacleAvoidance
	public Obstacle [] m_Obs;
	public float forwardCollisionProbeLength;
	public float forwardCollisionProbeLengthFactor = 30.0f;  //Becarefull! if this factor is too small, pos_d_normal.magnitude will close to zero and make turning very fast.
	public float boundingSphereRadius = 1.0f;
	public float fieldOfView = 80.0f;
	public float turnfactor = 1.0f;

}

public class AIBehaivor2 {
	
	public static bool seek(GameObject go, AIdata2 aiAdata){
		// calculate the steering velocity
		Vector3 pos_d = aiAdata.tarTrams - go.transform.position;
		float d = pos_d.magnitude;
		pos_d.Normalize ();
		Vector3 desired_velocity = pos_d * aiAdata.maxSpeed;
		Vector3 steering_velocity = desired_velocity - go.transform.forward * aiAdata.current_velocity;

		// reach goal and return
		if (d < Time.deltaTime * aiAdata.maxSpeed * 5.0f) {
			go.transform.position = aiAdata.tarTrams;
			return true;
		}

		// decompose the steering_velocity
		Vector3 steering_velocity_parallel = Vector3.Project (steering_velocity, go.transform.forward);
		Vector3 steering_velocity_normal = steering_velocity - steering_velocity_parallel;

		//seek angle
		Vector3 turn_forward_vector = go.transform.forward + steering_velocity_normal * Time.deltaTime;
		turn_forward_vector.Normalize ();
		go.transform.forward = turn_forward_vector;

		//seek velocity
		aiAdata.current_velocity = aiAdata.current_velocity + steering_velocity_parallel.magnitude * Time.deltaTime;
			// prevent exceed the max speed
		if (aiAdata.current_velocity > aiAdata.maxSpeed) {
			aiAdata.current_velocity = aiAdata.maxSpeed;
		}
		go.transform.position = go.transform.position + go.transform.forward * aiAdata.current_velocity* Time.deltaTime;
        return false;
    }

	public static bool obstacleAvoidance(GameObject go, AIdata2 aiAdata){
		Obstacle [] Obstacles = aiAdata.m_Obs;
		Vector3 pos_d;
		float distance;
		float pos_d_dotForward;
		Vector3 pos_d_parallel;
		Vector3 pos_d_normal;
		float theta;
		float fMinDistance = Mathf.Infinity;
		Obstacle nearest_Obstacle = null;
		for(int i = 0; i< Obstacles.Length; i++){
			// if obstacles are in the circle (radius = forwardCollisionProbeLength)
			pos_d = Obstacles[i].transform.position - go.transform.position;
			distance = pos_d.magnitude;
			aiAdata.forwardCollisionProbeLength = aiAdata.current_velocity * Time.deltaTime * aiAdata.forwardCollisionProbeLengthFactor;
			if ( distance > aiAdata.forwardCollisionProbeLength + Obstacles[i].m_fRadius ){
				continue;
			}

			// if obstacles are in of FOV
			pos_d_dotForward = Vector3.Dot(pos_d.normalized, go.transform.forward);
			theta = Mathf.Acos(pos_d_dotForward) * Mathf.Rad2Deg;
			if(theta > aiAdata.fieldOfView){
				continue;
			}

			// if obstacles are in the forward path cylinder
				// decompose the pos_d
			pos_d_parallel = Vector3.Project (pos_d, go.transform.forward);
			pos_d_normal = pos_d - pos_d_parallel;
			if(pos_d_normal.magnitude > aiAdata.boundingSphereRadius + Obstacles[i].m_fRadius){
				continue;
			}

			// find the Nearest intersected obstacle 
			if( distance < fMinDistance){
				fMinDistance = distance;
				nearest_Obstacle = Obstacles[i];
			}
		}

		if (nearest_Obstacle == null) {
			return false;
		} else {
			// steering force
				// angle
					// pos_d
			pos_d = nearest_Obstacle.transform.position - go.transform.position;
					// decompose the pos_d
			pos_d_parallel = Vector3.Project (pos_d, go.transform.forward);
			pos_d_normal = pos_d - pos_d_parallel;
					// steering force
			float steering_velocity_magnitude = aiAdata.turnfactor / (pos_d_normal.magnitude); 
			Vector3 turn_forward_vector = go.transform.forward - pos_d_normal.normalized * steering_velocity_magnitude * Time.deltaTime;
			turn_forward_vector.Normalize ();
			go.transform.forward = turn_forward_vector;

				// velocity
			go.transform.position = go.transform.position + go.transform.forward * aiAdata.current_velocity* Time.deltaTime;
			return true;
		}


	}
}
