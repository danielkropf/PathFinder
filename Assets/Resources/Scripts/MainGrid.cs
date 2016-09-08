using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainGrid : MonoBehaviour {

    public GameObject Right;
    public GameObject Left;
    public GameObject Up;
    public GameObject Down;

	public int altura;
	public int largura;

	public int valueTotal;
	public string function = "Empty";
	public bool myTurn = false;
	public bool passed = false;

	private List<GameObject> next = new List<GameObject>();

	void Start()
	{
		altura = int.Parse(transform.parent.name);
		largura = int.Parse(this.name);
	}

	void OnMouseUp()
	{
		switch (Main.phase)
		{
			case 1:

				this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/APoint");
				Main.pointA = this.gameObject;
				Main.phase++;
				function = "A";
				break;

			case 2:

				if (Main.pointA != this.gameObject)
				{
					this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/BPoint");
					Main.pointB = this.gameObject;
					Main.phase++;
					function = "B";
				}
				break;
			
			case 3:

				if(Main.pointA != this.gameObject && Main.pointB != this.gameObject)
				{
					if (this.gameObject.GetComponent<MainGrid>().function == "Empty")
					{
						this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Wall");
						function = "Wall";
						Debug.Log("Wall");
					}
					else if (this.gameObject.GetComponent<MainGrid>().function == "Wall")
					{
						this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Water");
						function = "Water";
						Debug.Log("Water");
					}
					else if (this.gameObject.GetComponent<MainGrid>().function == "Water")
					{
						this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Empty");
						function = "Empty";
						Debug.Log("Empty");
					}
				}
				break;

		}
	}

	void PathFinding()
	{
		List<GameObject> go = new List<GameObject>();

		#region Testing Side Places
		
		if (Right != null)
		{
			go.Add(Right);
			if (Right.GetComponent<MainGrid>().Down != null) go.Add(Right.GetComponent<MainGrid>().Down);
		}
		if (Down != null)
		{
			go.Add(Down);
			if (Down.GetComponent<MainGrid>().Left != null) go.Add(Down.GetComponent<MainGrid>().Left);
		}
		if (Left != null)
		{
			go.Add(Left);
			if (Left.GetComponent<MainGrid>().Up != null) go.Add(Left.GetComponent<MainGrid>().Up);
		}
		if (Up != null)
		{
			go.Add(Up);
			if (Up.GetComponent<MainGrid>().Right != null) go.Add(Up.GetComponent<MainGrid>().Right);
		}

		for (int i = 0; i < go.Count; i++)
		{
			int altura2 = go[i].GetComponent<MainGrid>().altura - Main.pointB.GetComponent<MainGrid>().altura;

			if (altura2 < 0) altura2 = altura2 * -10;
			else altura2 = altura2 * 10;

			int largura2 = go[i].GetComponent<MainGrid>().largura - Main.pointB.GetComponent<MainGrid>().largura;

			if (largura2 < 0) largura2 = largura2 * -10;
			else largura2 = largura2 * 10;

			if(go[i] == Up || go[i] == Left || go[i] == Down || go[i] == Right) go[i].GetComponent<MainGrid>().valueTotal = 10 + largura2 + altura2;
			else go[i].GetComponent<MainGrid>().valueTotal = 14 + largura2 + altura2;

			Debug.Log(altura2 + " + " + largura2 + " -> " + i);

			if (next != null) {
				for (int j = 0; j < next.Count; j++)
				{
					if (next[j].GetComponent<MainGrid>().valueTotal > go[i].GetComponent<MainGrid>().valueTotal)
					{
						next.Remove(next[j]);
						next.Add(go[i]);
					}
				}
			}
			else {
				next.Add(go[i]);
			}
		}
		#endregion

		#region Ending
		Main.TheWay.Add(next);
		//next.GetComponent<MainGrid>().myTurn = true;
		passed = true;
		myTurn = false;
		this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Player");
		#endregion
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
		{
			Main.phase = 4;
			if (!Main.pointA.GetComponent<MainGrid>().passed) Main.pointA.GetComponent<MainGrid>().myTurn = true;
			for (int i = 0; i < next.Count; i++)
			{
				Debug.Log("..");
				if (!myTurn && passed && !next[i].GetComponent<MainGrid>().passed)
				{
					next[i].GetComponent<MainGrid>().myTurn = true;
				}
			}
		}

		if (myTurn) PathFinding();
	}
}
