using System;
using UnityEngine;

[RequireComponent(typeof(TileEntity))]
public class StageTile : MonoBehaviour
{
	[NonSerialized]
	public TileEntity Entity;

	public bool BlockPlayer;
	public bool BlockSpell;

	private void OnMouseUpAsButton()
	{
		Entity.Stage.OnTileClick(this);
	}

	private void Start()
	{
		Entity = GetComponent<TileEntity>();
	}

	private void OnValidate()
	{
		Start();
	}
}
