using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyFarm
{
    public class PlayerController : MonoBehaviour
    {
        GameManager _gameManager;
        Rigidbody rigidbody;
        Animator animator;
        Vector3 targetPosition;
        bool positionReached;
        float speed, animationSpeed;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            targetPosition = transform.position;
            positionReached = false;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetTarget();
                speed = _gameManager.playerWalkSpeed;
                animationSpeed = GameManager.KAnimationWalkSpeed;
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                SetTarget();
                speed = _gameManager.playerRunSpeed;
                animationSpeed = GameManager.KAnimationRunSpeed;
            }
        }

        void FixedUpdate()
        {
            if (!positionReached)
            {
                var movement = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                rigidbody.MovePosition(movement);

                Vector3 direction = targetPosition - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, speed * Time.deltaTime);

                animator.SetFloat("Speed_f", animationSpeed);
            }

            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                animator.SetFloat("Speed_f", 0f);
                positionReached = true;
            }
        }

        private void SetTarget()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                positionReached = false;
            }
        }
    }

}