using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Nabeeha Ali: This is similar to the regular ClawMovement script, except it takes away the option to grab the player upon collision, made specifically for the game's interactive tutorial.
public class ClawMovementTutorial : MonoBehaviour
{
    //Info about vector from the roof
    Vector2 left, right, up, down;
    float mainTriangleArea;
    bool inBottomTriangle;
    bool inTopTriangle;
    Vector2 randomPoint;
    Vector2 currentPoint;

    //Info about relative vector on ground
    Vector2 relativePoint;
    Vector2 lineBtwnPoints;
    public GameObject player;

    //Determining probability of claw targetting player
    int probability;

    //Info about the moving the base of the claw 
    public GameObject ropeBottom;
    Vector2 newRopePosition;
    Vector2 ropeCurrPosition;
    public float clawSpeed;
    public float speedChangeRate;
    public float maxSpeed;

    //Claw shadow
    public GameObject clawShadow;
    float currShadowAlpha;
    float newShadowAlpha;

    //Info about generating point for the claw to move to
    private IEnumerator pointCoroutine;
    bool goDown = false;
    bool goUp = false;
    bool resumeRoutine = false;

    //Claw animation gameobjects
    public GameObject leftO, rightO;

    //Getting state of what the claw grabs to determine what to do with it
    public bool caughtStuffy = false;
    public bool destroyStuffy = false;
    public bool gameover = false;

    SoundManager SoundManager;

