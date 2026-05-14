using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TrackManager : MonoBehaviour {

    [System.Serializable]
    public class SegmentData {

        public string name = "";
        // 0 - 100
        [Range(0, 100)]
        public float appearChance = 0f;
        public Segment prefab;
    }

    // singleton
    public static TrackManager Instance;

    public Vector3 lastPosition;

    public float speed = 1f;
    public float acceleration = 0.1f;

    [SerializeField]
    private SegmentData[] _segmentDatas; // prefab data list

    [SerializeField]
    private Transform _activeParent;
    [SerializeField]
    private Transform _poolParent;
    [SerializeField]
    private Transform _spawnAnchor;

    /// <summary>
    /// Coin variables
    /// </summary>
    [Header("Coin")]
    [SerializeField]
    private GameObject _coinPrefab;
    [SerializeField]
    private Transform _coinAnchor;
    [SerializeField]
    private Transform _coinParent;
    [SerializeField]
    private float _coinSpawnRange = 1.5f;
    private float _coinTimer = 0f;
    private float _currentCoinRate = 0f;
    [SerializeField]
    private Vector2 _coinSpawnRateRange = new Vector2();
    private int _coinAmount = 0;

    // pool
    private List<Segment> _pool = new List<Segment>();

    // score
    private int _numberOfSegmentPassed = 0;

    public delegate void OnNewSegment(int i);
    public OnNewSegment onNewSegment;

    private void Awake() {
        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NewSegment(_spawnAnchor.position);

        ResetCoinRate();
    }

    private void Update() {
        lastPosition += Vector3.forward * speed;
    }

    /// <summary>
    /// SPAWN BEHAVIOR
    /// </summary>
    /// <param name="pos"></param>
    public void NewSegment(Vector3 pos) {

        Segment newSegment = null;

        if (_pool.Count > 0) {

            // get first segment
            newSegment = _pool.First();
            // delete
            _pool.RemoveAt(0);

        } else {

            int prefabIndex = GetPrefabIndex();

            Segment segmentPrefab = _segmentDatas[prefabIndex].prefab;

            newSegment = Instantiate(segmentPrefab, pos, Quaternion.identity, _activeParent);
        }

        newSegment.GetComponent<Transform>().SetParent(_activeParent);
        newSegment.transform.position = pos; // ligne coupable
        newSegment.Show();

        ++_numberOfSegmentPassed;

        speed += acceleration;

        if (onNewSegment != null) {
            onNewSegment(_numberOfSegmentPassed);
        }

    }

    private int GetPrefabIndex() {

        float total = 0f;
        foreach (var item in _segmentDatas) {
            total += item.appearChance;
        }

        float roll = Random.Range(0, total);

        float cumul = 0f;

        int i = 0;
        foreach (var item in _segmentDatas) {
            cumul += item.appearChance;
            if (roll < cumul) {
                return i;
            }
            ++i;
        }

        return -1;
    }

    #region coin
    void CreateCoin() {
        GameObject coin_Obj = Instantiate(_coinPrefab, _coinParent);

        Vector3 pos = _coinAnchor.position + Vector3.right * Random.Range(-_coinSpawnRange, _coinSpawnRange);
        coin_Obj.transform.position = pos;

    }
    void ResetCoinRate() {

        CreateCoin();

        float rate = Random.Range(_coinSpawnRateRange.x, _coinSpawnRateRange.y);
        Invoke("ResetCoinRate", rate);
    }

    public void StopSpawn() {
        CancelInvoke("ResetCoinRate");
    }
    #endregion

    public void DestroySegments(Segment segment) {
        //_pendingSegments.Add(segment);
        Destroy(segment.gameObject);
        segment.Hide();
    }

    /// <summary>
    /// GIZMOS
    /// </summary>
    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(SpawnAnchor.position, 2f);
    }

    /// <summary>
    /// GETTERS / SETTERS
    /// </summary>
    public Transform SpawnAnchor {
        get => _spawnAnchor;
        set => _spawnAnchor = value;
    }
    public int CoinAmount {
        get => _coinAmount;
        set => _coinAmount = value;
    }
}
