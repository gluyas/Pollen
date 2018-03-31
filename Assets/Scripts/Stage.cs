using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Stage : MonoBehaviour
{
	public TileData this[TileVector pos]
	{
		get
		{
			var data = new TileData();
			foreach (var entity in GetComponentsInChildren<TileEntity>())
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

	public Dictionary<TileVector, TileData> ToDictionary()
	{
		var map = new Dictionary<TileVector, TileData>();
		foreach (var ent in GetComponentsInChildren<TileEntity>())
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
	
#if UNITY_EDITOR
	internal void Update()
	{
		foreach (var ent in GetComponentsInChildren<TileEntity>())
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

	[RequireComponent(typeof(TileEntity))]
	public class Tile : MonoBehaviour
	{
		public TileEntity Entity;
		
		public bool BlockPlayer;
		public bool BlockSpell;

		private void Start()
		{
			Entity = GetComponent<TileEntity>();
		}
	}
	
	[Serializable]
	public struct TileData
	{
		public Tile Tile;
		public TileEntity Occupant;

		public bool HasTile
		{
			get { return Tile != null; }
		}
		
		public bool HasOccupant
		{
			get { return Occupant != null; }
		}
	}
}
