using Assets.Scripts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class BoardSquareSetup : ScriptableObject
    {
        public Vector2Int position;
        public PieceType pieceType;
        public TeamColor teamColor;
    }
}
