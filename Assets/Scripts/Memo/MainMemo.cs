using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class MainMemo : MonoBehaviour
{
    public GameObject imagePrefab;
    public GameObject gridPrefab;
    public GameObject pagePrompt;
    public GameObject pageTask;
    public GameObject currentGrid;
    public Transform gridContainer;
    public PromptMemo promptMemo;
    public PageManager PageManager;
    public DifficultySlider difficultySlider;
    public Dictionary<string, GameObject> trial = null;
    public int gridLength = 2;
    public float displayDuration = 1f;
    public bool isPractice;

    private int countMax;
    private int trialCount = 0;
    private readonly List<GameObject> gridCells = new();
    private readonly string iconFolderPath = @"D:\Files\Programming\Unity\LMT\Assets\Icons";

    void Start()
    {
        if (isPractice)
        {
            countMax = 2;
            gridLength = 3;
        }
        else
        {
            gridLength = (int)difficultySlider.slider.value + 2;
            countMax = (int)Mathf.Pow(gridLength, 2);
        }
        GridGenerate(gridLength);
        trial = TrialGenerate(gridLength);
    }

    void GridGenerate(int gridLength)
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

    Dictionary<string, GameObject> TrialGenerate(int gridLength)
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

    public void IconGenerate(GameObject container)
    {
        var iconTexture = new Texture2D(2, 2);
        iconTexture.LoadImage(File.ReadAllBytes(new List<string>(trial.Keys)[trialCount]));

        Sprite iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
        var iconObject = Instantiate(imagePrefab, container.transform, false);
        iconObject.GetComponent<Image>().sprite = iconSprite;

        //get grid reference for TaskMemo.cs
        var keys = new List<string>(trial.Keys);
        currentGrid = trial[keys[trialCount]];
    }

    public void OnButtonClick()
    {
        Destroy(promptMemo.imageContainer.transform.GetChild(0).gameObject);
        pagePrompt.SetActive(false); // Hide this page
        pageTask.SetActive(true);  // Show the Grid Page
    }

    public IEnumerator ReturnToOtherPage()
    {
        yield return new WaitForSeconds(displayDuration);
        Destroy(gridCells[trialCount].transform.GetChild(0).gameObject);

        pageTask.SetActive(false);  // Hide the Grid Page
        if (trialCount < countMax - 1)
        {
            trialCount++;
            pagePrompt.SetActive(true); // Show the Other Page
        }
        else
            PageManager.NavigateToNextPage();
    }
}