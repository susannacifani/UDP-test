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

        float Gx = float.Parse(points[points.Length - 3]); //Gx
        float Gy = float.Parse(points[points.Length - 2]); //Gy
        float Gz = float.Parse(points[points.Length - 1]); //Gz

        cube.transform.rotation = Quaternion.Euler(Gx, Gy, Gz);


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
