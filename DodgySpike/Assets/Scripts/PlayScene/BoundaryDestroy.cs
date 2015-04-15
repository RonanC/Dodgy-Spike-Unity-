using UnityEngine;
using System.Collections;

// this class destroys any objects that try to leave it (spikes and walls)

public class BoundaryDestroy : MonoBehaviour {
	void OnTriggerExit2D(Collider2D other){
		Destroy(other.gameObject);
	}
}
