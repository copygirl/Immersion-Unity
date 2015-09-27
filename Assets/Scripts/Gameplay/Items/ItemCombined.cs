using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> Item component for items that are made up of
///           multiple parts, which are items themselves. </summary>
public class ItemCombined : Item {

	readonly List<Item> _parts = new List<Item>();


	public IEnumerable<Item> parts { get { return _parts.Select(i => i); } }

	public override float weight {
		get { return parts.Sum(p => p.weight); }
		set { throw new NotSupportedException("Cannot set weight of ItemCombined"); }
	}


	void Start() {
		// Go over all children and add any items to parts collection.
		_parts.AddRange(
			transform.Cast<Transform>()
			         .Select(t => t.GetComponent<Item>())
			         .Where(i => (i != null)));
	}


	/// <summary> Adds a part to this item. </summary>
	public void AddPart(Item part) {
		if (part == null)
			throw new ArgumentNullException("part");
		if (part.transform.parent != null)
			throw new ArgumentException("Part already has a parent", "part");

		_parts.Add(part);

		part.transform.parent = transform;
		part.transform.ResetPositionAndRotation();
	}
	
	/// <summary> Removes a part from this item. </summary>
	public void RemovePart(Item part) {
		if (part == null)
			throw new ArgumentNullException("part");
		if (!_parts.Contains(part))
			throw new ArgumentException(string.Format(
				"{0} is not a part of {1}", part, this), "part");

		_parts.Remove(part);

		part.transform.parent = null;
	}

}
