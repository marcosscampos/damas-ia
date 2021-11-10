using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services
{
    [RequireComponent(typeof(PhotonView))]
    public class MultiplayerBoard : Board
    {
        private PhotonView photonView;


        protected override void Awake()
        {
            base.Awake();
            photonView = GetComponent<PhotonView>();
        }

        public override void SelectedPieceMoved(Vector2 coords)
        {
            Debug.LogError("RPC  select");
            photonView.RPC(nameof(RPC_OnSelectedPieceMoved), RpcTarget.AllBuffered, new object[] { coords });
        }

        public override void SetSelectedPiece(Vector2 coords)
        {
            Debug.LogError("RPC select");
            photonView.RPC(nameof(RPC_SetSelectedPiece), RpcTarget.AllBuffered, new object[] { coords });
        }


        [PunRPC]
        private void RPC_SetSelectedPiece(Vector2 coords)
        {
            Debug.LogError("ON RPC select");

            Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x), Mathf.RoundToInt(coords.y));
            OnSetSelectedPiece(intCoords);
        }

        [PunRPC]
        private void RPC_OnSelectedPieceMoved(Vector2 coords)
        {
            Debug.LogError("ON RPC move");

            Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x), Mathf.RoundToInt(coords.y));
            OnSelectedPieceMoved(intCoords);
        }

    }
}
