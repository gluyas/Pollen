using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Plant : TileEntity
{
	[HideInInspector]
	public Object Prefab; 
	
	public PlantType Type;

	[HideInInspector]
	public bool PlacedByPlayer = false;

#if UNITY_EDITOR
	private void OnValidate()
	{
		var prefab = PrefabUtility.GetPrefabObject(this.gameObject);
		if (prefab != null)
		{
			Prefab = prefab;
		}
		else
		{
			Debug.LogAssertionFormat("{0} not an instance of any Prefab", this);
		}
	}
#endif
}

public enum PlantType
{
	Normal,
}