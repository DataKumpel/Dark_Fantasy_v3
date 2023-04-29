using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Collectable {
    public ItemType type = ItemType.none;
    public string item_name = string.Empty;
    public int max_stack_size = 10;
    public int quantity = 1;
    public Sprite item_img;

    public int GetDifference() {
        return max_stack_size - quantity;
    }
}


public enum ItemType {
    none,
    resource,
}
