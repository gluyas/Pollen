using UnityEngine;

[RequireComponent(typeof(TileEntity))]
public class StageTile : MonoBehaviour
{
	[HideInInspector]
	public TileEntity Entity;
		
	public bool BlockPlayer;
	public bool BlockSpell;

	private void OnMouseUpAsButton()
	{
		Entity.Stage.OnTileClick(this);
	}

	private void OnValidate()
	{
		Entity = GetComponent<TileEntity>();
	}
}
