// TODO generate trial at the very beginning. try define the functions first and only call them here

using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Collections;

public class GridTrialGenerator : MonoBehaviour
{
    public GameObject gridPrefab;
    public Transform baseContainer;
    public int levelSelection; // Determines grid size (2x2, 3x3, 4x4)
    public string iconFolderPath = @"D:\Files\Programming\Unity\LMT\Assets\Icons";

    public GameObject otherPage; // Reference to the Other Page
    public GameObject gridPage;  // Reference to this (Grid Page)
    public float displayDuration = 1f; // Time to display the Grid Page

    private List<GameObject> gridCells = new List<GameObject>();
    private int i = 0;
    
    void OnEnable()
    {
        if (i == 0)
        {
            GenerateGrid(levelSelection);
        }
        PlaceIconsOnGrid(GenerateTrialSet(levelSelection));
        StartCoroutine(ReturnToOtherPage());
        // Debug.Log("OnEnable");
    }

    void GenerateGrid(int level)
    {
        // foreach (Transform child in baseContainer)
        // {
        //     Destroy(child.gameObject);
        // }

        gridCells.Clear();

        int gridSize = level + 1;
        float gridCellSize = 100f;
        float spacing = 20f;
        float adjustedSpacing = (gridSize == 3) ? spacing * 1.1f : spacing;

        float totalSize = (gridSize * gridCellSize) + ((gridSize - 1) * adjustedSpacing);
        float startX = -totalSize / 2f + gridCellSize / 2f;
        float startY = totalSize / 2f - gridCellSize / 2f;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                float xPos = startX + col * (gridCellSize + adjustedSpacing);
                float yPos = startY - row * (gridCellSize + adjustedSpacing);

                GameObject gridCell = Instantiate(gridPrefab, baseContainer);
                gridCell.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
                gridCell.SetActive(true);
                gridCells.Add(gridCell);
            }
        }
    }

    Dictionary<string, GameObject> GenerateTrialSet(int level)
    {
        List<string> iconPaths = LoadIconPaths();
        int gridSize = level + 1;
        int trialCount = gridSize * gridSize;

        // Shuffle and select a subset of icons
        ShuffleList(iconPaths);//shuffle pairs
        List<string> selectedIcons = iconPaths.GetRange(0, trialCount);

        // Assign positions and store them in the dictionary
        var iconPositionPairs = new Dictionary<string, GameObject>();
        for (int i = 0; i < trialCount; i++)
            iconPositionPairs[selectedIcons[i]] = gridCells[i];
        return iconPositionPairs;
    }

    List<string> LoadIconPaths()
    {
        string[] files = Directory.GetFiles(iconFolderPath, "*.png");
        return new List<string>(files);
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    void PlaceIconsOnGrid(Dictionary<string, GameObject> iconPositionPairs)
    {

        var keys = new List<string>(iconPositionPairs.Keys);
        if (i < keys.Count)
        {
            string key = keys[i];
            PlaceIconOnGrid(iconPositionPairs[key], keys[i]);
            Debug.Log(keys[i]);
            i++;
            Debug.Log("PlaceIconsOnGrid");
        }
    }

    void PlaceIconOnGrid(GameObject gridCell, string iconPath)
    {
        Texture2D iconTexture = LoadTextureFromFile(iconPath);

        Sprite iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
        var iconObject = new GameObject("Icon");
        Image iconImage = iconObject.AddComponent<Image>();
        iconImage.sprite = iconSprite;

        iconObject.transform.SetParent(gridCell.transform, false);
    }

    Texture2D LoadTextureFromFile(string filePath)
    {
        var texture = new Texture2D(2, 2);
        texture.LoadImage(File.ReadAllBytes(filePath));
        return texture;
    }

    IEnumerator ReturnToOtherPage()
    {
        yield return new WaitForSeconds(displayDuration);
        gridPage.SetActive(false);  // Hide the Grid Page
        otherPage.SetActive(true); // Show the Other Page
    }
}