using UnityEngine;

[RequireComponent(typeof(TileEntity))]
public class StageTile : MonoBehaviour
{
	[HideInInspector]
	public TileEntity Entity;
		
	public bool BlockPlayer;
	public bool BlockSpell;

	private void Start()
	{
		Entity = GetComponent<TileEntity>();
	}
}
