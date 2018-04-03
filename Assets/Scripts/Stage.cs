using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Stage : MonoBehaviour
{
	#region entity data model	
	private TileEntity[] _entities;
	public  TileEntity[] Entities
	{
		get
		{
			if (_entities == null)
			{
				_entities = GetComponentsInChildren<TileEntity>();
			}
			return _entities;
		}
	}
	
	/// <summary>
	/// Get the gameplay state at a given tile position
	/// Performs in O(n) time for the number of entities. Consider using ToDictionary.
	/// </summary>
	/// <param name="pos">Position to query</param>
	public TileData this[TileVector pos]
	{
		get
		{
			var data = new TileData();
			foreach (var entity in Entities)
			{
				if (entity.TilePos == pos)
				{
					if (entity.IsTile) data.Tile     = entity.Tile;
					else               data.Occupant = entity;
					
					if (data.Occupant != null && data.Tile != null) break;
				}
			}
			return data;
		}
	}

	/// <summary>
	/// Copies the current Stage data into a Dictionary to improve querying speed.
	/// Subsequent state changes will not be reflected in this object.
	/// </summary>
	/// <returns>A Dictionary representation of this Stage's current state</returns>
	public Dictionary<TileVector, TileData> ToDictionary()
	{
		var map = new Dictionary<TileVector, TileData>();
		foreach (var ent in Entities)
		{
			var data = map.ContainsKey(ent.TilePos) ? map[ent.TilePos] : new TileData();
			if (ent.IsTile)
			{
				if (data.Tile == null) data.Tile = ent.Tile;
			}
			else
			{
				if (data.Occupant == null) data.Occupant = ent;
			}
			map[ent.TilePos] = data;
		}
		return map;
	}
	#endregion

	public void OnTileClick(StageTile tile)
	{
		// move the player to that tile, if possible
		var player = GetComponentInChildren<Player>();
		if (player == null || player.Entity.TilePos == tile.Entity.TilePos) return;

		var space = this[tile.Entity.TilePos];
		Debug.AssertFormat(space.Tile == tile, "Tile mismatch at {0}", tile.Entity.TilePos);

		if (space.CanPlayerOccupy)
		{
			player.Entity.TilePos = tile.Entity.TilePos;
			player.Entity.SnapToWorldPos();
		}
	}
	
#if UNITY_EDITOR
	private void Update()
	{
		foreach (var ent in Entities)
		{
			var newPos = ent.transform.localPosition.ToNearestTile();
			if (newPos != ent.TilePos)
			{

				var data = this[newPos];
				if (ent.IsTile && !data.HasTile || !ent.IsTile && !data.HasOccupant)
				{
					ent.TilePos = newPos;
				}
			}
			ent.SnapToWorldPos();
		}
	}
#endif

	private void OnTransformChildrenChanged()
	{
		_entities = null;
	}

	/// <summary>
	/// Represents the game state at a single grid tile
	/// </summary>
	[Serializable]
	public struct TileData
	{
		public StageTile Tile;
		public TileEntity Occupant;

		public bool HasTile
		{
			get { return Tile != null; }
		}
		
		public bool HasOccupant
		{
			get { return Occupant != null; }
		}

		public bool CanPlayerOccupy
		{
			get { return Occupant == null && Tile != null && !Tile.BlockPlayer; }
		}
	}
}
