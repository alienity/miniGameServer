using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;


public class TestJson : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            string joStr = TestJson1();
            Debug.Log(joStr);

            var jo = JObject.Parse(joStr);
            string name = jo["name"].Value<string>();
            Debug.Log(name);
            float age = jo["Age"].Value<float>();
            Debug.Log(age);
        }
	}

    public string TestJson1()
    {
        var jo = new JObject();
        jo.Add("name", "xiangzi");
        jo.Add("Age", 3.6f);

        return jo.ToString();
    }
}
