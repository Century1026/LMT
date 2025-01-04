using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TaskMemo : MonoBehaviour
{
    public string iconFolderPath = @"D:\Files\Programming\Unity\LMT\Assets\Icons";
    public GameObject gridPrefab; // Assign a prefab for the grid
    public Transform gridParent; // Assign a parent object to organize the grids
    public int gridCount = 4; // Number of grids (2x2 layout)

    private Dictionary<string, Vector2> trialData = new Dictionary<string, Vector2>();
    private List<Vector2> gridPositions = new List<Vector2>();
    private List<string> iconPaths = new List<string>();

    void Start()
    {
        GenerateGridPositions();
        LoadIcons();
        GenerateTrials();
        DisplayTrials();
    }

    void GenerateGridPositions()
    {
        float gridSize = 1.0f; // Adjust grid size
        float offset = gridSize / 2; // Offset to center grids

        int rows = Mathf.CeilToInt(Mathf.Sqrt(gridCount));
        int cols = Mathf.CeilToInt(Mathf.Sqrt(gridCount));

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (gridPositions.Count >= gridCount) break;
                float x = col * gridSize - (cols * gridSize) / 2 + offset;
                float y = row * gridSize - (rows * gridSize) / 2 + offset;
                gridPositions.Add(new Vector2(x, y));
            }
        }
    }

    void LoadIcons()
    {
        if (Directory.Exists(iconFolderPath))
        {
            string[] files = Directory.GetFiles(iconFolderPath, "*.png"); // Load PNG icons
            foreach (string file in files)
            {
                iconPaths.Add(file);
            }
        }
        else
        {
            Debug.LogError("Icon folder path does not exist!");
        }
    }

    void GenerateTrials()
    {
        if (iconPaths.Count < gridCount)
        {
            Debug.LogError("Not enough icons for the number of grids!");
            return;
        }

        // Shuffle grid positions and icons
        ShuffleList(gridPositions);
        ShuffleList(iconPaths);

        for (int i = 0; i < gridCount; i++)
        {
            string iconPath = iconPaths[i];
            Vector2 position = gridPositions[i];
            trialData.Add(iconPath, position);
        }
    }

    void DisplayTrials()
    {
        foreach (var trial in trialData)
        {
            // Instantiate a grid object and set its position
            GameObject grid = Instantiate(gridPrefab, gridParent);
            grid.transform.localPosition = trial.Value;

            // Load icon texture
            Texture2D iconTexture = LoadTexture(trial.Key);
            if (iconTexture != null)
            {
                grid.GetComponent<Renderer>().material.mainTexture = iconTexture;
            }
        }
    }

    Texture2D LoadTexture(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            return texture;
        }
        Debug.LogError("Failed to load texture from " + filePath);
        return null;
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}