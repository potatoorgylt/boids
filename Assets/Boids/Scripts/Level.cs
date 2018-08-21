﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public Transform memberPrefab;
    public Transform enemyPrefab;
    public int numberOfMembers;
    public int numberOfEnemies;
    public List<Member> members;
    public List<Enemy> enemies;
    public Vector3 bounds;
    public float spawnRadius;
	// Use this for initialization
	void Start () {
        members = new List<Member>();
        enemies = new List<Enemy>();

        Spawn(memberPrefab, numberOfMembers);
        Spawn(enemyPrefab, numberOfEnemies);

        members.AddRange(FindObjectsOfType<Member>());
        enemies.AddRange(FindObjectsOfType<Enemy>());
    }
	
    void Spawn(Transform prefab, int count)
    {
        for(int i = 0; i < count; i++)
        {
            Instantiate(prefab, new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius)),
                Quaternion.identity);
        }
    }
    public List<Member> GetNeighbors(Member member, float radius)
    {
        List<Member> neighborsFound = new List<Member>();

        foreach(var otherMember in members)
        {
            if(otherMember == member)
            {
                continue;
            }
            if(Vector3.Distance(member.position, otherMember.position) <= radius)
            {
                neighborsFound.Add(otherMember);
            }
        }
        return neighborsFound;
    }

    public List<Enemy> GetEnemies(Member member, float radius)
    {
        List<Enemy> returnEnemies = new List<Enemy>();
        foreach(var enemy in enemies)
        {
            if(Vector3.Distance(member.position, enemy.position) <= radius)
            {
                returnEnemies.Add(enemy);
            }
        }
        return returnEnemies;
    }
}
