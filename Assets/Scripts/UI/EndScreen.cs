using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    private EndScreenHelper _endScreenHelper;
    private GameObject _scoreBoxes;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _endScreenHelper =  EndScreenHelper.Instance;
        _scoreBoxes = GameObject.Find("Scores");
        UpdateScoreTable();
    }

    private void UpdateScoreTable()
    {
        _endScreenHelper.PlayerScores = _endScreenHelper.PlayerScores
            .OrderByDescending(player => player.Score)
            .ToList();
        
        var topScores = _endScreenHelper.PlayerScores
            .Select(p => p.Score)
            .Distinct()
            .OrderByDescending(s => s)
            .Take(3)
            .ToList();
        
        var placeColors = new Dictionary<int, Color>
        {
            { 1, new Color(212f / 255f, 175f / 255f, 55f / 255f) },
            { 2, new Color(192f / 255f, 192f / 255f, 192f / 255f) },
            { 3, new Color(205f / 255f, 127f / 255f, 50f / 255f) }
        };

        for (int i = 0; i < _endScreenHelper.PlayerScores.Count; i++)
        {
            var score = _endScreenHelper.PlayerScores[i].Score;

            int place = topScores.IndexOf(score) + 1;

            if (place >= 1 && place <= 3)
            {
                UpdateTableRow(i, place, placeColors[place]);
            }
            else
            {
                UpdateTableRow(i, 4, Color.white);
            }
        }
    }

    private void UpdateTableRow(int index, int place, Color rowColor)
    {
        Transform scoreBox = _scoreBoxes.transform.GetChild(index).transform;
        scoreBox.GetChild(0).GetComponent<TextMeshProUGUI>().text = place.ToString();
        scoreBox.GetChild(0).GetComponent<TextMeshProUGUI>().color = rowColor;
        
        scoreBox.GetChild(1).GetComponent<TextMeshProUGUI>().text = _endScreenHelper.PlayerScores[index].PlayerName;
        scoreBox.GetChild(1).GetComponent<TextMeshProUGUI>().color = rowColor;
        
        scoreBox.GetChild(2).GetComponent<TextMeshProUGUI>().text = _endScreenHelper.PlayerScores[index].Score.ToString();
        scoreBox.GetChild(2).GetComponent<TextMeshProUGUI>().color = rowColor;
    }
}
