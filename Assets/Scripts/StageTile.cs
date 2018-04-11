using System;
using UnityEngine;

public class StageTile : TileEntity
{
	public int Elevation;

	public TileVectorTriplet TilePosTriplet
	{
		get { return new TileVectorTriplet(TilePos, Elevation);}
	}
	
	public bool BlockPlayer;
	public bool BlockSpell;

	private void OnMouseUpAsButton()
	{
		Stage.OnTileClick(this);
	}

	private void OnValidate()
	{
		SnapToWorldPos(Elevation);
	}
}
