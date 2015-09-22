using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class TerrainChunk : MonoBehaviour, IChunk {

	readonly BlockData[] _data = new BlockData[(Chunk.SIZE + 1) * (Chunk.SIZE + 1) * (Chunk.SIZE + 1)];

	Mesh _mesh = null;
	

	#region IChunk implementation

	public Terrain terrain { get; internal set; }

	public ChunkPos position { get; internal set; }

	#endregion

	#region IRawBlockAccess implementation

	public int width { get { return Chunk.SIZE + 1; } }
	public int depth { get { return Chunk.SIZE + 1; } }
	public int height { get { return Chunk.SIZE + 1; } }
	
	public BlockData this[int index] {
		get { return _data[index]; }
		set { _data[index] = value; }
	}

	#endregion


	public void UpdateMesh() {
		if (_mesh == null) {
			_mesh = new Mesh();
			GetComponent<MeshFilter>().sharedMesh = _mesh;
			GetComponent<MeshRenderer>().material =
				terrain.GetComponent<MeshRenderer>().material;
		}
		SurfaceNetsMeshGenerator.Generate(_mesh, this, terrain);

		// This should also update the collision mesh properly.
		GetComponent<MeshCollider>().sharedMesh = _mesh;
	}

}
