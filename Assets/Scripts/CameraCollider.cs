using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    public GameManager gameManager;
    private void OnTriggerEnter(Collider cube) {
        gameManager.endGame();
    }
}
