using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    //Ce composant est à placer sur un GameObject qui servira d'Ancre de la Caméra, ce GameObject ne doit PAS être enfant du Player

    [SerializeField] Rigidbody2D playerBody;

    // Update is called once per frame
    void Update()
    {
        Vector2 desiredPosition = playerBody.position + playerBody.velocity;

        //On limite la distance maximale de l'ancre pour quelle ne s'éloigne pas trop du joueur
        desiredPosition.y = Mathf.Clamp( desiredPosition.y, playerBody.position.y - 1.5f, playerBody.position.y + 3f );
        //On fait la même chose sur l'axe X
        desiredPosition.x = Mathf.Clamp( desiredPosition.x, playerBody.position.x - 5f, playerBody.position.x + 5f );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Mathf.SmoothStep(0f,1f,0.1f) );
    }
}
