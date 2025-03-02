
using System;
using System.IO;
using UnityEngine;

public class ItemDataSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        ItemDataSO itemDataSo = GetSO<ItemDataSO>(GetAssetPath(Path.Combine(fullPath, data[0])));

        itemDataSo.id = int.Parse(data[0]);
        itemDataSo.itemName = data[1];
        itemDataSo.requireLevel = int.Parse(data[2]);
        itemDataSo.hp = int.Parse(data[3]);
        itemDataSo.mp = int.Parse(data[4]);
        itemDataSo.description = data[5];
        itemDataSo.imagePath = data[6];
        return itemDataSo;
    }
}
