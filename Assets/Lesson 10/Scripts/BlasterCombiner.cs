using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lesson10
{
    public class BlasterCombiner : MonoBehaviour
    {
        [SerializeField] private MeshFilter[] _meshFilters;

        private Mesh _mesh;

        [ContextMenu("Generate Blaster Mesh")]
        private void GenerateBlasterMesh()
        {
        _mesh = new Mesh();

        Matrix4x4 objectMatrix = transform.worldToLocalMatrix;
        CombineInstance[] instances = new CombineInstance[_meshFilters.Length];

        for (int i = 0; i < _meshFilters.Length; ++i)

        {
            CombineInstance instance = new CombineInstance();
            instance.mesh = _meshFilters[i].sharedMesh;
            instance.transform = objectMatrix * _meshFilters[i].transform.localToWorldMatrix;
            instances[i] = instance;
        }

        _mesh.CombineMeshes(instances, mergeSubMeshes:false, useMatrices:true);
#if UNITY_EDITOR
            string path = EditorUtility.SaveFilePanelInProject(title: "Save Blaster Mesh", defaultName: "New Blaster Mesh", extension: "asset", message: "");
            AssetDatabase.CreateAsset(_mesh, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }
    }
}