    private void Start()
    {
        //Assigning probability of type of point being generated and shadow opacity
        probability = 10;
        newShadowAlpha = 0.9f;

        //Point that is on the bottom part of the ground (marked based on position of player)
        relativePoint = new Vector2(0, -6.6f); ;

        //These are the values that make up the verticies of the roof space (done manually sadly)
        left = new Vector2(-3.32f, 2.35f);
        right = new Vector2(3.32f, 2.35f);
        up = new Vector2(0, 3.64f);
        down = new Vector2(0, 0);

        //Generate a point
        pointCoroutine = generatePoint();
        StartCoroutine(pointCoroutine);

        SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    IEnumerator generatePoint()
    {
        while (true)
        {
            //Claw is not moving
            goDown = false;
            goUp = false;
            yield return new WaitForSeconds(5);

            //Assign a random force value to the claw's hingejoints
            int force = Random.Range(0, 100);
            leftO.GetComponent<HingeJoint2D>().breakForce = force;
            rightO.GetComponent<HingeJoint2D>().breakForce = force;

            //Max out the probability to 90% for targetting player position
            probability += 10;
            int i = Random.Range(0, 100);

            if (probability > 90)
            {
                probability = 90;
            }

            //Determines which type of point to create based on probability value
            if (i >= 0 && i <= probability)
            {
                createTargetedPoint();
                //Debug.Log("Target is based on player and is located at:" + randomPoint);
                clawSpeed += speedChangeRate;

                if (clawSpeed > maxSpeed)
                {
                    clawSpeed = maxSpeed;
                }
            }
            else if (i >= (probability + 1) && i <= 100)
            {
                generatePosition();
                //Debug.Log("Target is random and is located at:" + randomPoint);
                clawSpeed += speedChangeRate;
                if (clawSpeed > maxSpeed)
                {
                    clawSpeed = maxSpeed;
                }
            }
        }
    }

    //Takes the point generated from the roof and subtracts it by the downward vector (fixed value) to get player position
    void createTargetedPoint()
    {
        randomPoint = lineBtwnPoints - down - new Vector2(0, 0.3f);
    }

    //Generates a random point within the outer rectangle of the roof shape
    void generatePosition()
    {
        randomPoint = new Vector2(Random.Range(left.x, right.x), Random.Range(down.y, up.y));
        checkInSpace();
    }

    //Checking if the random point fits within the roof space by breaking the obj into triangles and checks if the point lies in both of them
    void checkInSpace()
    {
        inBottomTriangle = isInside(left, right, down, randomPoint);
        inTopTriangle = isInside(left, right, up, randomPoint);

        //Take inner rectangle corrdinates and substitute in random range if the point is not in the top or bottom triangle
        if (!inBottomTriangle && !inTopTriangle)
        {
            randomPoint = new Vector2(Random.Range(-1.84f, 1.84f), Random.Range(0.98f, 2.46f));
        }

    }

    //Checks if point lies within the upper/lower triangle of the roof shape
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

    //Gets area of the triangle to help determine if point lies within it
    float areaTriangle(Vector2 pointA, Vector2 pointB, Vector2 pointC)
    {
        mainTriangleArea = Mathf.Abs((pointA.x * (pointB.y - pointC.y) + pointB.x * (pointC.y - pointA.y) + pointC.x * (pointA.y - pointB.y)) / 2.0f);
        return mainTriangleArea;
    }

    void FixedUpdate()
    {
        //Creating vector based on relative point (bottom vertex of ground) and the player postion 
        lineBtwnPoints = new Vector2(player.transform.position.x - relativePoint.x, player.transform.position.y - relativePoint.y);

        //Move the claw from its current position to the point that was generated
        currentPoint = gameObject.transform.position;
        float step = clawSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, randomPoint, step);

        //Claw stops when it reaches the target point and moves down with rope
        if (currentPoint == randomPoint)
        {
            StopCoroutine(pointCoroutine);

            if (!goUp)
            {
                newRopePosition = new Vector2(0, -6.1f);
                destroyStuffy = false;
                goUp = true;
            }

            //Moving claw up/down depending on what the newRopePosition is
            ropeCurrPosition = ropeBottom.transform.localPosition;
            ropeBottom.transform.localPosition = Vector2.MoveTowards(ropeBottom.transform.localPosition, newRopePosition, step);

            //Changing opacity of shadow
            currShadowAlpha = clawShadow.GetComponent<SpriteRenderer>().color.a;
            float newAlpha = Mathf.Lerp(currShadowAlpha, newShadowAlpha, step);
            clawShadow.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, newAlpha);

            //Activating and deactivating colissions
            if (ropeBottom.transform.localPosition.y < -6f)
            {
                ignoreCollisions(false);
                ignorePlayerCollisions(false);
            }
            else if (ropeBottom.transform.localPosition.y <= -0.6f && ropeBottom.transform.localPosition.y >= -6f)
            {
                ignoreCollisions(true);
                //if player has not been caught and claw is going up
                if (!gameover)
                {
                    ignorePlayerCollisions(true);
                }
            }

            //If the rope is done moving downwards and it has caught a stuffed animal, it will move up and destroy the stuffy
            if (ropeCurrPosition == newRopePosition || caughtStuffy)
            {
                SoundManager.PlaySounds("ClawClose");
                SoundManager.NoLongClawSounds();

                if (!caughtStuffy)
                {
                    SoundManager.PlaySounds("ClawClick1");
                }

                //if the stuffy has been caught by the claw (links to stuffyLost and Player controls scripts to control what happens to the stuffies/player)
                if (caughtStuffy)
                {
                    SoundManager.PlaySounds("ClawCatchStuffy");
                    // Animate claw grabbing object
                    GameObject.Find("left_origin1").GetComponent<Animator>().Play("left_claw_grab");
                    GameObject.Find("right_origin1").GetComponent<Animator>().Play("right_claw_grab");
                    rightO.GetComponent<SpriteRenderer>().sortingOrder = -1;

                    newRopePosition = new Vector2(0, -0.5f);

                    if (gameover)
                    {
                        GameObject.Find("SceneBackground").GetComponent<Animator>().enabled = false;
                        GameObject.Find("Objective").GetComponent<GameObjective>().caughtPlayer();
                        //GameObject.Find("ColourManager").GetComponent<ColourManager>().changeCol();
                    }

                    //If stuffy has been caught and the rope has gone all the way up
                    if (ropeCurrPosition == newRopePosition)
                    {
                        destroyStuffy = true;

                        // Animate claw releasing object
                        GameObject.Find("left_origin1").GetComponent<Animator>().Play("left_claw_release");
                        GameObject.Find("right_origin1").GetComponent<Animator>().Play("right_claw_release");
                        rightO.GetComponent<SpriteRenderer>().sortingOrder = 1;

                        if (gameover)
                        {
                            //For Amy
                            //StartCoroutine(generatePoint());
                            //goDown = true;
                            //player.transform.position = new Vector2(-0.673f, -3.94f);
                        }

                    }
                }
                //Claw going up
                else if (!goDown)
                {
                    SoundManager.ClawSounds("ClawUp", 1);

                    leftO.SetActive(true);
                    rightO.SetActive(true);

                    newRopePosition = new Vector2(0, -0.5f);
                    goDown = true;

                    newShadowAlpha = 0.1f;
                }
                //Claw going down
                else if (goDown)
                {
                    SoundManager.ClawSounds("ClawDown", 1);

                    if (!resumeRoutine)
                    {
                        newShadowAlpha = 0.9f;

                        leftO.SetActive(true);
                        rightO.SetActive(true);

                        StartCoroutine(pointCoroutine);
                    }

                }

            }
        }

    }

    //Delay before moving to gameover screen
    IEnumerator endDelay()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(2);
    }

    //Ignores colissions between claw and stuffed animals (TODO: implement it for the player!)
    void ignoreCollisions(bool isOn)
    {
        GameObject[] animals;
        animals = GameObject.FindGameObjectsWithTag("Animal");

        for (int i = 0; i < animals.Length; i++)
        {
            Physics2D.IgnoreCollision(animals[i].GetComponent<PolygonCollider2D>(), leftO.GetComponent<EdgeCollider2D>(), isOn);
            Physics2D.IgnoreCollision(animals[i].GetComponent<PolygonCollider2D>(), rightO.GetComponent<EdgeCollider2D>(), isOn);

            Physics2D.IgnoreCollision(animals[i].GetComponent<CircleCollider2D>(), leftO.GetComponent<EdgeCollider2D>(), isOn);
            Physics2D.IgnoreCollision(animals[i].GetComponent<CircleCollider2D>(), rightO.GetComponent<EdgeCollider2D>(), isOn);
        }

        GameObject[] collectedAnimals;
        collectedAnimals = GameObject.FindGameObjectsWithTag("Collected");

        for (int i = 0; i < collectedAnimals.Length; i++)
        {
            Physics2D.IgnoreCollision(collectedAnimals[i].GetComponent<PolygonCollider2D>(), leftO.GetComponent<EdgeCollider2D>(), isOn);
            Physics2D.IgnoreCollision(collectedAnimals[i].GetComponent<PolygonCollider2D>(), rightO.GetComponent<EdgeCollider2D>(), isOn);

            Physics2D.IgnoreCollision(collectedAnimals[i].GetComponent<CircleCollider2D>(), leftO.GetComponent<EdgeCollider2D>(), isOn);
            Physics2D.IgnoreCollision(collectedAnimals[i].GetComponent<CircleCollider2D>(), rightO.GetComponent<EdgeCollider2D>(), isOn);
        }

        GameObject[] obstacles;
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        for (int i = 0; i < obstacles.Length; i++)
        {
            Physics2D.IgnoreCollision(obstacles[i].GetComponent<PolygonCollider2D>(), leftO.GetComponent<EdgeCollider2D>(), isOn);
            Physics2D.IgnoreCollision(obstacles[i].GetComponent<PolygonCollider2D>(), rightO.GetComponent<EdgeCollider2D>(), isOn);

            Physics2D.IgnoreCollision(obstacles[i].GetComponent<CircleCollider2D>(), leftO.GetComponent<EdgeCollider2D>(), isOn);
            Physics2D.IgnoreCollision(obstacles[i].GetComponent<CircleCollider2D>(), rightO.GetComponent<EdgeCollider2D>(), isOn);
        }
    }

    //Ignores collisions between player and the claw
    void ignorePlayerCollisions(bool isOn)
    {
        Physics2D.IgnoreCollision(player.GetComponent<PolygonCollider2D>(), leftO.GetComponent<EdgeCollider2D>(), isOn);
        Physics2D.IgnoreCollision(player.GetComponent<PolygonCollider2D>(), rightO.GetComponent<EdgeCollider2D>(), isOn);

        Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), leftO.GetComponent<EdgeCollider2D>(), isOn);
        Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), rightO.GetComponent<EdgeCollider2D>(), isOn);
    }
}

