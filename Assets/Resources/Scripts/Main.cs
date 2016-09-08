using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	public static int phase;
	public static GameObject pointA;
	public static GameObject pointB;
	public static List<List<GameObject>> TheWay = new List<List<GameObject>>();

	void Start()
	{
		phase = 1;
	}
}
