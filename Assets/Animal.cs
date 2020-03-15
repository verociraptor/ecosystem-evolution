using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Animal : MonoBehaviour
{
    //What action is the rabbit currently doing?
    enum moveState { ground, jump, turning, breeding };
    enum direction { left, right, up, down }
    moveState rabbitState;

    float timeToDeathByHunger = 200;
    float timeToDeathByThirst = 200;

    public bool waterDesire = true;
    public float speed = 1f;

    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 startRot;
    public Vector3 endRot;
    public float jumpDist = 1;
    public float jumpTime = 1.0f;

    private PathFinding pathFinding;

    //field of view of animal
    public const int animalFOV = 9;
    
    public const string AStarTag = "astar";
   
    
    public float startTime;

    protected void Start()
    {

        startPos = transform.position;
        endPos = transform.position;
        endPos.x = transform.position.x + jumpDist;
        startRot = transform.rotation.eulerAngles;
        endRot = transform.rotation.eulerAngles;

        var aStar = GameObject.FindWithTag(AStarTag);
        pathFinding = aStar.GetComponent<PathFinding>();
        rabbitState = moveState.ground;
     

    }



    void move()
    {

        if (rabbitState == moveState.ground)
        {
            
            startTime = Time.time;
            
            rabbitState = moveState.jump;
        }

        if (rabbitState == moveState.turning)
        {
            transform.Rotate(0, 90, 0);


            startTime = Time.time;

            endPos = startPos;
            if (Math.Abs(transform.rotation.eulerAngles.y - 90) < 1)
            {
                endPos.z -= 1;
            }
            else if (Math.Abs(transform.rotation.eulerAngles.y - 180) < 1)
            {
                endPos.x -= 1;
            }
            else if (Math.Abs(transform.rotation.eulerAngles.y - 270) < 1)
            {
                endPos.z += 1;
            }
            else
            {
                endPos.x += 1;
            }

            rabbitState = moveState.jump;
        }

        if (rabbitState == moveState.jump)
        {
            if (Time.time - startTime <= 1.1)
            {
                
                Vector3 center = (startPos + endPos) * 0.5f;

                center -= new Vector3(0, 1, 0);

                Vector3 riseRelCenter = startPos - center;
                Vector3 setRelCenter = endPos - center;

                float fracComplete = (Time.time - startTime) / jumpTime;

                transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
                transform.position += center;
                Debug.Log("starttime" + startTime + "time-start" + (Time.time));
            }
            else
            {
                Debug.Log("starttime" + startTime);
                startPos = transform.position;
                endPos.x = transform.position.x + jumpDist;
                rabbitState = moveState.ground;
            }
        }
    }

    protected void Update()
    {
        if (waterDesire)
        {
            GetWater();
        }


        //move();
    }

    protected void GetWater()
    {
        Collider water = GetClosestWaterCollider(transform.position, Physics.OverlapSphere(transform.position, animalFOV));
        
        IList<Node> path = pathFinding.FindPath(transform.position, water.transform.position);
        
       
    }
    //can later be generalized for any food source/mate
    protected Collider GetClosestWaterCollider(Vector3 origin, Collider[] colliders)
    {
        float bestDistance = float.MaxValue;

        Collider bestCollider = null;

        foreach(Collider col in colliders)
        {
            if(col.tag == "closetowater")
            {
                float distance = Vector3.Distance(origin, col.transform.position);

                if(distance < bestDistance)
                {
                    bestDistance = distance;
                    bestCollider = col;
                }
            }
        }

        return bestCollider;
    }


}
