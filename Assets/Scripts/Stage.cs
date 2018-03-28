using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
	private Dictionary<TileVector, Tile> _map = new Dictionary<TileVector, Tile>();

	public void AddTileEntity(TileEntity ent)
	{
		
	}
	
	[RequireComponent(typeof(TileEntity))]
	public class Tile : MonoBehaviour
	{
		public bool BlockPlayer;
		public bool BlockSpell;
		
		public TileEntity Occupant;
	}
	
	
}
