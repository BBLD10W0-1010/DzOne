// DiceView.cs
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
class DiceView : MonoBehaviour
{
    private Rigidbody rb;

    public event Action<int> OnValueDetermined;


    private readonly Vector3[] directions =
    {
        Vector3.forward,   // 1
        Vector3.down,    // 2
        Vector3.left, // 3
        Vector3.right,    // 4
        Vector3.up,      // 5
        Vector3.back    // 6  Vector3.back
    };

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ThrowDice()
    {
        Debug.Log("ThrowDice called");
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 randomDir = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            1,
            UnityEngine.Random.Range(-1f, 1f)
        ).normalized;

        var  force = UnityEngine.Random.Range(5f, 12f);
        var  torque = UnityEngine.Random.Range(5f, 15f);

        rb.AddForce(randomDir * force, ForceMode.Impulse);
        rb.AddTorque(UnityEngine.Random.insideUnitSphere * torque, ForceMode.Impulse);

        CancelInvoke(nameof(CheckStopped));
        InvokeRepeating(nameof(CheckStopped), 2f, 0.5f);
    }

    private void CheckStopped()
    {
        Debug.Log($"CheckStopped: v={rb.linearVelocity.magnitude}, av={rb.angularVelocity.magnitude}");

        if (rb.linearVelocity.magnitude < 0.05f && rb.angularVelocity.magnitude < 0.05f)
        {
            CancelInvoke(nameof(CheckStopped));
            var  value = GetTopFaceValue();
            OnValueDetermined?.Invoke(value);
        }
    }

    private int GetTopFaceValue()
    {
        float maxDot = -1f;
        int bestSide = 1;

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 worldDir = transform.TransformDirection(directions[i]);
            var dot = Vector3.Dot(worldDir, Vector3.up);

            if (dot > maxDot)
            {
                maxDot = dot;
                bestSide = i + 1; 
            }
        }

        return bestSide;
    }
}
