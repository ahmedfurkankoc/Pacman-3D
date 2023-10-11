using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanController : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 currentDirection = Vector3.right; // Baþlangýçta saða yönlü hareket
    private Vector3 nextDirection;

    private void Update()
    {
        HandleInput(); // Tuþ giriþlerini iþleme
        Move(); // Pacman'ýn hareketini saðlama
        LookAtDirection(); // Pacman'ýn dönme yönlü bakmasýný saðlama
        ClampPosition(); // Pozisyonu kýsýtlama (duvarlardan içeri geçmemesi)
    }

    private void HandleInput()
    {
        // Tuþ giriþlerini al
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Pacman'ýn yönünü güncelle
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
        // Hedef pozisyonun hesaplanmasý
        Vector3 targetPosition = transform.position + currentDirection * speed * Time.deltaTime;

        // Pacman'ýn hareketinin engellendiði durumun kontrolü
        RaycastHit hit;
        Vector3 sphereCenter = transform.position + Vector3.up * GetComponent<SphereCollider>().radius;
        if (!Physics.Raycast(sphereCenter, currentDirection, out hit, speed * Time.deltaTime) || !hit.collider.CompareTag("Wall"))
        {
            // Eðer duvarla çarpýþma yoksa, yönü güncelle
            currentDirection = nextDirection;
        }

        // Pacman'ýn yeni pozisyonunu ayarlama
        transform.position = targetPosition;
    }

    private void LookAtDirection()
    {
        // Pacman'ýn belirlenen yönüne dönmesi
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
            // Puan veya diðer iþlemleri burada güncelleyebilirsiniz
        }
    }

    private void ClampPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -9f, 9f); // Örnek sýnýrlar
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -9f, 9f); // Örnek sýnýrlar
        transform.position = clampedPosition;
    }
}
