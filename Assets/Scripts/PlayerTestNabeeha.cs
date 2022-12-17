using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTestNabeeha : MonoBehaviour
{
    public GameObject totalCount;
    public GameObject totalScore;
    public GameObject objective;
    bool newObjective = false;

    public GameObject joystickObj;

    bool cycle = false;

    int score = 0;

    bool animalInDistance;
    bool binDistance;
    bool throwPause;
    bool preventThrow;

    int collectTracker = 0;
    int throwTracker = 0;

    public float moveSpeed;
    public Rigidbody2D rb;
    Vector2 keyboardMovement;
    Vector2 joystickMovement;


    GameObject stuffy;
    public List<GameObject> rescuedStuffies;

    public Transform binTargetPoint;
    public PhysicsMaterial2D bouncyBounce;

    SoundManager SoundManager;
    PlayerBearAnimation PlayerAnimatorScript;
    float bkgAnimationRate = 0;
    //NEW
    public GameObject lights;

    void Start()
    {
        //rescuedStuffies.Add(this.gameObject);
        Physics2D.IgnoreLayerCollision(6, 6, true);         // collected stuffies can't collide
        Physics2D.IgnoreLayerCollision(7, 8, true);         // thrown stuffies can't collide with bin restriction
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
        keyboardMovement.x = Input.GetAxisRaw("Horizontal");
        keyboardMovement.y = Input.GetAxisRaw("Vertical");

        joystickMovement.x = Input.GetAxisRaw("JS1_hor");
        joystickMovement.y = Input.GetAxisRaw("JS1_ver");

        totalScore.GetComponent<TextMeshProUGUI>().text = score + "";

        //updating total score count
        //totalAnimals = catCount + cowCount + chickenCount + rabbitCount + whaleCount;

        //New score code (from Nabeeha)
        totalCount.GetComponent<TextMeshProUGUI>().text = "COLLECTED: " + rescuedStuffies.Count + "/" + objective.GetComponent<GameObjective>().totalAnimals;
        if(collectTracker == objective.GetComponent<GameObjective>().totalAnimals)
        {
            totalCount.GetComponent<TextMeshProUGUI>().text = "COLLECTED: " + objective.GetComponent<GameObjective>().totalAnimals + "/" + objective.GetComponent<GameObjective>().totalAnimals;
        }

        //Debug.Log("score : " + totalAnimals);

        if (collectTracker >= objective.GetComponent<GameObjective>().totalAnimals && newObjective)
        {
            objective.GetComponent<GameObjective>().changeMessage();
            GameObject.Find("Barrier").GetComponent<PrizeBin>().moveDown();
            preventThrow = false;
        }
        else
        {
            if (!newObjective)
            {
                collectTracker = rescuedStuffies.Count;
                throwTracker = 0;
                GameObject.Find("Barrier").GetComponent<PrizeBin>().moveUp();
                objective.GetComponent<GameObjective>().generateNumAnimals();
                newObjective = true;
                preventThrow = true;

                SoundManager.PlaySounds("CompleteMission");

                //instantiate one more block (from Nabeeha)
                if (GameObject.FindGameObjectsWithTag("Animal").Length < 10)
                {
                    GameObject.Find("Instantiation").GetComponent<ObjectInstantiations>().makeObstacle();
                }
            }
        }

        if (throwTracker >= objective.GetComponent<GameObjective>().totalAnimals && newObjective)
        {
            newObjective = false;
        }

        // COLLECTING STUFFY
        if (Input.GetButtonDown("JoystickButtonCollect") || Input.GetKeyDown(KeyCode.Return))
        {
            if (animalInDistance)
            {
                stuffy.transform.GetChild(0).transform.GetChild(0).tag = "Collected";
                stuffy.transform.GetChild(0).transform.GetChild(0).gameObject.layer = 6;

                if (rescuedStuffies.Count <= 0)         // No stuffies collected
                {
                    stuffy.transform.position = this.transform.position;
                    stuffy.transform.GetChild(0).GetComponent<HingeJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                }
                else                                    // Stuffies already collected
                {
                    stuffy.transform.position = rescuedStuffies[rescuedStuffies.Count - 1].transform.GetChild(0).transform.position;
                    stuffy.transform.GetChild(0).GetComponent<HingeJoint2D>().connectedBody = rescuedStuffies[rescuedStuffies.Count - 1].transform.GetChild(0).GetComponent<Rigidbody2D>();
                }

                // saving stuffy
                stuffy.transform.GetChild(0).transform.GetChild(0).name = stuffy.transform.GetChild(0).transform.GetChild(0).GetComponent<PlushyScript>().animalType + "Plush" + rescuedStuffies.Count;

                rescuedStuffies.Add(stuffy.transform.GetChild(0).gameObject);

                //increase bkg anim speed by (from Nabeeha)
                bkgAnimationRate += 0.3f;
                if (bkgAnimationRate >= 1.5f)
                {
                    bkgAnimationRate = 1.5f;
                }
                GameObject.Find("SceneBackground").GetComponent<Animator>().SetFloat("Rate", bkgAnimationRate);
                GameObject.Find("MainCamera").GetComponent<Animator>().SetFloat("Rate", bkgAnimationRate);

                SoundManager.PlaySounds("Collection");
                PlayerAnimatorScript.PlayerAnimationSpeed(rescuedStuffies.Count);

                //removing reference pt
                stuffy.transform.GetChild(0).transform.parent = null;
                Destroy(stuffy.gameObject);
                stuffy = null;
                animalInDistance = false;

                collectTracker++;

                increaseSpeed();
            }
        }

        // THROWING STUFFY
        if (Input.GetButtonDown("JoystickButtonThrow") || Input.GetKeyDown(KeyCode.Space))
        {
            if (binDistance && !throwPause && !preventThrow)
            {
                if (rescuedStuffies.Count >= 1)
                {
                    GameObject lastStuffy = rescuedStuffies[rescuedStuffies.Count - 1].gameObject;
                    rescuedStuffies.RemoveAt(rescuedStuffies.Count - 1);

                    GameObject disposedStuffy = lastStuffy.transform.GetChild(0).gameObject;
                    disposedStuffy.transform.parent = null;
                    disposedStuffy.name = "ThrownStuffy";
                    Destroy(lastStuffy);

                    Destroy(disposedStuffy.GetComponent<HingeJoint2D>());
                    disposedStuffy.transform.position = transform.position;
                    disposedStuffy.layer = 8;
                    disposedStuffy.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
                    disposedStuffy.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    throwStuffy(disposedStuffy, binTargetPoint.position, disposedStuffy.transform.position);

                    disposedStuffy.GetComponent<PlushyScript>().thrownStuffyDestroy();

                    throwTracker++;
                    score++;

                    pointSystem(disposedStuffy);

                    SoundManager.PlaySounds("ThrowStuffySound");
                    SoundManager.PlaySounds("ThrowStuffyPostive");
                    PlayerAnimatorScript.PlayerAnimationSpeed(rescuedStuffies.Count);

                    //decrease animation speed (from Nabeeha)
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

    IEnumerator holdThrow(int value)
    {
        throwPause = true;

        decreaseSpeed();

        yield return new WaitForSeconds(value);

        throwPause = false;
    }

    // Player movement
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + joystickMovement.normalized * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + keyboardMovement.normalized * moveSpeed * Time.fixedDeltaTime);

        if (keyboardMovement.x > 0.2f || joystickMovement.x > 0.2f)
        {
            joystickObj.transform.eulerAngles = new Vector3(-13f, 21, -55);
        }
        else if (keyboardMovement.x < -0.2f || joystickMovement.x < -0.2f)
        {
            joystickObj.transform.eulerAngles = new Vector3(-16f, 28f, 29);
        }
        else if (keyboardMovement.y > 0.2f || joystickMovement.y > 0.2f)
        {
            joystickObj.transform.eulerAngles = new Vector3(34f, 22, -13);
        }
        else if (keyboardMovement.y < -0.2f || joystickMovement.y < -0.2f)
        {
            joystickObj.transform.eulerAngles = new Vector3(-55f, 25, -2);
        }
        else
        {
            joystickObj.transform.eulerAngles = new Vector3(0, 33f, 0f);
        }
    }

    public int getLineCount()
    {
        return rescuedStuffies.Count;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Claw")
        {
            if (rescuedStuffies.Count > 0)
            {
                Destroy(rescuedStuffies[0].GetComponent<HingeJoint2D>());
            }

            rescuedStuffies.Clear();

            Physics2D.IgnoreLayerCollision(9, 16, true);
        }

        if (col.gameObject.tag == "Animal")
        {
            stuffy = col.gameObject.transform.parent.parent.gameObject;
            animalInDistance = true;
        }

    }

    //New code for flicker animation (from Nabeeha)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            SoundManager.PlaySounds("bump");
            GameObject.Find("SceneBackground").GetComponent<Animator>().SetTrigger("Obstacle");
            //NEW
            StartCoroutine(lightsOnOff());
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Claw")
        {
            col.transform.parent.parent.GetComponent<ClawMovement>().caughtStuffy = true;
            col.transform.parent.parent.GetComponent<ClawMovement>().gameover = true;
            this.transform.position = col.transform.position;
        }
        if (col.gameObject.tag == "PrizeBin")
        {
            binDistance = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        cycle = false;

        if (col.gameObject.tag == "Animal")
        {
            animalInDistance = false;
            stuffy = null;
        }

        if (col.gameObject.tag == "PrizeBin")
        {
            binDistance = false;
        }
    }

    // Removing stuffies from rescued list
    public void removeStuffies(string caughtStuffy)
    {
        for (int i = 0; i < rescuedStuffies.Count; i++)
        {
            if (rescuedStuffies[i].transform.GetChild(0).name == caughtStuffy)
            {
                for (int y = i; i < rescuedStuffies.Count; y++)
                {
                    Physics2D.IgnoreLayerCollision(11, 16, true);

                    rescuedStuffies[i].transform.GetChild(0).gameObject.layer = 11;
                    rescuedStuffies[i].transform.GetChild(0).GetComponent<Rigidbody2D>().gravityScale = 1f;

                    decreaseSpeed();

                    rescuedStuffies.RemoveAt(i);

                    PlayerAnimatorScript.PlayerAnimationSpeed(rescuedStuffies.Count);

                }
            }
        }
    }

    // Throwing stuffy into bin
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

    public void increaseSpeed()
    {
        moveSpeed += 0.12f;
    }

    public void decreaseSpeed()
    {
        moveSpeed -= 0.12f;
    }

    //NEW
    IEnumerator lightsOnOff()
    {
        lights.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        lights.SetActive(true);
    }
}
