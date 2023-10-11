using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanController : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 currentDirection = Vector3.right; // Ba�lang��ta sa�a y�nl� hareket
    private Vector3 nextDirection;

    private void Update()
    {
        HandleInput(); // Tu� giri�lerini i�leme
        Move(); // Pacman'�n hareketini sa�lama
        LookAtDirection(); // Pacman'�n d�nme y�nl� bakmas�n� sa�lama
        ClampPosition(); // Pozisyonu k�s�tlama (duvarlardan i�eri ge�memesi)
    }

    private void HandleInput()
    {
        // Tu� giri�lerini al
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Pacman'�n y�n�n� g�ncelle
        if (horizontalInput > 0)
        {
            nextDirection = Vector3.right;
        }
        else if (horizontalInput < 0)
        {
            nextDirection = Vector3.left;
        }
        else if (verticalInput > 0)
        {
            nextDirection = Vector3.forward;
        }
        else if (verticalInput < 0)
        {
            nextDirection = Vector3.back;
        }
    }

    private void Move()
    {
        // Hedef pozisyonun hesaplanmas�
        Vector3 targetPosition = transform.position + currentDirection * speed * Time.deltaTime;

        // Pacman'�n hareketinin engellendi�i durumun kontrol�
        RaycastHit hit;
        Vector3 sphereCenter = transform.position + Vector3.up * GetComponent<SphereCollider>().radius;
        if (!Physics.Raycast(sphereCenter, currentDirection, out hit, speed * Time.deltaTime) || !hit.collider.CompareTag("Wall"))
        {
            // E�er duvarla �arp��ma yoksa, y�n� g�ncelle
            currentDirection = nextDirection;
        }

        // Pacman'�n yeni pozisyonunu ayarlama
        transform.position = targetPosition;
    }

    private void LookAtDirection()
    {
        // Pacman'�n belirlenen y�n�ne d�nmesi
        if (currentDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dot"))
        {
            Destroy(other.gameObject);
            // Puan veya di�er i�lemleri burada g�ncelleyebilirsiniz
        }
    }

    private void ClampPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -9f, 9f); // �rnek s�n�rlar
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -9f, 9f); // �rnek s�n�rlar
        transform.position = clampedPosition;
    }
}
