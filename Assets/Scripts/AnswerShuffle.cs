using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerShuffle : MonoBehaviour
{
    public RectTransform answer1;
    public RectTransform answer2;
    public RectTransform answer3;

    void Start()
    {
        ShufflePositions();
    }

    void ShufflePositions()
    {
        List<Vector2> positions = new List<Vector2>
        {
            new Vector2(-740f, -63f),
            new Vector2(-740f, -222.5184f),
            new Vector2(-740f, -382.3074f)
        };

        ShuffleList(positions);

        answer1.anchoredPosition = positions[0];
        answer2.anchoredPosition = positions[1];
        answer3.anchoredPosition = positions[2];
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
