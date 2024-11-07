using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HandTracking : MonoBehaviour
{
    // Start is called before the first frame update
    public UDPReceiver udpReceiver;
    public GameObject cube;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string data = udpReceiver.data;

        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);
        //print(data);
        string[] points = data.Split(',');
        //print(points[0]);


        //if (data.GetType() == typeof(string))
            //print("same");
        //print(data);
        //stampa da points[6]
        //print(points[11]); //ogni 5 spazi c'è un numero
        /*
            for (int i = 0; i < 40; i++)
        {
            print(points[i]);
        }
        */




        /*

        //0        1*3      2*3
        //x1,y1,z1,x2,y2,z2,x3,y3,z3

        for (int i = 0; i < 21; i++)
        {

            float x = 7 - float.Parse(points[i * 3]) / 100;
            float y = float.Parse(points[i * 3 + 1]) / 100;
            float z = float.Parse(points[i * 3 + 2]) / 100;

            cube.transform.localPosition = new Vector3(x, y, z);

        }
        */


    }
}
