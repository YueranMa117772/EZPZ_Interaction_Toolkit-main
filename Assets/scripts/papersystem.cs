using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class papersystem : MonoBehaviour
{
    public Transform frontRoot;
    public Transform backRoot;

    private List<GameObject> frontPapers = new List<GameObject>();
    private List<GameObject> backPapers = new List<GameObject>();

    private int currentFront = 1;
    private int currentBack = 3;

    void Start()
    {
        CollectAndSortPapers();
        Refresh();
    }

    void CollectAndSortPapers()
    {
        frontPapers.Clear();
        backPapers.Clear();

        foreach (Transform t in frontRoot)
            frontPapers.Add(t.gameObject);

        foreach (Transform t in backRoot)
            backPapers.Add(t.gameObject);

        frontPapers.Sort((a, b) => ExtractNumber(a.name).CompareTo(ExtractNumber(b.name)));
        backPapers.Sort((a, b) => ExtractNumber(a.name).CompareTo(ExtractNumber(b.name)));
    }

    int ExtractNumber(string name)
    {
        Match m = Regex.Match(name, @"\d+");
        if (m.Success)
            return int.Parse(m.Value);

        return 0;
    }

    public void FeedLine()
    {
        if (currentBack > 0)
        {
            currentBack--;
            currentFront++;
            Refresh();
        }
    }

    void Refresh()
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