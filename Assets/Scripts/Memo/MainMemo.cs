using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using UnityEditor.Search;

public class MainMemo : MonoBehaviour
{
    public List<KeyValuePair<Sprite, GameObject>> trial;
    public DifficultySlider difficultySlider;
    public Transform gridContainer;
    // public PageManager memoPageManager;
    public PageManager pageManager;
    public PromptMemo promptMemo;
    public TaskMemo taskMemo;
    public GameObject currentGrid;
    public GameObject imagePrefab;
    public GameObject gridPrefab;
    public GameObject pagePrompt;
    public GameObject pageTask;
    public GameObject instructionPractice;
    public GameObject instructionTask;
    public int practiceCount = 2;
    public bool isPractice = true;
    public float displayDuration = 1f;

    private int gridLength;
    private int trialCountMax;
    private int runCountMax = 2;
    private int trialCount = 0;
    private int runCount = 0;
    private readonly string iconFolderPath = @"D:\Files\Programming\Unity\LMT\Assets\Icons";

    void Start()
    {
        if (isPractice)
        {
            gridLength = 3;
            instructionPractice.SetActive(true);
        }
        else
        {
            pagePrompt.SetActive(true);
            gridLength = (int)difficultySlider.slider.value + 2;
        }
        trialCountMax = (int)Mathf.Pow(gridLength, 2) - 1;
        List<GameObject> grid = GridGenerate(gridLength);
        trial = TrialGenerate(gridLength, grid);
    }

    List<GameObject> GridGenerate(int gridLength)
    {
        List<GameObject> gridCells = new();
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
        ShuffleList(gridCells);
        return gridCells;
    }

    List<KeyValuePair<Sprite, GameObject>> TrialGenerate(int gridLength, List<GameObject> gridCells)
    {
        int trialCount = gridLength * gridLength;
        var iconGridPairs = new List<KeyValuePair<Sprite, GameObject>>();
        var iconPaths = new List<string>(Directory.GetFiles(iconFolderPath, "*.png")).GetRange(0, trialCount);

        ShuffleList(iconPaths);
        for (int i = 0; i < trialCount; i++)
        {
            var iconTexture = new Texture2D(2, 2);
            iconTexture.LoadImage(File.ReadAllBytes(new List<string>(iconPaths)[i]));
            Sprite iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
            iconGridPairs.Add(new KeyValuePair<Sprite, GameObject>(iconSprite, gridCells[i]));
        }
        return iconGridPairs;
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
        var iconObject = Instantiate(imagePrefab, container.transform, false);
        iconObject.GetComponent<Image>().sprite = trial[trialCount].Key;

        //Get grid reference for TaskMemo.cs
        currentGrid = trial[trialCount].Value;
    }

    public void OnButtonClick()
    {
        Destroy(promptMemo.imageContainer.transform.GetChild(0).gameObject);
        pagePrompt.SetActive(false);
        pageTask.SetActive(true);
    }

    public IEnumerator ReturnToOtherPage()
    {
        yield return new WaitForSeconds(displayDuration);
        Destroy(taskMemo.imageContainer.transform.GetChild(0).gameObject);
        pageTask.SetActive(false);
        
        if (runCount < runCountMax)
        {
            if (trialCount < trialCountMax)
            {
                trialCount++;

                // End practice
                if (isPractice && trialCount == practiceCount)
                {
                    foreach (Transform child in gridContainer)
                        Destroy(child.gameObject);
                    
                    isPractice = false;
                    trialCount = 0;
                    gridLength = (int)difficultySlider.slider.value + 2;
                    trialCountMax = (int)Mathf.Pow(gridLength, 2) - 1;
                    
                    List<GameObject> grid = GridGenerate(gridLength);
                    trial = TrialGenerate(gridLength, grid);
                    instructionTask.SetActive(true);
                }
                else
                    pagePrompt.SetActive(true);
            }
            else
            {
                trialCount = 0;
                runCount++;
                pagePrompt.SetActive(true);
            }
        }
        else
        {
            pageManager.NavigateToNextPage();
        }
    }
}