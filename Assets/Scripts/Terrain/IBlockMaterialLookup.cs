
/// <summary> Interface for looking up block materials
///           by their IDs and the other way around. </summary>
public interface IBlockMaterialLookup {

	int GetMaterialId(BlockMaterial material);

	BlockMaterial GetMaterial(int materialId);


	int RegisterMaterial(BlockMaterial material);

}
