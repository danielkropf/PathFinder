using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainGrid : MonoBehaviour {

    public GameObject Right;
    public GameObject Left;
    public GameObject Up;
    public GameObject Down;
    public int way = -1;

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
						valueTotal = 10000000;
					}
					else if (this.gameObject.GetComponent<MainGrid>().function == "Wall")
					{
						this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Water");
						function = "Water";
						valueTotal = 10;
					}
					else if (this.gameObject.GetComponent<MainGrid>().function == "Water")
					{
						this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Empty");
						function = "Empty";
						valueTotal = 0;
					}
				}
				break;
		}
	}

	void Update()
	{
		if (Main.phase == 3) if (Input.GetKeyUp (KeyCode.KeypadEnter) || Input.GetKeyUp (KeyCode.Return)) Main.phase = 4;
	}
}
