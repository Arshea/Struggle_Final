using UnityEngine;
using System.Collections;

public class EnemyTerritory : MonoBehaviour
{
        Collider territory;
        GameObject player;
        bool playerInTerritory;
 
        public GameObject enemy;
        BasicEnemy basicenemy;
 
        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            basicenemy = enemy.GetComponent <BasicEnemy> ();
            playerInTerritory = false;
            territory = gameObject.GetComponent(typeof(Collider)) as Collider;
        }
     
        // Update is called once per frame
        void Update()
        {
            // Debug.Log("EnemyTerritory update");


            // if (playerInTerritory == true)
            // {
            //     basicenemy.attacking = true;
            // }
 
            // if (playerInTerritory == false)
            // {
            //     basicenemy.attacking = false;
            // }
        }
 
        void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Player entered territory");

            if (other.gameObject == player)
            {
                basicenemy.attacking = true;
            }
        }
     
        void OnTriggerExit(Collider other)
        {
            //Debug.Log("Player exited territory");

            if (other.gameObject == player) 
            {
                basicenemy.attacking = false;
            }
        }
}