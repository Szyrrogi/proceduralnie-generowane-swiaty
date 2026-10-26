using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct BlockPrefab
{
    public string blockName;
    public GameObject prefab;
}

public class BlockManager : MonoBehaviour
{
    public float skala = 1f;
    public List<BlockPrefab> dostepneBloki;
    private Dictionary<string, GameObject> blockMap = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (var block in dostepneBloki)
        {
            if (!blockMap.ContainsKey(block.blockName))
            {
                blockMap.Add(block.blockName, block.prefab);
            }
            else
            {
                Debug.LogWarning($"Duplikat {block.blockName}");
            }
        }
    }

    public GameObject PostawBlok(string blockType, Vector3Int pozycja, Transform rodzic)
    {
        if (!blockMap.TryGetValue(blockType, out GameObject prefabDoPostawienia))
        {
            Debug.LogError($"Niepoprawna nazwa bloku: {blockType}");
            return null;
        }

        Vector3 pozycjaSwiata = (Vector3)pozycja * skala;
        GameObject nowy = Instantiate(prefabDoPostawienia, rodzic);
        nowy.transform.localPosition = pozycjaSwiata;
        nowy.transform.localScale = Vector3.one * skala;
        return nowy;
    }
}