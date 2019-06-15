using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemGenre
{
    Gate,
    Turret,
    Wall,
    Trap,
    Forward,
    Back
}
[System.Serializable]
public struct ItemData
{
    public ItemGenre Genre;
    public Sprite Icon;
    public int ItemCode;
    public int Power;
    public int Shield;
}

public class InventroyData : MonoBehaviour
{
    [Header("Stored Items")]
    public List<ItemData> m_Items;

    public static InventroyData _instance;

    public static InventroyData Instance {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<InventroyData>();
                //Debug.Log(_instance.name);
            }

            return _instance;
        }
    }

}
