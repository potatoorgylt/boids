﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour {

    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public Level level;
    public MemberConfig conf;

    Vector3 wanderTarget;

    private void Start()
    {
        level = FindObjectOfType<Level>();
        conf = FindObjectOfType<MemberConfig>();

        position = transform.position;
        velocity = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
    }

    private void Update()
    {
        acceleration = Combine();
        acceleration = Vector3.ClampMagnitude(acceleration, conf.maxAcceleration);
        velocity = velocity + acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, conf.maxVelocity);
        position = position + velocity * Time.deltaTime;
        WrapAround(ref position, -level.bounds, level.bounds);
        transform.position = position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), 0.15F); ;
    }

    protected Vector3 Wander()
    {
        float jitter = conf.wanderJitter * Time.deltaTime;
        wanderTarget += new Vector3(RandomBinomial() * jitter, RandomBinomial() * jitter, RandomBinomial() * jitter);
        wanderTarget = wanderTarget.normalized;
        wanderTarget *= conf.wanderRadius;
        Vector3 targetInLocalSpace = wanderTarget + new Vector3(0, conf.wanderDistance, 0); //TODO: check for 3d
        Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);
        targetInWorldSpace -= this.position;
        return targetInWorldSpace.normalized;
    }

    Vector3 Cohesion()
    {
        Vector3 cohesionVector = new Vector3();
        int countMembers = 0;
        var neighbors = level.GetNeighbors(this, conf.cohesionRadius);
        if (neighbors.Count == 0)
            return cohesionVector;
        foreach(var member in neighbors)
        {
            if (isInFOV(member.position))
            {
                cohesionVector += member.position;
                countMembers++;
            }
        }
        if(countMembers == 0)
        {
            return cohesionVector;
        }
        cohesionVector /= countMembers;
        cohesionVector = cohesionVector - this.position;
        cohesionVector = Vector3.Normalize(cohesionVector);
        return cohesionVector;
    }

    Vector3 Alignment()
    {
        Vector3 alignVector = new Vector3();
        var members = level.GetNeighbors(this, conf.aligmentRadius);
        if (members.Count == 0)
            return alignVector;

        foreach(var member in members)
        {
            if (isInFOV(member.position))
            {
                alignVector += member.velocity;
            }
        }
        return alignVector.normalized;
    }

    Vector3 Separation()
    {
        Vector3 separateVector = new Vector3();
        var members = level.GetNeighbors(this, conf.seperationRadius);
        if(members.Count == 0)
        {
            return separateVector;
        }
        foreach(var member in members)
        {
            if (isInFOV(member.position))
            {
                Vector3 movingTowards = this.position - member.position;
                if(movingTowards.magnitude > 0)
                {
                    separateVector += movingTowards.normalized / movingTowards.magnitude;
                }
            }
        }

        return separateVector.normalized;
    }

    Vector3 Avoidance()
    {
        Vector3 avoidVector = new Vector3();
        var enemyList = level.GetEnemies(this, conf.avoidanceRadius);
        if(enemyList.Count == 0)
        {
            return avoidVector;
        }
        foreach(var enemy in enemyList)
        {
            avoidVector += RunAway(enemy.position);
        }
        return avoidVector.normalized;
    }

    Vector3 RunAway(Vector3 target)
    {
        Vector3 neededVelocity = (position - target).normalized * conf.maxVelocity;
        return neededVelocity - velocity;
    }

    virtual protected Vector3 Combine()
    {
        Vector3 finalVec = conf.cohesionPriority * Cohesion() + conf.wanderPriority * Wander()
            + conf.aligmentPriority * Alignment() + conf.seperationPriority * Separation()
            + conf.avoidancePriority * Avoidance();
        return finalVec;
    }

    void WrapAround(ref Vector3 vector, Vector3 min, Vector3 max)
    {
        vector.x = WrapAroundFloat(vector.x, min.x, max.x);
        vector.y = WrapAroundFloat(vector.y, min.y, max.y);
        vector.z = WrapAroundFloat(vector.z, min.z, max.z);
    }
    float WrapAroundFloat(float value, float min, float max)
    {
        if(value > max)
        {
            value = min;
        }
        else if(value < min)
        {
            value = max;
        }
        return value;
    }

    float RandomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    bool isInFOV(Vector3 vec)
    {
        return Vector3.Angle(this.velocity, vec - this.position) <= conf.maxFOV;
    }
}
