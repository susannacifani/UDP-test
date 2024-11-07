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

    // Fattore di smoothing per Lerp
    float lerpFactor = 0.001f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string data = udpReceiver.data;

        if (string.IsNullOrEmpty(data))
            return;

        // Rimuovere i caratteri iniziali e finali
        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);

        // Dividere i dati in base alla virgola
        string[] points = data.Split(',');

        // Leggere i valori di rotazione dal giroscopio
        float Gx = float.Parse(points[points.Length - 3]); // Gx
        float Gy = float.Parse(points[points.Length - 2]); // Gy
        float Gz = float.Parse(points[points.Length - 1]); // Gz

        // Creare la rotazione target
        Quaternion targetRotation = Quaternion.Euler(Gx, Gy, Gz);

        // Interpolare tra la rotazione attuale e quella target
        cube.transform.rotation = Quaternion.Lerp(cube.transform.rotation, targetRotation, lerpFactor);
    }
}
