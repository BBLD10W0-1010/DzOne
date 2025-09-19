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
    //кэшим параметры и сравниваем их изменение чтобы не пересоздавать префаб каждый апдейт
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
        var normalDistrib = 360f / cubeCount;
        
        for (int i = 0; i < cubeCount; i++)
        {
            var angle = i * normalDistrib;
            CreateCubeAtAngle(angle, i);
        }
    }

    void CreateSequentialCubes()
    {
        for (int i = 0; i < cubeCount; i++)
        {
            var angle = i * spacing;
            CreateCubeAtAngle(angle, i);
        }
    }

    void CreateCubeAtAngle(float angle, int index)
    {
        var position = CalculatePosition(angle);
        var cube = Instantiate(cubePrefab, position, Quaternion.identity, transform);
        cubes.Add(cube);
        initialAngles.Add(angle);
        cube.name = $"Cube_{index}";
    }

    Vector3 CalculatePosition(float angle)
    {
        var rad = angle * Mathf.Deg2Rad;
        var x = Mathf.Cos(rad) * radius;
        var z = Mathf.Sin(rad) * radius;
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
    //проверка изменения параметров
    void CheckForParameterChanges()
    {
        if (cubeCount != lastCubeCount || radius != lastRadius || uniformDistribution != lastUniformDistribution || Mathf.Abs(spacing - lastSpacing) > 0.001f)
        {
            parametersChanged = true;
        }
    }
    //метод который вращает кубы - вызывается в апдейте 
    void RotateCubes()
    {
        //можно енум; посмотреть как у других на паре; я <3 тернарные операции 
        var direction = clockwise ? 1f : -1f;
        var currentRotation = Time.time * rotationSpeed * direction;
        //цикл по созданным кубам - двигаем каждый кубик в цикле
        for (int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i] != null)
            {
                var finalAngle = initialAngles[i] + currentRotation;
                var newPosition = CalculatePosition(finalAngle);
                cubes[i].transform.position = newPosition;
                
                cubes[i].transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * direction);
            }
        }
    }

    //сетеры
    
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
    //вызывается при изменении радиуса
    void UpdateCubePositions()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i] != null)
            {
                var position = CalculatePosition(initialAngles[i]);
                cubes[i].transform.position = position;
            }
        }
    }
    
    // void UpdatePositionsOnly()
    // {
    //     for (int i = 0; i < cubes.Count; i++)
    //     {
    //         if (cubes[i] != null)
    //         {
    //             Vector3 position = CalculatePosition(initialAngles[i]);
    //             cubes[i].transform.position = position;
    //         }
    //     }
    //     parametersChanged = false;
    //     CacheParameters();
    // } не нужон!!!!
}
