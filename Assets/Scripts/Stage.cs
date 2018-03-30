using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Stage : MonoBehaviour
{
	private Dictionary<TileVector, Tile> _map = new Dictionary<TileVector, Tile>();
	
#if UNITY_EDITOR
	internal void Update()
	{
		foreach (var ent in GetComponentsInChildren<TileEntity>())
		{
			var newPos = ent.transform.localPosition.ToNearestTile();
			if (true) ent.TilePos = newPos;	// entry point for conditional snapping
			ent.SnapToWorldPos();
		}
	}
#endif

	[RequireComponent(typeof(TileEntity))]
	public class Tile : MonoBehaviour
	{
		public bool BlockPlayer;
		public bool BlockSpell;
		
		public TileEntity Occupant;
	}
	
	
}
