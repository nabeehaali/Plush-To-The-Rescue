using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainPlayerScript : MonoBehaviour
{
    // Score Vars
    public GameObject totalCount;
    public GameObject totalScore;
    public GameObject objective;
    bool newObjective;
    int score = 0;
    int collectTracker = 0;                         // Tracks collected amount of plushies throughout game (also including stolen ones)
    int throwTracker = 0;                           // Tracks # of thrown stuffies in Prize Bin
    bool cycle = false;

    // Sprite JS
    public GameObject joystickObj;

    // Player Action Vars
    bool animalInDistance;                          // Close enough to a plushy to collect
    public bool binDistance;                        // Close enough to bin to throw plushy
    bool throwPause;                                // Timing pause between plushy throws
    bool preventThrow;                              // Prevents any plushy throw based on Objective status

    // Player + movement
    public float moveSpeed;
    public Rigidbody2D rb;
    Vector2 keyboardMovement;
    Vector2 joystickMovement;

    // Temp plushy
    GameObject stuffy;                              // Recently collided with
    
    // Rescued line of plushies
    public List<GameObject> rescuedStuffies;

    // Prize Bin components
    public Transform binTargetPoint;                // Vector point where plushy is aimed at when thrown (PHYSICS)
    public PhysicsMaterial2D bouncyBounce;          // Bouncy plushy material

    // Sound + Animations
    SoundManager SoundManager;
    PlayerBearAnimation PlayerAnimatorScript;
    float bkgAnimationRate = 0;
    public GameObject lights;

    void Start()
    {
        // Collision Ignores
        Physics2D.IgnoreLayerCollision(6, 6, true);         // collected stuffies can't collide with one another
        Physics2D.IgnoreLayerCollision(7, 8, true);         // thrown stuffies can't collide with bin blockers
        Physics2D.IgnoreLayerCollision(8, 9, true);         // thrown stuffies can't collide with player

        animalInDistance = false;
        binDistance = false;
        throwPause = false;
        newObjective = false;
        preventThrow = false;

        SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        PlayerAnimatorScript = GetComponent<PlayerBearAnimation>();
    }

    void Update()
    {
        // Control Movement (keyboard + controller)
        keyboardMovement.x = Input.GetAxisRaw("Horizontal");
        keyboardMovement.y = Input.GetAxisRaw("Vertical");
        joystickMovement.x = Input.GetAxisRaw("JS1_hor");
        joystickMovement.y = Input.GetAxisRaw("JS1_ver");

        // Score Updates
        totalScore.GetComponent<TextMeshProUGUI>().text = score + "";
        totalCount.GetComponent<TextMeshProUGUI>().text = "COLLECTED: " + rescuedStuffies.Count + "/" + objective.GetComponent<GameObjective>().totalAnimals;

        if (collectTracker == objective.GetComponent<GameObjective>().totalAnimals)
        {
            totalCount.GetComponent<TextMeshProUGUI>().text = "COLLECTED: " + objective.GetComponent<GameObjective>().totalAnimals + "/" + objective.GetComponent<GameObjective>().totalAnimals;
        }

        if (collectTracker >= objective.GetComponent<GameObjective>().totalAnimals && newObjective)             // Objective completed, bring barrier down = players can throw
        {
            objective.GetComponent<GameObjective>().changeMessage();
            GameObject.Find("Barrier").GetComponent<PrizeBin>().moveDown();
            preventThrow = false;
        }
        else
        {
            if (!newObjective)                                                                                  // New objective, bring barrier up = players can't throw
            {
                collectTracker = rescuedStuffies.Count;
                throwTracker = 0;
                GameObject.Find("Barrier").GetComponent<PrizeBin>().moveUp();
                objective.GetComponent<GameObjective>().generateNumAnimals();
                newObjective = true;
                preventThrow = true;

                // sound
                SoundManager.PlaySounds("CompleteMission");

                // bg animation
                if (GameObject.FindGameObjectsWithTag("Animal").Length < 10)
                {
                    GameObject.Find("Instantiation").GetComponent<ObjectInstantiations>().makeObstacle();
                }
            }
        }

        if (throwTracker >= objective.GetComponent<GameObjective>().totalAnimals && newObjective)               // Players completed objective, resetting for a new objective
        {
            newObjective = false;
        }

        // Adding a plushy to rescue line
        if (Input.GetButtonDown("JoystickButtonCollect") || Input.GetKeyDown(KeyCode.Return))
        {
            if (animalInDistance)
            {
                // tag + layer 
                stuffy.transform.GetChild(0).transform.GetChild(0).tag = "Collected";
                stuffy.transform.GetChild(0).transform.GetChild(0).gameObject.layer = 6;

                if (rescuedStuffies.Count <= 0)                                                                 // No stuffies collected
                {
                    stuffy.transform.position = this.transform.position;
                    stuffy.transform.GetChild(0).GetComponent<HingeJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                }
                else                                                                                            // Stuffies already collected
                {
                    stuffy.transform.position = rescuedStuffies[rescuedStuffies.Count - 1].transform.GetChild(0).transform.position;
                    stuffy.transform.GetChild(0).GetComponent<HingeJoint2D>().connectedBody = rescuedStuffies[rescuedStuffies.Count - 1].transform.GetChild(0).GetComponent<Rigidbody2D>();
                }

                // Updating plushies stats
                stuffy.transform.GetChild(0).transform.GetChild(0).name = stuffy.transform.GetChild(0).transform.GetChild(0).GetComponent<PlushyScript>().animalType + "Plush" + rescuedStuffies.Count;

                // Adding to rescue line
                rescuedStuffies.Add(stuffy.transform.GetChild(0).gameObject);

                SoundManager.PlaySounds("Collection");
                PlayerAnimatorScript.PlayerAnimationSpeed(rescuedStuffies.Count);

                // Removing parent as the reference pt for joint connection is no longer needed
                stuffy.transform.GetChild(0).transform.parent = null;
                Destroy(stuffy.gameObject);
                stuffy = null;
                animalInDistance = false;

                // Player stats update
                collectTracker++;
                increaseSpeed();

                // Increase bkg anim speed
                bkgAnimationRate += 0.3f;
                if (bkgAnimationRate >= 1.5f)
                {
                    bkgAnimationRate = 1.5f;
                }
                GameObject.Find("SceneBackground").GetComponent<Animator>().SetFloat("Rate", bkgAnimationRate);
                GameObject.Find("MainCamera").GetComponent<Animator>().SetFloat("Rate", bkgAnimationRate);
            }
        }

        // Throwing a plushy in prize bin
        if (Input.GetButtonDown("JoystickButtonThrow") || Input.GetKeyDown(KeyCode.Space))
        {
            if (binDistance && !throwPause && !preventThrow)
            {
                if (rescuedStuffies.Count >= 1)
                {
                    // Getting last plushy in line and updating stats
                    GameObject lastStuffy = rescuedStuffies[rescuedStuffies.Count - 1].gameObject;
                    rescuedStuffies.RemoveAt(rescuedStuffies.Count - 1);
                    GameObject disposedStuffy = lastStuffy.transform.GetChild(0).gameObject;                            // Child, so not the joint but the plushy
                    disposedStuffy.transform.parent = null;
                    disposedStuffy.name = "ThrownStuffy";
                    Destroy(lastStuffy);                                                                                // Parent = joint gameobject = no longer needed
                    Destroy(disposedStuffy.GetComponent<HingeJoint2D>());
                    disposedStuffy.transform.position = transform.position;                                             // Same pos as player so the throw looks more accurate
                    
                    // Seen when needed (prize bin, blocked by machine, seen through chute)
                    disposedStuffy.layer = 8;
                    disposedStuffy.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
                    disposedStuffy.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    throwStuffy(disposedStuffy, binTargetPoint.position, disposedStuffy.transform.position);

                    // Destroy plushy
                    disposedStuffy.GetComponent<PlushyScript>().thrownStuffyDestroy();

                    // Score update
                    throwTracker++;
                    score++;
                    pointSystem(disposedStuffy);

                    SoundManager.PlaySounds("ThrowStuffySound");
                    SoundManager.PlaySounds("ThrowStuffyPostive");
                    PlayerAnimatorScript.PlayerAnimationSpeed(rescuedStuffies.Count);

                    // Decrease bkg animation speed
                    bkgAnimationRate -= 0.3f;
                    if (bkgAnimationRate <= 0)
                    {
                        bkgAnimationRate = 0;
                    }
                    GameObject.Find("SceneBackground").GetComponent<Animator>().SetFloat("Rate", bkgAnimationRate);
                    GameObject.Find("MainCamera").GetComponent<Animator>().SetFloat("Rate", bkgAnimationRate);
                }
            }
        }
    }

    void FixedUpdate()
    {
        // Player movement
        rb.MovePosition(rb.position + joystickMovement.normalized * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + keyboardMovement.normalized * moveSpeed * Time.fixedDeltaTime);

        // Sprite JS movement
        if(keyboardMovement.x > 0.2f || joystickMovement.x > 0.2f)
        {
            joystickObj.transform.eulerAngles = new Vector3(-13f, 21, -55);
        }
        else if(keyboardMovement.x < -0.2f || joystickMovement.x < -0.2f)
        {
            joystickObj.transform.eulerAngles = new Vector3(-16f, 28f, 29);
        }
        else if(keyboardMovement.y > 0.2f || joystickMovement.y > 0.2f)
        {
            joystickObj.transform.eulerAngles = new Vector3(34f, 22, -13);
        }
        else if(keyboardMovement.y < -0.2f || joystickMovement.y < -0.2f)
        {
            joystickObj.transform.eulerAngles = new Vector3(-55f, 25, -2);
        }
        else
        {
            joystickObj.transform.eulerAngles = new Vector3(0, 33f, 0f);
        }
    }

    // New code for flicker animation
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            SoundManager.PlaySounds("bump");
            GameObject.Find("SceneBackground").GetComponent<Animator>().SetTrigger("Obstacle");

            //NEW
            StartCoroutine(lightsOnOff());
            StartCoroutine(dizzy());
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Player caught by Claw
        if (col.tag == "Claw")
        {
            if (rescuedStuffies.Count > 0)
            {
                Destroy(rescuedStuffies[0].GetComponent<HingeJoint2D>());                                   // Detach first plushy's hinge from the player so only players taken away
            }
            rescuedStuffies.Clear();

            Physics2D.IgnoreLayerCollision(9, 16, true);                                                    // Player ignores border colliders
        }

        // Player finds an available plushy
        if (col.gameObject.tag == "Animal") 
        {
            stuffy = col.gameObject.transform.parent.parent.gameObject;
            animalInDistance = true;
        }

        // Player hits an obstacle
        if (col.tag == "Obstacle")
        {
            SoundManager.PlaySounds("bump");
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        // Player caught by claw
        if (col.tag == "Claw")                                                                                     
        {
            col.transform.parent.parent.GetComponent<ClawMovement>().caughtStuffy = true;
            col.transform.parent.parent.GetComponent<ClawMovement>().gameover = true;
            this.transform.position = col.transform.position;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
                cycle = false;

        // Player ignores available plushy
        if (col.gameObject.tag == "Animal")
        {
            animalInDistance = false;
            stuffy = null;
        }
    }

    // Removing plushies from rescue line
    public void removeStuffies(string caughtStuffy)
    {
        for (int i = 0; i < rescuedStuffies.Count; i++)
        {
            if(rescuedStuffies[i].transform.GetChild(0).name == caughtStuffy)                                       // Find caught plushy
            {
                for(int y = i; i < rescuedStuffies.Count; y++)                                                      // Remove every plushy that's saved in that index
                {
                    Physics2D.IgnoreLayerCollision(11,16, true);
                    rescuedStuffies[i].transform.GetChild(0).gameObject.layer =11;
                    rescuedStuffies[i].transform.GetChild(0).GetComponent<Rigidbody2D>().gravityScale = 1f;
                    decreaseSpeed();
                    rescuedStuffies.RemoveAt(i);

                    PlayerAnimatorScript.PlayerAnimationSpeed(rescuedStuffies.Count);
                }
            }
        }        
    }

    // Throwing plushy into bin == PHYSICS
    void MoveBoy(GameObject stuffy, Vector2 vel)
    {
        stuffy.GetComponent<Rigidbody2D>().velocity = vel;
    }

    void throwStuffy(GameObject stuffy, Vector2 targetPos, Vector2 startPos)
    {
        stuffy.transform.GetChild(0).gameObject.SetActive(false);

        stuffy.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

        stuffy.GetComponent<PolygonCollider2D>().isTrigger = false;
        stuffy.GetComponent<Rigidbody2D>().gravityScale = 0.7f;

        Vector2 direction = targetPos - startPos;

        float height = direction.y;
        Vector2 halfRange = new Vector2(direction.x, 0);
        float Vy = Mathf.Sqrt(-2 * Physics2D.gravity.y * height);
        Vector2 VX = -(halfRange * Physics2D.gravity.y) / Vy;

        float randForceX = Random.Range(1.5f, 4);
        float randForceY = Random.Range(1, 2);
        MoveBoy(stuffy, new Vector2(VX.x / randForceX, Vy * randForceY));
    }

    // Player's speed inc/dec
    public void increaseSpeed()
    {
        moveSpeed += 0.12f;
    }
    public void decreaseSpeed()
    {
        moveSpeed -= 0.12f;
    }

    // Get how many plushies are currently in the rescue line
    public int getLineCount()
    {
        return rescuedStuffies.Count;
    }

    // Identify which type of plushy was thrown into the prize bin
    void pointSystem(GameObject stuffy)
    {
        //NEW CODE
        if (stuffy.GetComponent<PlushyScript>().animalType == "Cat")
        {
            Debug.Log("Point for " + stuffy.GetComponent<PlushyScript>().animalType);
            GameObject.Find("PointManager").GetComponent<PointManager>().catCount++;
            //catCount++;
        }
        else if (stuffy.GetComponent<PlushyScript>().animalType == "Cow")
        {
            Debug.Log("Point for " + stuffy.GetComponent<PlushyScript>().animalType);
            GameObject.Find("PointManager").GetComponent<PointManager>().cowCount++;
            //cowCount++;
        }
        else if (stuffy.GetComponent<PlushyScript>().animalType == "Chicken")
        {
            Debug.Log("Point for " + stuffy.GetComponent<PlushyScript>().animalType);
            GameObject.Find("PointManager").GetComponent<PointManager>().chickenCount++;
            //chickenCount++;
        }
        else if (stuffy.GetComponent<PlushyScript>().animalType == "Whale")
        {
            Debug.Log("Point for " + stuffy.GetComponent<PlushyScript>().animalType);
            GameObject.Find("PointManager").GetComponent<PointManager>().whaleCount++;
            //whaleCount++;
        }
        else if (stuffy.GetComponent<PlushyScript>().animalType == "Rabbit")
        {
            Debug.Log("Point for " + stuffy.GetComponent<PlushyScript>().animalType);
            GameObject.Find("PointManager").GetComponent<PointManager>().rabbitCount++;
            //rabbitCount++;
        }
        else
        {
            Debug.Log("Unidentified animal!");
        }

        StartCoroutine("holdThrow", 0.5f);
    }

    // Pause between throws to prevent unappealing plushy overlapping
    IEnumerator holdThrow(int value)
    {
        throwPause = true;
        decreaseSpeed();

        yield return new WaitForSeconds(value);

        throwPause = false;
    }

    IEnumerator lightsOnOff()
    {
        lights.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        lights.SetActive(true);
    }

    IEnumerator dizzy()
    {
        transform.Find("dizzy").gameObject.SetActive(true);
        float tempSpeed = moveSpeed;
        moveSpeed = 0;
        yield return new WaitForSeconds(1);
        transform.Find("dizzy").gameObject.SetActive(false);
        moveSpeed = tempSpeed;
    }
}