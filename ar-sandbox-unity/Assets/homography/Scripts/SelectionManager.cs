/*
Copyright (C) 2012 Chirag Raman

This file is part of Projection-Mapping-in-Unity3D.

Projection-Mapping-in-Unity3D is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Projection Mapping in Unity3D is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Projection-Mapping-in-Unity3D.  If not, see <http://www.gnu.org/licenses/>.
*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour {
	
	GameObject dynamicControllersParent;
	GameObject staticControllersParent;
	public List<GameObject> staticControllers;
	public List<GameObject> dynamicControllers;
	public int activeIndex;
	private int lastActiveIndex;
	public bool indexChanged;
	private bool dynamicControllerVisibility;
	public bool startHomography = true;
	
	// Use this for initialization
	void Start () {
	
		dynamicControllersParent = GameObject.Find("DynamicControllers");
		staticControllersParent = GameObject.Find("StaticControllers");
		
		//Define Lists
		staticControllers = new List<GameObject>();
		dynamicControllers = new List<GameObject>();
		
		//intiate activeIndex
		activeIndex = 0;
		lastActiveIndex = 0;
		indexChanged = false;
		
		//Dynamic Controller Visibility
		dynamicControllerVisibility = true;
		
		startHomography = true;
		
		
		//Turn off visibility of static controllers
		foreach(Transform child in staticControllersParent.transform){
			staticControllers.Add(child.gameObject);
			child.GetComponent<Renderer>().enabled = false;
		}	
		
		//Change the colour of the dynamic controllers and add the DynamicController Script as a Component
		foreach(Transform child in dynamicControllersParent.transform){
			dynamicControllers.Add (child.gameObject);
			child.gameObject.AddComponent<DynamicController>();
			child.GetComponent<Renderer>().material.color = Color.red;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(indexChanged == true){
			
			//Change colour of active controller to green
			indexChanged = false;
			dynamicControllers[lastActiveIndex].GetComponent<Renderer>().material.color = Color.red;
			lastActiveIndex = activeIndex;
			dynamicControllers[activeIndex].GetComponent<Renderer>().material.color = Color.green;
						
		}
		
		//Toggle dynamic controllers visibility
		
		if(Input.GetButtonUp("ToggleController")){
			if(dynamicControllerVisibility == true){
				dynamicControllerVisibility = false;
				foreach(Transform child in dynamicControllersParent.transform){
					child.GetComponent<Renderer>().enabled = false;
				}
			}
			else {
				dynamicControllerVisibility = true;
				foreach(Transform child in dynamicControllersParent.transform){
					child.GetComponent<Renderer>().enabled = true;
				}
				
			}
		}		
	}
	
}
