using System;
using System.Collections;
using NUnit.Framework.Constraints;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool smoothMove;
    private bool isMoving;
    public bool canFlicker;
    public float flickerTime;
    private Vector3 startPos;
    private Vector3 endPos;
    public float timeToMove;
    public float distance;
    public Transform frontMovePoint;
    public Transform backMovePoint;
    public Transform leftMovePoint;
    public Transform rightMovePoint;
    public LayerMask obstical;

    public Lighting lighting;
    public Vector3? bufferInput;
    private Coroutine currentMove;
    private Coroutine flickerCoroutine;

    public bool canMove;

    public HungerController hungerController;


    void Start()
    {
        lighting.ClearScreen();
        lighting.SetLight(transform.position);
        lighting.SetGroundLight(transform.position);
        canFlicker = true;
    }

    void Update()
    {
        
        Vector3? newInput = GetInput();
        if (newInput.HasValue)
        {
            if (isMoving && !canMove)
            {
                bufferInput = newInput.Value;
            }
            else
            {
                Move(newInput.Value);
            }
        }
        if (!isMoving && canFlicker && flickerCoroutine == null)
        {
            flickerCoroutine = StartCoroutine(Flicker());
        }


        

       
    }

    private Vector3? GetInput()
    {
        if (!canMove) return null;

        if (Input.GetKey(KeyCode.W) && !isMoving && !Physics2D.OverlapCircle(frontMovePoint.position, 0.2f, obstical))
        {
           
            return new Vector3(0f, distance, 0f);
            
        }
        if (Input.GetKey(KeyCode.A) && !isMoving && !Physics2D.OverlapCircle(leftMovePoint.position, 0.2f, obstical))
        {
            return new Vector3(-distance, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.S) && !isMoving && !Physics2D.OverlapCircle(backMovePoint.position, 0.2f, obstical))
        {
            return new Vector3(0f, -distance, 0f);
        }
        if (Input.GetKey(KeyCode.D) && !isMoving && !Physics2D.OverlapCircle(rightMovePoint.position, 0.2f, obstical))
        {
            return new Vector3(distance, 0f, 0f);
        }
        return null;
    }
   
    private void Move(Vector3 direction)
    {
        if (currentMove != null)
        {
            StopCoroutine(currentMove);
        }
        currentMove = StartCoroutine(MovePlayer(direction));
    }
    

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        bool hasLit = false;
        float timeTaken = 0;
        startPos = transform.position;
        endPos = startPos + direction;
        if (smoothMove)
        {
            while(timeTaken < timeToMove)
            {
                transform.position = Vector3.Lerp(startPos, endPos, (timeTaken / timeToMove));
                timeTaken += Time.deltaTime;
                if (timeTaken > (timeToMove / 2) - 0.05f && timeTaken < (timeToMove / 2) + 0.05f && !hasLit)
                {
                    lighting.SetGroundLight(endPos);
                    hasLit = true;
                }
                yield return null;
            }
        }
        else yield return new WaitForSeconds(timeToMove);
        
        
       
        transform.position = endPos;
        lighting.SetLight(transform.position);
        isMoving = false;
        currentMove = null;

        if (transform.position.x >= hungerController.uIController.winPositionX)
        {
            hungerController.uIController.WinScreen();
            yield break; // Stop here, don't continue movement
        }

        if (bufferInput.HasValue)
        {
            Vector3 nextMove = bufferInput.Value;
            bufferInput = null;
            Move(nextMove);
        }
        hungerController.Check();
        hungerController.Starve(1);
        
    }

    private IEnumerator Flicker()
    {
        canFlicker = false;
        lighting.SetGroundLight(transform.position);
        yield return new WaitForSeconds(flickerTime);
        canFlicker = true;
        flickerCoroutine = null;
    }
 
}




 /*
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3()))
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            }
            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
            } 
        }
        
        */