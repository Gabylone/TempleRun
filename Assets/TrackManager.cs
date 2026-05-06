using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public static TrackManager Instance;

    [SerializeField]
    private Segment[] _segmentPrefabs;

    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private Transform _spawnAnchor;

    private List<Segment> _pool = new List<Segment>();


    public Transform SpawnAnchor {
        get => _spawnAnchor;
        set => _spawnAnchor = value;
    }

    private void Awake() {
        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NewSegment(_spawnAnchor.position);
    }

    public void NewSegment(Vector3 pos) {

        Segment newSegment = null;

        if (_pool.Count > 0) {

            newSegment = _pool.First();
            _pool.RemoveAt(0);

        } else {

            int prefabIndex = Random.Range(0, _segmentPrefabs.Length);
            Segment segmentPrefab = _segmentPrefabs[prefabIndex];
            newSegment = Instantiate(segmentPrefab, pos, Quaternion.identity, _parent);
            newSegment.GetComponent<Transform>().SetParent(_parent);
        }

        newSegment.Show();

    }

    public void DestroySegments(Segment segment) {
        _pool.Add(segment);
        segment.Hide();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(SpawnAnchor.position, 2f);
    }
}
