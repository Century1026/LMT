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
        GenerateGrid(levelSelection);
        PlaceIconsOnGrid(GenerateTrialSet(levelSelection));
        StartCoroutine(ReturnToOtherPage());
        // Debug.Log("OnEnable");
    }

    // void Start()
    // {
    //     GenerateGrid(levelSelection);
    //     PlaceIconsOnGrid(GenerateTrialSet(levelSelection));
    //     // OnEnable();
    //     StartCoroutine(ReturnToOtherPage());
    //     // Debug.Log("Start");
    // }

    void GenerateGrid(int level)
    {
        foreach (Transform child in baseContainer)
        {
            Destroy(child.gameObject);
        }

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
                // Image gridImage = gridCell.GetComponent<Image>();
                // if (gridImage != null)
                // {
                //     gridImage.enabled = true;
                // }

                // Button gridButton = gridCell.GetComponent<Button>();
                // if (gridButton != null)
                // {
                //     gridButton.enabled = true;
                // }

                gridCells.Add(gridCell);
            }
        }
    }

    Dictionary<string, GameObject> GenerateTrialSet(int level)
    {
        List<string> iconPaths = LoadIconPaths();
        if (iconPaths == null || iconPaths.Count == 0)
        {
            Debug.LogWarning("No icons found in the specified folder.");
            return null;
        }

        int gridSize = level + 1;
        int trialCount = gridSize * gridSize;

        if (iconPaths.Count < trialCount)
        {
            Debug.LogError("Not enough icons to fill the grid.");
            return null;
        }

        // Shuffle and select a subset of icons
        ShuffleList(iconPaths);//shuffle pairs
        List<string> selectedIcons = iconPaths.GetRange(0, trialCount);

        // Assign positions and store them in the dictionary
        // iconPositionPairs.Clear();
        Dictionary<string, GameObject> iconPositionPairs = new Dictionary<string, GameObject>();
        for (int i = 0; i < trialCount; i++)
        {
            // Vector2 gridPosition = gridCells[i].GetComponent<RectTransform>().anchoredPosition;
            // iconPositionPairs[selectedIcons[i]] = gridPosition;
            iconPositionPairs[selectedIcons[i]] = gridCells[i];

            // PlaceIconOnGrid(gridCells[i], selectedIcons[i]);//read pair
        }
//read only one pair
        // Debug.Log("Trials generated successfully:");
        // foreach (var pair in iconPositionPairs)
        // {
        //     Debug.Log($"Icon: {Path.GetFileName(pair.Key)}, Position: {pair.Value}");
        // }
        return iconPositionPairs;
    }

    List<string> LoadIconPaths()
    {
        if (!Directory.Exists(iconFolderPath))
        {
            Debug.LogError("Icon folder path does not exist: " + iconFolderPath);
            return null;
        }

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
        // foreach (var pair in iconPositionPairs)
        // {
        //     Debug.Log(pair.Key);
        //     PlaceIconOnGrid(pair.Value, pair.Key);
        //     return;
        // }
        // int i = 0;
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
    // List<IconPositionPair> iconPositionPairs
    {
        Texture2D iconTexture = LoadTextureFromFile(iconPath);
        if (iconTexture == null)
        {
            Debug.LogError("Failed to load texture: " + iconPath);
            return;
        }

        Sprite iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
        GameObject iconObject = new GameObject("Icon");
        iconObject.transform.SetParent(gridCell.transform, false);

        Image iconImage = iconObject.AddComponent<Image>();
        iconImage.sprite = iconSprite;
        iconImage.rectTransform.sizeDelta = new Vector2(80f, 80f);
        iconImage.rectTransform.anchoredPosition = Vector2.zero;
    }

    Texture2D LoadTextureFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            return texture;
        }
        return null;
    }



    IEnumerator ReturnToOtherPage()
    {
        yield return new WaitForSeconds(displayDuration);
        gridPage.SetActive(false);  // Hide the Grid Page
        otherPage.SetActive(true); // Show the Other Page
        // Debug.Log("ReturnToOtherPage");
        // Start();
    }
}