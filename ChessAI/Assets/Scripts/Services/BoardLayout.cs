using Assets.Scripts.Enum;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Services
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Board/Layout")]
    public class BoardLayout : ScriptableObject
    {
        [SerializeField]
        private BoardSquareSetup[] boardSquares;

        public int GetPiecesCount()
        {
            return boardSquares.Length;
        }

        public Vector2Int GetSquareCoordsAtIndex(int index)
        {
            return new Vector2Int(boardSquares[index].position.x - 1, boardSquares[index].position.y - 1);
        }

        public string GetSquarePieceNameAtIndex(int index)
        {
            return boardSquares[index].pieceType.ToString();
        }

        public TeamColor GetSquareTeamColorAtIndex(int index)
        {
            return boardSquares[index].teamColor;
        }
    }
}
