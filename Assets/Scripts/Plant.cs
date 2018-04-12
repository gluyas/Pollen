using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : TileEntity
{
	public PlantType Type;
}

public enum PlantType
{
	Normal,
}