using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{
    public GameData curentGameData;
    public GameObject gridSquarePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.0f;
    public float topPosition;

    private List<GameObject> _squareList = new List<GameObject>();

    void Stat()
    {
        SpawnGridSquares();
        SetSquresPosition();
    }

    private void SetSquresPosition()
    {
        var squreRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squreTransform = _squareList[0].GetComponent<Transform>();
        var offset = new Vector2
        {
            x = (squreRect.width * squreTransform.localScale.x + squareOffset) * 0.01f,
            y = (squreRect.height * squreTransform.localScale.y + squareOffset) * 0.01f
        };

        var startPos = GetFirstSquarePos();
        int collumNumber = 0;
        int rowNumber = 0;

        foreach (var squre in _squareList)
        {
            if (rowNumber + 1 > curentGameData.selectedBoardData.Rows)
            {
                collumNumber++;
                rowNumber = 0;
            }

            var positionX = startPos.x + offset.x * collumNumber;
            var positionY = startPos.y - offset.y * rowNumber;

            squre.GetComponent<Transform>().position = new Vector2(positionX, positionY);
            rowNumber++;
        }
    }

    private Vector2 GetFirstSquarePos()
    {
        var startPos = new Vector2(0f, transform.position.y);
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTrans = _squareList[0].GetComponent<Transform>();
        var squareSize = new Vector2(0f, 0f);

        squareSize.x = squareRect.width * squareTrans.localScale.x;
        squareSize.y = squareRect.height * squareTrans.localScale.y;

        var midWithPos = ((curentGameData.selectedBoardData.Columns - 1 * squareSize.x) / 2) * 0.01f;
        var midWithHeight = ((curentGameData.selectedBoardData.Rows - 1 * squareSize.y) / 2) * 0.01f;

        startPos.x = (midWithPos != 0) ? midWithPos * -1 : midWithPos;
        startPos.y += midWithHeight;
        return startPos;
    }

    private void SpawnGridSquares()
    {
        if (curentGameData != null)
        {
            var squreScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));
            foreach (var squres in curentGameData.selectedBoardData.Board)
            {
                foreach (var squreLetter in squres.Row)
                {
                    var normalLetterData = alphabetData.AlphabetNormal.Find(data => data.letter == squreLetter);
                    var selectedLetterData = alphabetData.AlphabetHighlited.Find(data => data.letter == squreLetter);
                    var correctLetter = alphabetData.AlphabetWrong.Find(data => data.letter == squreLetter);

                    if (normalLetterData.image == null || selectedLetterData.image == null)
                    {
                        Debug.LogError("Error Fullil all. Press Fill up with random button! Letter: " + squreLetter);

#if UNITY_EDITOR
                        if (UnityEditor.EditorApplication.isPlaying)
                        {
                            UnityEditor.EditorApplication.isPlaying = false;
                        }
#endif                  
                    }
                    else
                    {
                        _squareList.Add(Instantiate(gridSquarePrefab));
                        _squareList[_squareList.Count - 1].GetComponent<Gridquare>().SetSprite(normalLetterData, correctLetter, selectedLetterData);
                        _squareList[_squareList.Count - 1].transform.SetParent(this.transform);
                        _squareList[_squareList.Count - 1].GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);
                        _squareList[_squareList.Count - 1].transform.localScale = squreScale;
                    }
                }
            }
        }
    }

    private Vector3 GetSquareScale(Vector3 defultscale)
    {
        var finalScale = defultscale;
        var ajusment = 0.01f;

        while (SholdScaleDown(finalScale))
        {
            finalScale.x -= ajusment;
            finalScale.y -= ajusment;

            if (finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = ajusment;
                finalScale.y = ajusment;
                return finalScale;
            }
        }
        return finalScale;
    }

    private bool SholdScaleDown(Vector3 targetScale)
    {
        var squareRect = gridSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect ;
        var squareSize = new Vector2(0f,0f);
        var startPosition = new Vector2(0f, 0f);

        squareSize.x = (squareRect.width * targetScale.x) + squareOffset;
        squareSize.y = (squareRect.height * targetScale.y) + squareOffset;

        var midWidthPosition = ((curentGameData.selectedBoardData.Columns * squareSize.x) / 2) * 0.01f;
        var MidWidthHight = ((curentGameData.selectedBoardData.Rows * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y = MidWidthHight;

        return startPosition.x < GethalfScreenWidth() * -1 || startPosition.y > topPosition;
    }

    private float GethalfScreenWidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;
        return width / 2;
    }
}
