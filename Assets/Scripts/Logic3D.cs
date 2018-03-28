using System.Threading;
using UnityEngine;

public static class Logic3D {	
	// modify these values to change the orientation of the board.
	public const float Scale = 1;
	public static readonly Vector3 North = Vector3.forward * Scale;
	public static readonly Vector3 Up 	 = Vector3.up 	   * Scale;

	// the following are defined in terms of the above values
	public static readonly Vector3 EVector = Quaternion.AngleAxis(120, Up)  * North;    // 120 from North
	public static readonly Vector3 WVector = Quaternion.AngleAxis(-120, Up) * North;	// 120 from Southeast
	
	public static readonly Matrix4x4 Inverse = GetTileInverse();

	private static Matrix4x4 GetTileInverse()
	{
		var mat = Matrix4x4.identity;	// mat into std basis
		mat.SetColumn(0, WVector);
		mat.SetColumn(1, EVector);
		mat.SetColumn(2, Up);
		return mat.inverse;
	}
	
	
	public static Vector3 ToVector3(this TileVector tv)
	{
		var mat = Matrix4x4.identity;	// mat into std basis
		mat.SetColumn(0, WVector);
		mat.SetColumn(1, EVector);
		mat.SetColumn(2, Up);
		return mat.MultiplyPoint(new Vector3(tv.W, tv.E, 0));
	}

	public static Vector3 ToVector3(this CardinalDirection d)
	{
		return d.GetTileVector().ToVector3();
	}

	public static TileVector ToNearestTile(this Vector3 v)
	{
		var tilePos = Inverse.MultiplyPoint(v);
		return new TileVector(Mathf.RoundToInt(tilePos.x), Mathf.RoundToInt(tilePos.y));
	}
}
