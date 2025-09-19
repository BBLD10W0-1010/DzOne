using System.Collections.Generic;
using UnityEngine;

public class MerryGoRound1 : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private int cubeCount = 8;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private bool clockwise = true;
    
    [Header("Распределение")]
    [SerializeField] private bool uniformDistribution = true;
    [SerializeField] private float spacing = 1f;
    
    private List<GameObject> cubes = new List<GameObject>();
    private List<float> initialAngles = new List<float>();
    private bool parametersChanged = false;
    
    private int lastCubeCount;
    private float lastRadius;
    private bool lastUniformDistribution;
    private float lastSpacing;

    void Awake()
    {
        CacheParameters();
        CreateCubes();
    }

    void CacheParameters()
    {
        lastCubeCount = cubeCount;
        lastRadius = radius;
        lastUniformDistribution = uniformDistribution;
        lastSpacing = spacing;
    }

    void CreateCubes()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("Префаб не назначен!");
            return;
        }
        foreach (var cube in cubes)
        {
            if (cube != null)
                Destroy(cube);
        }
        cubes.Clear();
        initialAngles.Clear();

        if (uniformDistribution)
        {
            CreateUniformCubes();
        }
        else
        {
            CreateSequentialCubes();
        }

        parametersChanged = false;
        CacheParameters();
    }

    
    void CreateUniformCubes()
    {
        float angleStep = 360f / cubeCount;
        
        for (int i = 0; i < cubeCount; i++)
        {
            float angle = i * angleStep;
            CreateCubeAtAngle(angle, i);
        }
    }

    void CreateSequentialCubes()
    {
        for (int i = 0; i < cubeCount; i++)
        {
            float angle = i * spacing;
            CreateCubeAtAngle(angle, i);
        }
    }

    void CreateCubeAtAngle(float angle, int index)
    {
        Vector3 position = CalculatePosition(angle);
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity, transform);
        
        cubes.Add(cube);
        initialAngles.Add(angle);
        
        cube.name = $"Cube_{index}";
    }

    Vector3 CalculatePosition(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad) * radius;
        float z = Mathf.Sin(rad) * radius;
        return new Vector3(x, 0, z);
    }

    void Update()
    {
        CheckForParameterChanges();
        
        if (parametersChanged)
        {
            CreateCubes();
        }
        
        RotateCubes();
    }

    void CheckForParameterChanges()
    {
        if (cubeCount != lastCubeCount || 
            radius != lastRadius || 
            uniformDistribution != lastUniformDistribution || 
            Mathf.Abs(spacing - lastSpacing) > 0.001f)
        {
            parametersChanged = true;
        }
    }

    void RotateCubes()
    {
        float direction = clockwise ? 1f : -1f;
        float currentRotation = Time.time * rotationSpeed * direction;
        
        for (int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i] != null)
            {
                float finalAngle = initialAngles[i] + currentRotation;
                Vector3 newPosition = CalculatePosition(finalAngle);
                cubes[i].transform.position = newPosition;
                
                cubes[i].transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * direction);
            }
        }
    }
    
    public void SetRadius(float newRadius)
    {
        radius = newRadius;
        UpdateCubePositions();
    }

    public void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }

    public void SetDirection(bool isClockwise)
    {
        clockwise = isClockwise;
    }

    public void SetCubeCount(int count)
    {
        cubeCount = Mathf.Max(1, count);
        CreateCubes();
    }

    public void SetDistributionType(bool isUniform)
    {
        uniformDistribution = isUniform;
        CreateCubes();
    }

    void UpdateCubePositions()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i] != null)
            {
                Vector3 position = CalculatePosition(initialAngles[i]);
                cubes[i].transform.position = position;
            }
        }
    }
    
    void UpdatePositionsOnly()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i] != null)
            {
                Vector3 position = CalculatePosition(initialAngles[i]);
                cubes[i].transform.position = position;
            }
        }
        parametersChanged = false;
        CacheParameters();
    }
}
