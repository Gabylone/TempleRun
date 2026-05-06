using UnityEngine;

public class Segment : MonoBehaviour
{
    public float speed = 1f;

    public float maxZ = -10f;

    public float lenght = 10f;

    private bool _instantiated = false;
    private bool _destroyed = false;

    public Transform _spawnAnchor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Show() {
        _instantiated = false;
        _destroyed = false;
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.forward * speed * Time.deltaTime, Space.World);

        if (!_instantiated && transform.position.z < TrackManager.Instance.SpawnAnchor.position.z - lenght) {
            TrackManager.Instance.NewSegment(_spawnAnchor.position);
            _instantiated = true;
        }

        if ( !_destroyed && transform.position.z < maxZ) {
            TrackManager.Instance.DestroySegments(this);
            _destroyed = true;
        }
    }
}
