using UnityEditor;
using UnityEngine;

public class TileEntity : MonoBehaviour
{
	[HideInInspector]
	public Stage Stage;

	[HideInInspector] 
	public TileVector TilePos;

	[HideInInspector]
	public StageTile Tile;
	public bool IsTile
	{
		get { return Tile != null; }
	}

	public void SnapToWorldPos()
	{
		transform.localPosition = TilePos.ToVector3();
	}

	internal void OnValidate()
	{
		Tile = GetComponent<StageTile>();
		
		{
			var stage = GetComponentInParent<Stage>();
			if (stage == null)
			{
				Debug.LogWarning("TileEntity must be direct child of a Stage");
				Stage = null;
			}
			else
			{
				Stage = stage;
			}
		}
	}
}

[CustomEditor(typeof(TileEntity))]
internal class TileEntityEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var tileEntity = (TileEntity) target;

		{	// TilePos editor
			var pos = EditorGUILayout.Vector2IntField(
				"Tile Pos", new Vector2Int(tileEntity.TilePos.W, tileEntity.TilePos.E));
	
			var newPos = new TileVector(pos.x, pos.y);
			if (newPos != tileEntity.TilePos)
			{
				tileEntity.SnapToWorldPos();
			}			
		}

		{	// reset pos button
			if (GUILayout.Button("Refresh"))
			{
				tileEntity.SnapToWorldPos();
				tileEntity.OnValidate();
			}
		}
	}
}