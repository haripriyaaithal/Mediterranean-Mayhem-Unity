using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] Transform player;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start() {
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void LateUpdate() {
        if (player != null) {
            transform.position = player.position + offset;
        }
    }
}
