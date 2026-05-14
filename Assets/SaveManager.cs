using System.IO;
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
        Load();
    }

    public void Save() {

        // rempli la classe de save
        _saveData.coin = TrackManager.Instance.CoinAmount;
        _saveData.lastPosition = TrackManager.Instance.lastPosition;

        ItemData[] itemDatas = new ItemData[2] {
            new ItemData("potion de feu", 520, 5),
            new ItemData("parchemin de tonerre", 10, 300)
        };
        _saveData.itemData = itemDatas;

        /*--------------------------------------------------*/

        // creer notre json
        string json = JsonUtility.ToJson(_saveData, true);
        // crere notre path
        string path = $"{Application.persistentDataPath}/save.json";
        // on l'inscrit dans l'ordi
        File.WriteAllText(path, json);

        Debug.Log($"Chemin : {path}");
    }

    public void Load() {

        Debug.Log($"Loading Game");


        // load
        string path = $"{Application.persistentDataPath}/save.json";
        string json = File.ReadAllText(path);

        _saveData = JsonUtility.FromJson<SaveData>(json);

        /// LOADING
        TrackManager.Instance.CoinAmount = _saveData.coin;
    }

}

/// <summary>
/// JSON
/// </summary>
[System.Serializable]
public class SaveData {
    public string name = "defaultName";

    public Vector3 lastPosition;

    public int coin = 0;

    public ItemData[] itemData;
}

[System.Serializable]
public class ItemData {

    public string _name;
    public int _price;
    public int _damage;

    public ItemData(string name, int startPrice, int startDamage) {
        _name = name;
        _price = startPrice;
        _damage = startDamage;
    }

}
