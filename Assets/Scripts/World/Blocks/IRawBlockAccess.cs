
/// <summary> Interface for getting raw access to block data.
///           Useful for performance critical tasks such as
///           terrain and mesh generation. </summary>
public interface IRawBlockAccess {

	int width { get; }
	int depth { get; }
	int height { get; }

	/// <summary> Returns the raw block data array. </summary>
	BlockData[] blockData { get; }

}
