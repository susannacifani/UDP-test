using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingAccelerometer : MonoBehaviour
{
    public UDPReceiver udpReceiver;
    public GameObject cube;

    private float accelerometerScaleFactor = 0.0005f; // Ridotto per un movimento più controllato
    private Vector3 velocity = Vector3.zero;
    private float friction = 0.98f; // attrito per rallentare il movimento
    private float maxSpeed = 3.0f; // Ridotto limite di velocità massima per evitare movimenti troppo rapidi

    void Update()
    {
        string data = udpReceiver.data;

        if (string.IsNullOrEmpty(data))
            return;

        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);

        string[] points = data.Split(',');

        if (points.Length < 8)
            return;

        // Prendi i valori dell'accelerometro
        float Ax = float.Parse(points[points.Length - 8]);
        float Ay = float.Parse(points[points.Length - 7]);
        float Az = float.Parse(points[points.Length - 6]);

        float accelX = Ax * accelerometerScaleFactor;
        float accelY = Ay * accelerometerScaleFactor;
        float accelZ = Az * accelerometerScaleFactor;

        // Applicare una soglia per ignorare piccoli rumori
        float threshold = 0.02f; // Soglia per filtrare rumori minimi
        if (Mathf.Abs(accelX) < threshold) accelX = 0;
        if (Mathf.Abs(accelY) < threshold) accelY = 0;
        if (Mathf.Abs(accelZ) < threshold) accelZ = 0;

        Vector3 acceleration = new Vector3(accelX, accelY, accelZ);

        // Aggiungi un boost alla velocità
        float speedBoost = 1.5f; // Ridotto boost per evitare accelerazione troppo veloce
        velocity += acceleration * Time.deltaTime * speedBoost;

        // Applicare l'attrito
        velocity *= friction;

        // Limitare la velocità massima
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Spostare il cubo usando la velocità calcolata
        cube.transform.Translate(velocity * Time.deltaTime);
    }
}
