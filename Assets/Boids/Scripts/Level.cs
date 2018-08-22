using System.Collections;
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
	// Use this for initialization
	void Start () {
        members = new List<Member>();
        enemies = new List<Enemy>();
        Vector3 spawnPos = bounds / 2;
        Spawn(memberPrefab, numberOfMembers, spawnPos);
        Spawn(enemyPrefab, numberOfEnemies, spawnPos);

        members.AddRange(FindObjectsOfType<Member>());
        enemies.AddRange(FindObjectsOfType<Enemy>());
    }
	
    void Spawn(Transform prefab, int count, Vector3 spawnPos)
    {
        Transform temp;
        for(int i = 0; i < count; i++)
        {
            Vector3 randPos = new Vector3(
                Random.Range(-spawnPos.x + transform.position.x, spawnPos.x + transform.position.x),
                Random.Range(-spawnPos.y + transform.position.y, spawnPos.y + transform.position.y),
                Random.Range(-spawnPos.z + transform.position.z, spawnPos.z + transform.position.z));

            temp = Instantiate(prefab, randPos, Quaternion.identity);
            temp.transform.parent = gameObject.transform;
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5F);
        Gizmos.DrawWireCube(transform.position, new Vector3(bounds.x, bounds.y, bounds.z));
    }
}
