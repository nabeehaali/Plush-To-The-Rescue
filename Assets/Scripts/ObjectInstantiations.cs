using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstantiations : MonoBehaviour
{
    public GameObject[] animalList;
    public GameObject[] obstacleList;
    public float numObjectsStart;

    Vector2 left, right, up, down;
    float mainTriangleArea;
    bool inBottomTriangle;
    bool inTopTriangle;
    Vector2 randomPoint;

    Vector2 leftMini, rightMini, upMini, downMini;
    bool inBottomTriangleMini, inTopTriangleMini;

    bool isUpdate;
    SoundManager SoundManager;

    private void Start()
    {
        left = new Vector2(-3.29f, -4.59f);
        right = new Vector2(3.29f, -4.59f);
        up = new Vector2(0, -2.62f);
        down = new Vector2(0, -6.12f);

        leftMini = new Vector2(-3.72f, -4.54f);
        rightMini = new Vector2(-1.71f, -4.54f);
        upMini = new Vector2(-2.74f, -4f);
        downMini = new Vector2(-2.85f, -5.2f);

        //instantiate set number of animals and obstacles
        for (int i = 0; i < numObjectsStart; i++)
        {
            int index = Random.Range(0, animalList.Length);
            generatePosition();
            Instantiate(animalList[index], randomPoint, Quaternion.identity);

            int index2 = Random.Range(0, obstacleList.Length);
            generatePosition();
            Instantiate(obstacleList[index2], randomPoint, Quaternion.identity);
        }

        isUpdate = true;

        SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    //similar to claw movment, generates a point where the object can be instantiated on the ground
    void generatePosition()
    {
        randomPoint = new Vector2(Random.Range(left.x, right.x), Random.Range(down.y, up.y));
        checkInSpace();
    }

    void checkInSpace()
    {
        inBottomTriangle = isInside(left, right, down, randomPoint);
        inTopTriangle = isInside(left, right, up, randomPoint);

        inBottomTriangleMini = isInside(leftMini, rightMini, downMini, randomPoint);
        inTopTriangleMini = isInside(leftMini, rightMini, upMini, randomPoint);

        //take inner rectangle corrdinates and substitute in random range
        if(!inBottomTriangle && !inTopTriangle || inTopTriangleMini || inBottomTriangleMini)
        {
            randomPoint = new Vector2(Random.Range(-2, 2f), Random.Range(-3.57f, -5.27f));
        }

    }
    bool isInside(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 point)
    {
        float Area = areaTriangle(pointA, pointB, pointC);
        float Area1 = areaTriangle(point, pointA, pointB);
        float Area2 = areaTriangle(point, pointB, pointC);
        float Area3 = areaTriangle(point, pointC, pointA);
        float Sum = Area1 + Area2 + Area3;

        if (Area == Sum)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    float areaTriangle(Vector2 pointA, Vector2 pointB, Vector2 pointC)
    {
        mainTriangleArea = Mathf.Abs((pointA.x * (pointB.y - pointC.y) + pointB.x * (pointC.y - pointA.y) + pointC.x * (pointA.y - pointB.y)) / 2.0f);
        return mainTriangleArea;
    }

    private void Update()
    {
        //continue instantiating objects where there's less than the starting amount
        if (GameObject.FindGameObjectsWithTag("Animal").Length < numObjectsStart)
        {
            if (isUpdate)
            {
                isUpdate = false;
                StartCoroutine(createAnimal());
            }
        }

        if (GameObject.FindGameObjectsWithTag("Obstacle").Length < numObjectsStart)
        {
            if (isUpdate)
            {
                isUpdate = false;
                StartCoroutine(createObstacle());
            }
        }
    }

    //instantate random animal from the given list
    IEnumerator createAnimal()
    {
        yield return new WaitForSeconds(2f);
        generatePosition();
        int index = Random.Range(0, animalList.Length);
        Instantiate(animalList[index], randomPoint, Quaternion.identity);
        isUpdate = true;
        SoundManager.PlaySounds("NewStuffy");
    }

    //instantate random obstacle from the given list
    public IEnumerator createObstacle()
    {
        yield return new WaitForSeconds(2f);
        generatePosition();
        int index = Random.Range(0, obstacleList.Length);
        Instantiate(obstacleList[index], randomPoint, Quaternion.identity);
        isUpdate = true;
        SoundManager.PlaySounds("NewObstacle");
    }

    //creates an obstacle (for other function)
    public void makeObstacle()
    {
        StartCoroutine(createObstacle());
    }
}

