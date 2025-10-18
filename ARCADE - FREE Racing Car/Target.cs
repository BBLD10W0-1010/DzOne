using UnityEngine;

public class Target : MonoBehaviour
{
    public int scoreValue = 10; // Очки за уничтожение
    
    void Start()
    {
        // Автоматически добавляем Rigidbody и коллайдер если их нет
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false; // Физика включена
        }
        
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }
}