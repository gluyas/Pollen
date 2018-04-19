using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantable : TileEntity
{
	public PlantableState State = PlantableState.Vacant;
}

[Serializable]
public enum PlantableState
{
	Vacant,
	Imbued,
	Solved,
}
