using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorController : MonoBehaviour
{

    [Header("Generator Area")]
    [SerializeField]
    private float areaStartOffset;
    [SerializeField]
    private float areaEndOffset;
    [SerializeField]
    private float posYFirstSpawn;

    [Header("Templates")]
    [SerializeField]
    private List<TerrainTemplateController> terrainTemplates;
    [SerializeField]
    private float terrainTemplateWidth;

    [Header("Force Early Template")]
    [SerializeField]
    private List<TerrainTemplateController> earlyTerrainTemplates;

    private List<GameObject> spawnedTerrain;
    private float lastGeneratedPositionX;
    private float lastRemovedPositionX;

    private const float debugLineHeight = 10.0f;

    private Dictionary<string, List<GameObject>> pool;

    private void Start()
    {
        pool = new Dictionary<string, List<GameObject>>();

        spawnedTerrain = new List<GameObject>();

        lastGeneratedPositionX = GetHorizontalPositionStart();
        lastRemovedPositionX = lastGeneratedPositionX - terrainTemplateWidth;

        foreach (TerrainTemplateController terrain in earlyTerrainTemplates)
        {
            GenerateTerrain(lastGeneratedPositionX, terrain);
            lastGeneratedPositionX += terrainTemplateWidth;
        }

        while (lastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidth;
        }
    }

    private float GetHorizontalPositionStart() => Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f)).x + areaStartOffset;
    private float GetHorizontalPositionEnd() => Camera.main.ViewportToWorldPoint(new Vector2(1f, 0f)).x + areaEndOffset;

    private void GenerateTerrain(float posX, TerrainTemplateController forceTerrain = null)
    {
        GameObject newTerrain;

        if (forceTerrain == null)
            newTerrain = GenerateFromPool(terrainTemplates[Random.Range(0, terrainTemplates.Count)].gameObject, transform);
        else
            newTerrain = GenerateFromPool(forceTerrain.gameObject, transform);

        newTerrain.transform.position = new Vector2(posX, posYFirstSpawn);
        spawnedTerrain.Add(newTerrain);
    }

    private void Update()
    {
        if(GameManager.Instance.GameState == GameState.Start)
        {
            while (lastGeneratedPositionX < GetHorizontalPositionEnd())
            {
                GenerateTerrain(lastGeneratedPositionX);
                lastGeneratedPositionX += terrainTemplateWidth;
            }

            while (lastRemovedPositionX + terrainTemplateWidth < GetHorizontalPositionStart())
            {
                lastRemovedPositionX += terrainTemplateWidth;
                RemoveTerrain(lastRemovedPositionX);
            }
        }
    }

    private void RemoveTerrain(float posX)
    {
        GameObject terrainToRemove = null;

        foreach (GameObject item in spawnedTerrain)
        {
            if (item.transform.position.x == posX)
            {
                terrainToRemove = item;
                break;
            }
        }

        if (terrainToRemove != null)
        {
            spawnedTerrain.Remove(terrainToRemove);
            ReturnToPool(terrainToRemove);
        }
    }

    private GameObject GenerateFromPool(GameObject item, Transform parent)
    {
        if (pool.ContainsKey(item.name))
        {
            if (pool[item.name].Count > 0)
            {
                GameObject newItemFromPool = pool[item.name][0];
                pool[item.name].Remove(newItemFromPool);
                newItemFromPool.SetActive(true);
                return newItemFromPool;
            }
        }
        else
            pool.Add(item.name, new List<GameObject>());

        GameObject newItem = Instantiate(item, parent);
        newItem.name = item.name;
        return newItem;
    }

    //Debug Pool
    private void ReturnToPool(GameObject item)
    {
        if (!pool.ContainsKey(item.name)) Debug.LogError("ITEM POOL IS INVALID");
        pool[item.name].Add(item);
        item.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Vector3 areaStartPosition = transform.position;
        Vector3 areaEndPosition = transform.position;

        areaStartPosition.x = GetHorizontalPositionStart();
        areaEndPosition.x = GetHorizontalPositionEnd();

        Debug.DrawLine(areaStartPosition + Vector3.up * debugLineHeight / 2, areaStartPosition + Vector3.down * debugLineHeight / 2, Color.red);
        Debug.DrawLine(areaEndPosition + Vector3.up * debugLineHeight / 2, areaEndPosition + Vector3.down * debugLineHeight / 2, Color.red);
    }
}
