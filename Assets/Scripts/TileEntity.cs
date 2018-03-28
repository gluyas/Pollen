using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class TileEntity : MonoBehaviour
{
	[SerializeField]
	internal Stage _stage;
	public Stage Stage {
		get { return _stage; }
		set { _stage = value; }
	}

	[SerializeField]
	[HideInInspector]
	internal TileVector _tilePos;
	public TileVector TilePos { 
		get { return _tilePos; }
		set { _tilePos = value; }
	}
	
	public void SnapToWorldPos()
	{
		transform.localPosition = TilePos.ToVector3();
	}

#if UNITY_EDITOR
	internal void Update()
	{
		TilePos = transform.localPosition.ToNearestTile();
		SnapToWorldPos();
	}
#endif
	
	internal void OnValidate()
	{
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
	
			tileEntity.TilePos = new TileVector(pos.x, pos.y);
			tileEntity.SnapToWorldPos();
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