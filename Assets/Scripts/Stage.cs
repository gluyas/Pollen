using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Stage : MonoBehaviour
{
	#region entity data model	
	private TileEntity[] _entities;
	public  TileEntity[] Entities
	{
		get
		{
#if UNITY_EDITOR
			return GetComponentsInChildren<TileEntity>();
#else
			if (_entities == null)
			{
				_entities = GetComponentsInChildren<TileEntity>();
			}
			return _entities;
#endif
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
					if (entity is StageTile)      data.Tile      = entity as StageTile;
					else if (entity is Plantable) data.Plantable = entity as Plantable;
					else                          data.Occupant  = entity;
					
					if (data.Occupant != null && data.Tile != null && data.Plantable != null) break;
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
			if (ent is StageTile)
			{
				if (data.Tile == null) data.Tile = ent as StageTile;
			}
			else if (ent is Plantable)
			{
				if (data.Plantable == null) data.Plantable = ent as Plantable;
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
		if (player == null || player.TilePos == tile.TilePos) return;

		var space = this[tile.TilePos];
		Debug.AssertFormat(space.Tile == tile, "Tile mismatch at {0}", tile.TilePos);

		if (space.CanPlayerOccupy)
		{
			StartCoroutine(player.MoveTo(tile.TilePosTriplet));
		}
	}
	
#if UNITY_EDITOR
	private void Update()
	{
		if (EditorApplication.isPlaying) return;
		foreach (var ent in Entities)
		{
			var newTriplet = ent.transform.localPosition.ToNearestTileTriplet();
			var newPos = newTriplet.Horizontal;
			
			// determine new horizontal positioning
			TileData space;
			if (newPos != ent.TilePos)
			{
				space = this[newPos];
				if (ent is StageTile)
				{
					if (!space.HasTile) ent.TilePos = newPos;
				}
				else if (ent is Plantable)
				{
					if (!space.HasPlantable) ent.TilePos = newPos;
				}
				else 
				{
					if (!space.HasOccupant) ent.TilePos = newPos;
				}
				EditorUtility.SetDirty(ent);
			}
			else
			{
				space = this[ent.TilePos];
			}
			
			// determine elevation and snap to position
			int elevation;
			if (ent is StageTile)
			{
				var tile = ent as StageTile;
				tile.Elevation = newTriplet.Vertical;
				elevation = tile.Elevation;
				
				EditorUtility.SetDirty(tile);
			}
			else
			{
				elevation = space.HasTile ? space.Tile.Elevation : 0;
			}
			ent.SnapToWorldPos(elevation);
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
		public Plantable Plantable;
		
		public bool HasTile
		{
			get { return Tile != null; }
		}
		
		public bool HasOccupant
		{
			get { return Occupant != null; }
		}

		public bool HasPlantable
		{
			get { return Plantable != null; }
		}

		public bool CanPlayerOccupy
		{
			get { return Occupant == null && Tile != null && !Tile.BlockPlayer; }
		}
	}
}
