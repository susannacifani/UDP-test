using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HandTracking : MonoBehaviour
{
    public UDPReceiver udpReceiver;
    public GameObject cube;

    // Fattore di scala per il giroscopio (ipotizzando ±250°/s)
    private float gyroscopeScaleFactor = 0.00763f;

    // Variabili di accumulo per la rotazione (in gradi)
    private float accumulatedRotationX = 0f;
    private float accumulatedRotationY = 0f;
    private float accumulatedRotationZ = 0f;

    void Update()
    {
        string data = udpReceiver.data;

        // Verifica se i dati non sono nulli o vuoti
        if (string.IsNullOrEmpty(data))
            return;

        // Rimuovere i caratteri iniziali e finali (assumendo che siano parentesi o altri caratteri)
        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);

        // Dividere i dati in base alla virgola
        string[] points = data.Split(',');

        // Assicurarsi che ci siano abbastanza dati per leggere il giroscopio
        if (points.Length < 3)
            return;

        // Leggere i valori grezzi del giroscopio (Gx, Gy, Gz)
        float Gx = float.Parse(points[points.Length - 3]);
        float Gy = float.Parse(points[points.Length - 2]);
        float Gz = float.Parse(points[points.Length - 1]);

        // Convertire i valori del giroscopio in gradi per secondo
        float gyroX = Gx * gyroscopeScaleFactor;
        float gyroY = -Gy * gyroscopeScaleFactor;
        float gyroZ = Gz * gyroscopeScaleFactor;

        // Integrare i valori del giroscopio per ottenere l'angolo di rotazione cumulativo (in gradi)
        accumulatedRotationX = (accumulatedRotationX + gyroX * Time.deltaTime) % 360f;
        accumulatedRotationY = (accumulatedRotationY + gyroY * Time.deltaTime) % 360f;
        accumulatedRotationZ = (accumulatedRotationZ + gyroZ * Time.deltaTime) % 360f;

        // Creare la rotazione target usando i valori integrati
        Quaternion targetRotation = Quaternion.Euler(accumulatedRotationX, accumulatedRotationY, accumulatedRotationZ);

        // Applicare la rotazione al cubo
        cube.transform.rotation = targetRotation;
    }
}
