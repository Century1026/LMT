using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainMemo : MonoBehaviour
{
    public Transform gridContainer;
    public GameObject gridPrefab;
    public int gridLength; // Determines grid size (2x2, 3x3, 4x4)
    public string iconFolderPath = @"D:\Files\Programming\Unity\LMT\Assets\Icons";

    public readonly List<GameObject> gridCells = new();
    public Dictionary<string, GameObject> trial = null;
    
    void Start()
    {
        GenerateGrid(gridLength);
        trial = GenerateTrialSet(gridLength);
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

                GameObject gridCell = Instantiate(gridPrefab, gridContainer);
                gridCell.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
                gridCells.Add(gridCell);
            }
        }
    }

    Dictionary<string, GameObject> GenerateTrialSet(int gridLength)
    {
        var iconPaths = new List<string>(Directory.GetFiles(iconFolderPath, "*.png"));
        int trialCount = gridLength * gridLength;
        
        List<string> selectedIcons = iconPaths.GetRange(0, trialCount);
        ShuffleList(gridCells);//shuffle pairs

        var iconPositionPairs = new Dictionary<string, GameObject>();
        for (int i = 0; i < trialCount; i++)
            iconPositionPairs[selectedIcons[i]] = gridCells[i];
        return iconPositionPairs;
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}