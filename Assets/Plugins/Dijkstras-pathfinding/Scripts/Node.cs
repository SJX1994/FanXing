﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DijkstrasPathfinding
{
/// <summary>
/// The Node.
/// </summary>
public class Node : MonoBehaviour
{

	/// <summary>
	/// The connections (neighbors).
	/// </summary>
	
	[SerializeField]
	protected List<Node> m_Connections = new List<Node> ();

	/// <summary>
	/// Gets the connections (neighbors).
	/// </summary>
	/// <value>The connections.</value>
	public virtual List<Node> connections
	{
		get
		{
			return m_Connections;
		}
	}

	public Node this [ int index ]
	{
		get
		{
			return m_Connections [ index ];
		}
	}

	void OnValidate ()
	{
		if (m_Connections == null)return;
		// Removing duplicate elements
		m_Connections = m_Connections.Distinct ().ToList ();
	}
	
}
}