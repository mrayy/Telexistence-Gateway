using UnityEngine;
using System.Collections;
//using UnityEngine.UI.Extensions;

public class MainMenuManager : MonoBehaviour {

//    public DropDownList robotList;

	// Use this for initialization
	public void ChangeToScene (int sceneToChangeTo) {

        Application.LoadLevel(sceneToChangeTo);
	}

    public void Start()
    {/*
        Debug.Log("Adding Items");
        robotList.Items.Clear();
        robotList.Items.Add(new DropDownListItem("Tachilab TELUBEE"));
        robotList.Items.Add(new DropDownListItem("KMD"));
        robotList.Items.Add(new DropDownListItem("Robomech"));
        robotList.RebuildPanel();*/

    }
}
