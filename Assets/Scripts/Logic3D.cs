using UnityEngine;

public static class Logic3D {	
	// modify these values to change the orientation of the board.
	public const float Scale = 2;
	public static readonly Vector3 North = Vector3.forward * Scale;
	public static readonly Vector3 Up 	 = Vector3.up 		* Scale;

	// the following are defined in terms of the above values
	public static readonly Vector3 EVector = Quaternion.AngleAxis(120, Up) * North;	// 120 from North
	public static readonly Vector3 WVector = Quaternion.AngleAxis(120, Up) * EVector;	// 120 from Southeast

	public static Vector3 ToVector3(this TileVector tv)
	{
		return tv.W * WVector + tv.E * EVector;
	}

	public static Vector3 ToVector3(this CardinalDirection d)
	{
		return d.GetTileVector().ToVector3();
	}
}
