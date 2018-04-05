using System;
using UnityEngine;

public class TileEntity : MonoBehaviour
{
	[NonSerialized]
	public Stage Stage;
	
	public TileVector TilePos;

	[NonSerialized]
	public StageTile Tile;
	public bool IsTile
	{
		get { return Tile != null; }
	}

	public void SnapToWorldPos()
	{
		transform.localPosition = TilePos.ToVector3();
	}

	private void Start()
	{
		Tile = GetComponent<StageTile>();
		Stage = GetComponentInParent<Stage>();
	}

	private void OnValidate()
	{
		Start();
		if (Stage == null && this.isActiveAndEnabled)
		{
			Debug.LogWarningFormat("{0} is not child of a Stage", this);
		}
	}
}