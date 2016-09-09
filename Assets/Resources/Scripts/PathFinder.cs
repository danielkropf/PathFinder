using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {

	public GameObject go;
	private List<GameObject> sides = new List<GameObject>();

	void Start()
	{
		go = Main.pointA;
	}
	void Update () {
		if (Main.phase == 4)
		{
			Main.phase = 5;
			go = Main.pointA;
			PathFind();
		}
	}

	private void PathFind() 
	{
		if (go != Main.pointB)
		{
			#region Geting Side Places

			#region Right
			if (go.GetComponent<MainGrid>().Right != null)
			{
				sides.Add(go.GetComponent<MainGrid>().Right);

				#region Right.Up
				if (go.GetComponent<MainGrid>().Right.GetComponent<MainGrid>().Up != null)
					sides.Add(go.GetComponent<MainGrid>().Right.GetComponent<MainGrid>().Up);
				#endregion

				#region Right.Down
				if (go.GetComponent<MainGrid>().Right.GetComponent<MainGrid>().Down != null)
					sides.Add(go.GetComponent<MainGrid>().Right.GetComponent<MainGrid>().Down);
				#endregion
			}
			#endregion

			#region Left
			if (go.GetComponent<MainGrid>().Left != null)
			{
				sides.Add(go.GetComponent<MainGrid>().Left);

				#region Left.Up
				if (go.GetComponent<MainGrid>().Left.GetComponent<MainGrid>().Up != null)
					sides.Add(go.GetComponent<MainGrid>().Left.GetComponent<MainGrid>().Up);
				#endregion

				#region Left.Down
				if (go.GetComponent<MainGrid>().Left.GetComponent<MainGrid>().Down != null)
					sides.Add(go.GetComponent<MainGrid>().Left.GetComponent<MainGrid>().Down);
				#endregion
			}
			#endregion

			#region Up
			if (go.GetComponent<MainGrid>().Up != null)
				sides.Add(go.GetComponent<MainGrid>().Up);
			#endregion

			#region Down
			if (go.GetComponent<MainGrid>().Down != null)
				sides.Add(go.GetComponent<MainGrid>().Down);
			#endregion

			#endregion

			#region Geting The Prices

			for (int i = 0; i < sides.Count; i++)
			{
				int difAltura = sides[i].GetComponent<MainGrid>().altura - Main.pointB.GetComponent<MainGrid>().altura;
				if (difAltura < 0) difAltura *= -10;
				else difAltura *= 10;

				int difLargura = sides[i].GetComponent<MainGrid>().largura - Main.pointB.GetComponent<MainGrid>().largura;
				if (difLargura < 0) difLargura *= -10;
				else difLargura *= 10;

				if (sides[i] == go.GetComponent<MainGrid>().Left || sides[i] == go.GetComponent<MainGrid>().Up || sides[i] == go.GetComponent<MainGrid>().Right || sides[i] == go.GetComponent<MainGrid>().Down)
				{
					sides[i].GetComponent<MainGrid>().valueTotal += difAltura + difLargura + 10;
				}
				else
				{
					sides[i].GetComponent<MainGrid>().valueTotal += difAltura + difLargura + 14;
				}
			}

			#endregion

			#region Geting the Lower

			List<GameObject> gos = new List<GameObject>();

			gos.Add(sides[0]);
			for (int i = 1; i < sides.Count; i++)
			{
				if (gos[0].GetComponent<MainGrid>().valueTotal > sides[i].GetComponent<MainGrid>().valueTotal)
				{
					gos.Clear();
					gos.Add(sides[i]);
				}
				else if (gos[0].GetComponent<MainGrid>().valueTotal == sides[i].GetComponent<MainGrid>().valueTotal)
				{
					gos.Add(sides[i]);
				}
			}

			#endregion

			#region Saving the Lower

			if (go == Main.pointA)
			{
				Main.TheWay.Add(new List<GameObject>());
				Main.TheWay[0].Add(go);
			}

			for (int i = 0; i < gos.Count; i++)
			{
				List<GameObject> gos2 = new List<GameObject>();
				if (gos2 != null)
				{
					for (int j = 0; j < Main.TheWay.Count; j++)
					{
						if (Main.TheWay[j][Main.TheWay[j].Count - 1] == go)
						{
							gos2 = Main.TheWay[j];
							j = Main.TheWay.Count;
						}
					}
				}
				Main.TheWay.Add(gos2);
				Main.TheWay[Main.TheWay.Count - 1].Add(gos[i]);
			}

			#endregion

			#region Forwarding

			go.GetComponent<MainGrid>().passed = true;
			if (go != Main.pointB)
			{
				for (int i = 0; i < Main.TheWay.Count; i++)
				{
					for (int j = 0; j < Main.TheWay[i].Count; j++)
					{
						Main.TheWay[i][j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Player");
						if (!Main.TheWay[i][j].GetComponent<MainGrid>().passed)
						{
							go = Main.TheWay[i][j];
							PathFind();
						}
					}
				}
			}

			#endregion

		}
		else
		{
			EndPathFinding();
		}
	}

	private void EndPathFinding()
	{
		List<int> ints = new List<int>();
		for (int i = 0; i < Main.TheWay.Count; i++)
		{
			ints.Add(0);
		}


		for (int i = 0; i < Main.TheWay.Count; i++)
		{
			for (int j = 0; j < Main.TheWay[i].Count; j++)
			{
				ints[i] += Main.TheWay[i][j].GetComponent<MainGrid>().valueTotal;
			}
		}
	}
}
