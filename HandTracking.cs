using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingAccelerometer : MonoBehaviour
{
    public UDPReceiver udpReceiver;
    public GameObject cube;

    private float accelerometerScaleFactor = 0.00005f;
    private Vector3 velocity = Vector3.zero;
    private float friction = 0.98f; // Fattore di attrito per rallentare la velocità

    void Update()
    {
        string data = udpReceiver.data;

        if (string.IsNullOrEmpty(data))
            return;

        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);

        string[] points = data.Split(',');

        if (points.Length < 3)
            return;

        float Ax = float.Parse(points[points.Length - 8]);
        float Ay = float.Parse(points[points.Length - 7]);
        float Az = float.Parse(points[points.Length - 6]);

        float accelX = Ax * accelerometerScaleFactor;
        float accelY = Ay * accelerometerScaleFactor;
        float accelZ = Az * accelerometerScaleFactor;

        // Applicare una soglia per ignorare piccoli rumori
        float threshold = 0.05f;
        if (Mathf.Abs(accelX) < threshold) accelX = 0;
        if (Mathf.Abs(accelY) < threshold) accelY = 0;
        if (Mathf.Abs(accelZ) < threshold) accelZ = 0;

        Vector3 acceleration = new Vector3(accelX, accelY, accelZ);

        // Integrare l'accelerazione per ottenere la velocità
        velocity += acceleration * Time.deltaTime;

        // Applicare l'attrito
        velocity *= friction;

        // Limitare la velocità massima
        float maxSpeed = 1.0f;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Spostare il cubo usando la velocità calcolata
        cube.transform.Translate(velocity * Time.deltaTime);
    }
}
