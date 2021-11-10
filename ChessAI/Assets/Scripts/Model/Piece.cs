using Assets.Scripts.Abstractions;
using Assets.Scripts.Enum;
using Assets.Scripts.Services;
using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MaterialSetter))]
[RequireComponent(typeof(IObjectTweetner))]
public abstract class Piece : MonoBehaviour
{
    [SerializeField]
    private MaterialSetter materialSetter;
    public Board board { protected get; set; }
    public Vector2Int occupiedSquare { get; set; }
    public TeamColor team { get; set; }
    public bool hasMoved { get; set; }
    public List<Vector2Int> availableMoves;

    private IObjectTweetner tweetner;

    public abstract List<Vector2Int> SelectAvailableSquares();


    	private void Awake()
	{
		availableMoves = new List<Vector2Int>();
		tweetner = GetComponent<IObjectTweetner>();
		materialSetter = GetComponent<MaterialSetter>();
		hasMoved = false;
	}

	public void SetMaterial(Material selectedMaterial)
	{
		materialSetter.SetSingleMaterial(selectedMaterial);
	}

	public bool IsFromSameTeam(Piece piece)
	{
		return team == piece.team;
	}

	public bool CanMoveTo(Vector2Int coords)
	{
		return availableMoves.Contains(coords);
	}

	public virtual void MovePiece(Vector2Int coords)
	{

	}


	protected void TryToAddMove(Vector2Int coords)
	{
		availableMoves.Add(coords);
	}

	public void SetData(Vector2Int coords, TeamColor team, Board board)
	{
		this.team = team;
		occupiedSquare = coords;
		this.board = board;
		transform.position = board.CalculatePositionFromCoords(coords);
	}

	public bool IsAttackingPieceOfType<T>() where T : Piece
	{
		foreach (var square in availableMoves)
		{
			if (board.GetPieceOnSquare(square) is T)
				return true;
		}
		return false;
	}
}
