using Assets.Scripts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class CameraSetup : MonoBehaviour
    {
        [SerializeField] Camera mainCamera;

        public void SetupCamera(TeamColor team)
        {
            if (team == TeamColor.Black)
            {
                FlipCamera();
            }
        }

        private void FlipCamera()
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -mainCamera.transform.position.z);
            mainCamera.transform.Rotate(Vector3.up, 180f, Space.World);
        }
    }
}
