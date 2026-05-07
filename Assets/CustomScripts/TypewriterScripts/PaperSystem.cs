using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// Controls front and back papers.
public class PaperSystem : MonoBehaviour
{
    public Transform FrontRoot;
    public Transform BackRoot;

    private List<GameObject> frontPapers = new List<GameObject>();
    private List<GameObject> backPapers = new List<GameObject>();

    private int currentFront = 1;
    private int currentBack = 3;

    private void Start()
    {
        CollectAndSortPapers();
        RefreshPapers();
    }

    private void CollectAndSortPapers()
    {
        frontPapers.Clear();
        backPapers.Clear();

        foreach (Transform paper in FrontRoot)
        {
            frontPapers.Add(paper.gameObject);
        }

        foreach (Transform paper in BackRoot)
        {
            backPapers.Add(paper.gameObject);
        }

        frontPapers.Sort((a, b) => ExtractNumber(a.name).CompareTo(ExtractNumber(b.name)));
        backPapers.Sort((a, b) => ExtractNumber(a.name).CompareTo(ExtractNumber(b.name)));
    }

    private int ExtractNumber(string objectName)
    {
        Match match = Regex.Match(objectName, @"\d+");

        if (match.Success)
        {
            return int.Parse(match.Value);
        }

        return 0;
    }

    public void FeedLine()
    {
        if (currentBack > 0)
        {
            currentBack--;
            currentFront++;

            RefreshPapers();
        }
    }

    private void RefreshPapers()
    {
        for (int i = 0; i < frontPapers.Count; i++)
        {
            frontPapers[i].SetActive(i < currentFront);
        }

        for (int i = 0; i < backPapers.Count; i++)
        {
            backPapers[i].SetActive(i < currentBack);
        }
    }
}