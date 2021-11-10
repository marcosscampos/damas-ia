using Assets.Scripts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class PiecesCreator : MonoBehaviour
    {
        [SerializeField] private GameObject[] piecesPrefabs;
        [SerializeField] private Material blackMaterial;
        [SerializeField] private Material whiteMaterial;
        private Dictionary<string, GameObject> nameToPieceDict = new Dictionary<string, GameObject>();

        private void Awake()
        {
            foreach (var piece in piecesPrefabs)
            {
                nameToPieceDict.Add(piece.GetComponent<Piece>().GetType().ToString(), piece);
            }
        }

        public GameObject CreatePiece(Type type)
        {
            GameObject prefab = nameToPieceDict[type.ToString()];
            if (prefab)
            {
                GameObject newPiece = Instantiate(prefab);
                return newPiece;
            }
            return null;
        }

        public Material GetTeamMaterial(TeamColor team)
        {
            return team == TeamColor.White ? whiteMaterial : blackMaterial;
        }
    }
}
