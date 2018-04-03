using UnityEngine;

[RequireComponent(typeof(TileEntity))]
public class Player : MonoBehaviour
{
	public TileEntity Entity;
	
	private void OnValidate()
	{
		Entity = GetComponent<TileEntity>();
	}
}
