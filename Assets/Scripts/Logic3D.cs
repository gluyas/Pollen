using System;
using System.Threading;
using UnityEngine;

public static class Logic3D {	
	// modify these values to change the orientation of the board.
	public const float Scale = 1;
	public const float VerticalScale = 1/6f;
	public static readonly Vector3 North = Vector3.forward * Scale;
	public static readonly Vector3 Up 	 = Vector3.up 	   * Scale * VerticalScale;

	// the following are defined in terms of the above values
	public static readonly Vector3 EVector = Quaternion.AngleAxis(120, Up)  * North;    // 120 from North
	public static readonly Vector3 WVector = Quaternion.AngleAxis(-120, Up) * North;	// 120 from Southeast

	public static readonly Matrix4x4 ToVec3   = TileToVec3Matrix();
	public static readonly Matrix4x4 FromVec3 = ToVec3.inverse;

	private static Matrix4x4 TileToVec3Matrix()
	{
		var mat = Matrix4x4.identity;	// mat into std basis
		mat.SetColumn(0, WVector);		// West -> Vec3.x
		mat.SetColumn(1, Up);			// Up   -> Vec3.y
		mat.SetColumn(2, EVector);		// East -> Vec3.z
		return mat;
	}
	
	public static Vector3 ToVector3(this TileVector tv)
	{
		return ToVec3.MultiplyPoint(new Vector3(tv.W, 0, tv.E));
	}

	public static Vector3 ToVector3(this CardinalDirection d)
	{
		return d.GetTileVector().ToVector3();
	}

	public static Vector3 ToVector3(this TileVectorTriplet tv3)
	{
		var tv = tv3.Horizontal;
		return ToVec3.MultiplyPoint(new Vector3(tv.W, tv3.Vertical, tv.E));
	}
	
	public static TileVector ToNearestTile(this Vector3 v)
	{
		return ToNearestTileTriplet(v).Horizontal;
	}
	
	public static TileVectorTriplet ToNearestTileTriplet(this Vector3 v)
	{
		var tilePos = FromVec3.MultiplyPoint(v);
		
		var horizontal = new TileVector(Mathf.RoundToInt(tilePos.x), Mathf.RoundToInt(tilePos.z));
		var vertical = Mathf.RoundToInt(tilePos.y);
		
		return new TileVectorTriplet(horizontal, vertical);
	}
}

[Serializable]
public struct TileVectorTriplet
{
	public TileVector Horizontal;
	public int Vertical;

	public TileVectorTriplet(TileVector horizontal, int vertical = 0)
	{
		Horizontal = horizontal;
		Vertical = vertical;
	}
}
