using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // Référence au personnage
    public Vector3 offset; // Décalage de position de la caméra
    public float smoothSpeed = 0.125f; // Vitesse de lissage de la position

    void Start()
    {
        // Initialise l'offset basé sur la position actuelle de la caméra et du joueur
        offset = transform.position - playerTransform.position;
    }

    void LateUpdate()
    {
        // Calcule la nouvelle position de la caméra
        Vector3 desiredPosition = playerTransform.position + offset;

        // Lisse le mouvement de la caméra pour éviter les saccades
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Optionnel : Ajuste la rotation de la caméra si nécessaire
        transform.LookAt(playerTransform.position);
    }
}