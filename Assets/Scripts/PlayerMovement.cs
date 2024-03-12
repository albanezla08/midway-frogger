using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 speed;
    private Vector3 startPos;
    [SerializeField] private float inputDelay;
    [SerializeField] private float anticipation;
    private float inputTimer = 0;
    private bool canMove = true;
    private Vector3 lastDirection;
    private Coroutine moveRoutine;
    [SerializeField] private TilesManager tilesManager;

    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;

    private void Start()
    {
        speed = tilesManager.getTileSize();
        startPos = tilesManager.getBottomLeft() + speed/2 + new Vector3(0.001f,0.001f);
        transform.position = startPos;
    }

    // Update is called once per frame
    private void Update()
    {
        // disable input delay for now
        // if (inputTimer < inputDelay)
        // {
        //     canMove = false;
        //     inputTimer += Time.deltaTime;
        // }
        // else
        // {
        //     canMove = true;
        // }
        if (canMove)
        {
            CheckMovement();
        }
    }

    private void CheckMovement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            spriteRenderer.sprite = spriteArray[0];
            Move(Vector3.up);
        }
        else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && transform.position != startPos)
        {
            spriteRenderer.sprite = spriteArray[1];
            Move(Vector3.down);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            spriteRenderer.sprite = spriteArray[2];
            Move(Vector3.right);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) && transform.position != startPos)
        {
            spriteRenderer.sprite = spriteArray[3];
            Move(Vector3.left);
        }
    }

    private void Move(Vector3 direction)
    {
        canMove = false;
        // inputTimer = 0; // start input delay when move
        Vector3 nextPosition = transform.position + Vector3.Scale(direction,speed);
        if (!tilesManager.canMoveToTile(nextPosition)) // if cant move to nextPos
        {
            canMove = true;
            return;
        }
        lastDirection = direction; // used to know which direction to slide on ice
        moveRoutine = StartCoroutine(MoveOverTime(nextPosition, inputDelay, anticipation));
    }

    private IEnumerator MoveOverTime(Vector3 target, float time, float startupTime)
    {
        yield return new WaitForSeconds(startupTime);
        float elapsedTime = 0;
        float t;
        while (elapsedTime < time)
        {
            t = elapsedTime / time;
            transform.position = Vector3.Lerp(transform.position, target, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
        EndMove();
    }

    private void EndMove()
    {
        canMove = true;
        tilesManager.checkTileEffects(transform.position, this);
    }

    public void Respawn()
    {
        StopCoroutine(moveRoutine);
        transform.position = startPos;
        canMove = true;
    }

    public void Win() {
        Debug.Log("Win");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void IceSlide() {
        canMove = false;
        Vector3 speedVec = Vector3.Scale(lastDirection, speed);
        Vector3 nextPos = transform.position + speedVec;
        if (!tilesManager.canMoveToTile(nextPos)) // if cant move to nextPos
        {
            canMove = true;
            return;
        }
        moveRoutine = StartCoroutine(MoveSlide(nextPos, 0.07f));
    }

    private IEnumerator MoveSlide(Vector3 target, float s)
    {
        while (Vector3.Distance(transform.position, target) > 0.001)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, s);
            yield return null;
        }
        transform.position = target;
        EndMove();
    }

    // private IEnumerator AnimateSlide()
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(0.5f);
    //     }
    // }
}
