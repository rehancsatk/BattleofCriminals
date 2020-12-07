using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField]
    private GameObject boar_Prefab, cannibal_Prefab;

    public Transform[] cannibal_Spawnpoints, boar_spawnpoint;

    [SerializeField]
    private int cannibal_enemycount, boar_Enemycount;

    private int initial_Cannibal, initial_boar;

    public float wait_before_Spawn_Enemytime = 10f;


    void Awake()
    {
        MakeInstance();
    }
    void Start()
    {
        initial_Cannibal = cannibal_enemycount;
        initial_boar = boar_Enemycount;

        SpawnEnemy();

        StartCoroutine("ChecktoSpawnEnemies");
    }
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void SpawnEnemy()
    {
        SpawnCannibals();
        SpawnBoars();
    }
    void SpawnCannibals()
    {
        int index = 0;
        for (int i = 0; i < cannibal_enemycount; i++)
        {
            if (index >= cannibal_Spawnpoints.Length)
            {
                index = 0;
            }
            Instantiate(cannibal_Prefab, cannibal_Spawnpoints[index].position, Quaternion.identity);
            index++;
        }
        cannibal_enemycount = 0;
    }
    void SpawnBoars()
    {
        int index = 0;
        for (int i = 0; i < boar_Enemycount; i++)
        {
            if (index >= boar_spawnpoint.Length)
            {
                index = 0;
            }
            Instantiate(boar_Prefab, boar_spawnpoint[index].position, Quaternion.identity);
            index++;
        }
        boar_Enemycount = 0;

    }
    IEnumerator ChecktoSpawnEnemies()
    {
        yield return new WaitForSeconds(wait_before_Spawn_Enemytime);

        SpawnCannibals();

        SpawnBoars();

        StartCoroutine("ChecktoSpawnEnemies");
    }
    public void EnemyDied(bool cannibal) //esko access kraw gy healthscript me jo indicate kre ga yahan
    {
        if (cannibal) // indicate ky cannibal mar gya
        {
            cannibal_enemycount++; // so agly cannibals ajae gy aik me increment hu ga
            if (cannibal_enemycount > initial_Cannibal)
            {
                cannibal_enemycount = initial_Cannibal;
            }
        }
        else
        {
            boar_Enemycount++;
            if (boar_Enemycount > initial_boar)
            {
                boar_Enemycount = initial_boar;
            }
        }
    }
    public void StopSpawing()
    {
        StopCoroutine("ChecktoSpawnEnemies");
    }
}
