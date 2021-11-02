using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersBoardController : MonoBehaviour
{
    public Piece[,] pieces = new Piece[8, 8];
    public GameObject whitePiece;
    public GameObject blackPiece;

    public Vector3 boardOffset = new Vector3(-4.0f, 0, -4.0f);
    //public Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    public void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        for (int y = 0; y < 3; y++)
        {
            for(int x = 0; x < 8; x += 2)
            {
                GeneratePiece(x, y);
            }
        }
    }

    private void GeneratePiece(int x, int y)
    {
        GameObject go = Instantiate(whitePiece) as GameObject;
        go.transform.SetParent(transform);

        Piece p = go.GetComponent<Piece>();
        pieces[x, y] = p;
        MovePiece(p, x, y);
    }

    private void MovePiece(Piece p, int x, int y)
    {
        p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset;
    }
}
