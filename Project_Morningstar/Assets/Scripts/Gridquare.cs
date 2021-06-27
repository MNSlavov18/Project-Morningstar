using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridquare : MonoBehaviour
{
    public int squreIndex { get; set; }

    private AlphabetData.LetterData _normalLetterData;
    private AlphabetData.LetterData _correctLetterData;
    private AlphabetData.LetterData _selectedLetterData;

    private SpriteRenderer _dispayedImage;
    void Start()
    {
        _dispayedImage = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(AlphabetData.LetterData normalLetterData, AlphabetData.LetterData selectedLetterData, AlphabetData.LetterData correctLtterData)
    {
        _normalLetterData = normalLetterData;
        _selectedLetterData = selectedLetterData;
        _correctLetterData = correctLtterData;

        GetComponent<SpriteRenderer>().sprite = _normalLetterData.image;
    }
}
