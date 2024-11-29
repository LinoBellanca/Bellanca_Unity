using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // R�f�rence au personnage
    public Vector3 offset; // D�calage de position de la cam�ra
    public float smoothSpeed = 0.125f; // Vitesse de lissage de la position

    void Start()
    {
        // Initialise l'offset bas� sur la position actuelle de la cam�ra et du joueur
        offset = transform.position - playerTransform.position;
    }

    void LateUpdate()
    {
        // Calcule la nouvelle position de la cam�ra
        Vector3 desiredPosition = playerTransform.position + offset;

        // Lisse le mouvement de la cam�ra pour �viter les saccades
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Optionnel : Ajuste la rotation de la cam�ra si n�cessaire
        transform.LookAt(playerTransform.position);
    }
}