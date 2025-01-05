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
    public GameObject pageTask;
    public int gridLength; // Determines grid size (2x2, 3x3, 4x4)
    public float displayDuration = 1f; // Time to display the Grid Page
    public string iconFolderPath = @"D:\Files\Programming\Unity\LMT\Assets\Icons";

    private readonly List<GameObject> gridCells = new();
    private Dictionary<string, GameObject> trial = null;
    private int trialIndex = 0;
    
    void OnEnable()
    {
        if (trialIndex == 0)
        {
            GenerateGrid(gridLength);
            trial = GenerateTrialSet(gridLength);
        }
        IconsDisplay(trial);
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
        var iconPaths = new List<string>(Directory.GetFiles(iconFolderPath, "*.png"));
        int trialCount = gridLength * gridLength;
        
        List<string> selectedIcons = iconPaths.GetRange(0, trialCount);
        ShuffleList(gridCells);//shuffle pairs

        // Assign positions and store them in the dictionary
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

    void IconsDisplay(Dictionary<string, GameObject> iconPositionPairs)
    {
        var keys = new List<string>(iconPositionPairs.Keys);
        if (trialIndex < keys.Count)
        {
            IconGenerate(iconPositionPairs[keys[trialIndex]], keys[trialIndex]);
            Debug.Log(keys[trialIndex]);
            trialIndex++;
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
        Destroy(gridCells[trialIndex-1].transform.GetChild(0).gameObject);
        pageTask.SetActive(false);  // Hide the Grid Page
        pagePrompt.SetActive(true); // Show the Other Page
    }
}