using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class TerrainChunk : MonoBehaviour, IChunk {

	readonly BlockData[] _data = new BlockData[Chunk.SIZE * Chunk.SIZE * Chunk.SIZE];

	Mesh _mesh = null;
	

	#region IChunk implementation

	public Terrain terrain { get; internal set; }

	public ChunkPos position { get; internal set; }

	#endregion

	#region IRawBlockAccess implementation

	public int width { get { return Chunk.SIZE; } }
	public int depth { get { return Chunk.SIZE ; } }
	public int height { get { return Chunk.SIZE; } }
	
	public BlockData this[int index] {
		get { return _data[index]; }
		set { _data[index] = value; }
	}

	#endregion


	public void UpdateMesh() {
		if (_mesh == null) {
			_mesh = new Mesh();
			GetComponent<MeshFilter>().sharedMesh = _mesh;
			GetComponent<MeshRenderer>().sharedMaterial =
				terrain.GetComponent<MeshRenderer>().sharedMaterial;
		}
		var access = new IRawBlockAccess[8];
		for (var i = 0; i < access.Length; i++)
			access[i] = terrain[
				new ChunkPos(position.x + (i & 1),
				             position.y + ((i >> 1) & 1),
				             position.z + ((i >> 2) & 1))];
		SurfaceNetsMeshGenerator.Generate(_mesh, access, terrain);

		// This should also update the collision mesh properly.
		GetComponent<MeshCollider>().sharedMesh = _mesh;
	}

}
