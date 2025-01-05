using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GridTrialGenerator : MonoBehaviour
{
    public GameObject imagePrefab;
    public GameObject pagePrompt;
    public GameObject pageTask;
    public MainMemo mainMemo;
    public float displayDuration = 1f;
    private int trialIndex = 0;
    
    void OnEnable()
    {
        IconsDisplay(mainMemo.trial);
        StartCoroutine(ReturnToOtherPage());
    }

    void IconsDisplay(Dictionary<string, GameObject> iconPositionPairs)
    {
        var keys = new List<string>(iconPositionPairs.Keys);
        if (trialIndex < keys.Count)
        {
            IconGenerate(imagePrefab, iconPositionPairs[keys[trialIndex]], keys[trialIndex]);
            trialIndex++;
        }
    }

    void IconGenerate(GameObject prefab, GameObject gridCell, string iconPath)
    {
        Texture2D iconTexture = LoadTextureFromFile(iconPath);
        Sprite iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
        var iconObject = Instantiate(prefab, gridCell.transform, false);
        iconObject.GetComponent<Image>().sprite = iconSprite;
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
        Destroy(mainMemo.gridCells[trialIndex-1].transform.GetChild(0).gameObject);
        pageTask.SetActive(false);  // Hide the Grid Page
        pagePrompt.SetActive(true); // Show the Other Page
    }
}