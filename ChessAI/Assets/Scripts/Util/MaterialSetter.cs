using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class MaterialSetter : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _meshRenderer;

        private MeshRenderer meshRenderer
        {
            get
            {
                if(_meshRenderer == null)
                    _meshRenderer = GetComponent<MeshRenderer>();
                return _meshRenderer;
            }
        }

        public void SetSingleMaterial(Material material)
        {
            meshRenderer.material = material;
        }
    }
}
