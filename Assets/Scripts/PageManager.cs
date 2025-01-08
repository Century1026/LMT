using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject[] pages;
    private int currentPageIndex = 0;

    public void NavigateToPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < pages.Length)
        {
            pages[currentPageIndex].SetActive(false);
            pages[pageIndex].SetActive(true);
            currentPageIndex = pageIndex;
        }
        else
        {
            currentPageIndex = 0;
        }
    }

    public void NavigateToNextPage()
    {
        NavigateToPage(currentPageIndex + 1);
    }
}