using System;
using System.Collections;
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
	
	public IEnumerator MoveTo(TileVector target)
	{
		TilePos = target;
		
		var from = transform.localPosition;
		var to = target.ToVector3();
		var startTime = Time.time;	
		var time = 0.5f;
		
		float t;
		do
		{
			t = (Time.time - startTime) / time;
			transform.localPosition = Vector3.Lerp(from, to, t);
			yield return new WaitForFixedUpdate();
		} while (t < 1);
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