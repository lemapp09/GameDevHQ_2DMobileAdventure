using System;
using System.Collections;
using System.Collections.Generic;
using LemApperson_2D_Mobile_Adventure;
using UnityEngine;

namespace LemApperson_2D_Mobile_Adventure.Enemy
{

    public class AcidEffect : MonoBehaviour
    {
        // move right at 3 meters per second
        [SerializeField] private float moveSpeed = 3f;
        // detect Player and deal dame (IDamageable Interface)
        // Destroy this after 5 seconds
        
        private void Start()
        {
            Destroy(gameObject, 5f);
        }

        void Update()
        {
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.right); // move right
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<IDamageable>()?.Damage(); // call Damage() from IDamageable Interface.
                Destroy(this.gameObject);
            }
        }

    }
}