using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Spell class which represents a single step in a Spell invocation
/// </summary>
public class Spell
{
	public TileVector Pos;
	public CardinalDirection Dir;

	public Plant Plant;

	public IEnumerable<Plantable> PlantableArea;

	public Spell(TileVector pos, CardinalDirection dir, IEnumerable<Plantable> plantableArea)
	{
		Pos = pos;
		Dir = dir;
		PlantableArea = plantableArea;
	}

	public static List<Spell> Invoke(Stage stage, TileVector pos, CardinalDirection dir)
	{
		var map = stage.ToDictionary();
		var history = new List<Spell>();
		
		HashSet<Plantable> currentPlantableArea = null;
		
		while (map.ContainsKey(pos))
		{
			var space = map[pos];

			if (currentPlantableArea == null)
			{
				if (space.Plantable != null && space.Plantable.State == PlantableState.Vacant)
				{
					currentPlantableArea = new HashSet<Plantable>();
					
					var fringe = new Queue<TileVector>();
					fringe.Enqueue(pos);			
				
					while (fringe.Count > 0)	// find all adjacent plantables.
					{
						var fringePos = fringe.Dequeue();
						if (!map.ContainsKey(fringePos)) continue;
				
						var plantable = map[fringePos].Plantable;
						if (plantable == null || currentPlantableArea.Contains(plantable)) continue;
				
						currentPlantableArea.Add(plantable);
						foreach (var adj in fringePos.Adjacent())
						{
							fringe.Enqueue(adj);
						}
					}
				}
			}
			else if (!currentPlantableArea.Contains(space.Plantable))	// stepped out of the plant area
			{
				bool solved = currentPlantableArea.All(p =>
				{
					var plant = map[p.TilePos].Occupant as Plant;
					return plant != null && plant.Hits > 0;
				});

				foreach (var plantable in currentPlantableArea)
				{
					plantable.State = solved ? PlantableState.Solved : PlantableState.Imbued;
				}
				currentPlantableArea = null;
			}
			
			var spell = new Spell(pos, dir, currentPlantableArea);	// record this frame of the spell invocation
			history.Add(spell);

			if (space.HasTile && !space.Tile.BlockSpell)	// advance spell
			{
				var plant = space.Occupant as Plant;
				if (plant != null)
				{
					spell.Plant = plant;			// add plant to record, for convinience
					
					switch (plant.Type)				// trigger plant side effects
					{
						case PlantType.Twist:
							dir = dir.ArcClockwise(1);
							break;
					}

					if (currentPlantableArea != null) // implies this area is being activated by this current spell
					{
						plant.Hits += 1; // make plant grow
					}
				}
				pos = pos + dir;
			}
			else
			{
				if (currentPlantableArea != null)
				{
					foreach (var plantable in currentPlantableArea)
					{
						plantable.State = PlantableState.Imbued;
					}
				}
				break;		// end the spell invocation
			}
		}
		return history;
	}
}
