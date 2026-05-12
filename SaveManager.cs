using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField]
    private SaveData _saveData;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        // path to save folder
        string savePath = Application.persistentDataPath;
        Debug.Log($"{savePath}");
    }

    public void Save() {

        _saveData.score = TrackManager.Instance.CoinAmount;

        _saveData.lastPosition = TrackManager.Instance.lastPosition;
    }

    public void Load() {

    }

}

/// <summary>
/// JSON
/// </summary>
[System.Serializable]
public class SaveData {
    public string name = "defaultName";

    public Vector3 lastPosition;

    public int score = 0;
    // obs
    public int coin = 0;

    public ItemData[] itemData;
}

[System.Serializable]
public class ItemData {
    public string name;
    public int price;
    public int damage;
}
