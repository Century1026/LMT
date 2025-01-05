using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Collections;

public class GridTrialGenerator : MonoBehaviour
{
    public Transform baseContainer;
    public GameObject gridPrefab;
    public GameObject pagePrompt;
    public GameObject pageTask;  // Reference to this (Grid Page)
    public int gridLength; // Determines grid size (2x2, 3x3, 4x4)
    public float displayDuration = 1f; // Time to display the Grid Page
    public string iconFolderPath = @"D:\Files\Programming\Unity\LMT\Assets\Icons";

    private readonly List<GameObject> gridCells = new();
    private int trialCount = 0;
    
    void OnEnable()
    {
        if (trialCount == 0)
            GenerateGrid(gridLength);
        IconsDisplay(GenerateTrialSet(gridLength));
        StartCoroutine(ReturnToOtherPage());
    }

    void GenerateGrid(int gridLength)
    {
        float gridCellSize = 100f;
        float spacing = 20f;

        float totalSize = (gridLength * gridCellSize) + ((gridLength - 1) * spacing);
        float startX = -totalSize / 2f + gridCellSize / 2f;
        float startY = totalSize / 2f - gridCellSize / 2f;

        for (int row = 0; row < gridLength; row++)
        {
            for (int col = 0; col < gridLength; col++)
            {
                float xPos = startX + col * (gridCellSize + spacing);
                float yPos = startY - row * (gridCellSize + spacing);

                GameObject gridCell = Instantiate(gridPrefab, baseContainer);
                gridCell.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
                gridCell.SetActive(true);
                gridCells.Add(gridCell);
            }
        }
    }

    Dictionary<string, GameObject> GenerateTrialSet(int gridLength)
    {
        List<string> iconPaths = LoadIconPaths();
        int trialCount = gridLength * gridLength;

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

    void IconsDisplay(Dictionary<string, GameObject> iconPositionPairs)
    {
        var keys = new List<string>(iconPositionPairs.Keys);
        if (trialCount < keys.Count)
        {
            IconGenerate(iconPositionPairs[keys[trialCount]], keys[trialCount]);
            Debug.Log(keys[trialCount]);
            trialCount++;
        }
    }

    GameObject IconGenerate(GameObject gridCell, string iconPath)
    {
        Texture2D iconTexture = LoadTextureFromFile(iconPath);

        Sprite iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
        var iconObject = new GameObject("Icon");
        Image iconImage = iconObject.AddComponent<Image>();
        iconImage.sprite = iconSprite;

        iconObject.transform.SetParent(gridCell.transform, false);

        return iconObject;
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
        Destroy(gridCells[trialCount-1].transform.GetChild(0).gameObject);
        pageTask.SetActive(false);  // Hide the Grid Page
        pagePrompt.SetActive(true); // Show the Other Page
    }
}