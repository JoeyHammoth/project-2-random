using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using System.Collections;
using System.Xml.Serialization;
using Assets.Scripts;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Camera playerCamera;

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float baseSpeed = 2.0f;

    [SerializeField] private float currentSpeed;
    [SerializeField] private float staminaSpeedIncrease = 1.0f;


    private Plane gamePlane;
    private Vector3 aimDirection;
    private Vector3 moveDirection;

    private CharacterController controller;
    private PlayerHealthManager healthManager;
    private PlayerStaminaManager staminaManager;
    private Animator animator;


    private float velocityZ = 0.0f;
    private float velocityX = 0.0f;
    private float acceleration = 2.0f;
    private float maxWalkVelocity = 1.0f;


    private float timer = 0.0f;
    private int mushroomCount = 0;

    
    public enum GameState
    {
        Playing,
        GameOver
    }
    public GameState currentState = GameState.Playing;

    // Start is called before the first frame update
    void Start()
    {
        this.gamePlane = new Plane(Vector3.up, Vector3.zero);
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        healthManager = GetComponentInChildren<PlayerHealthManager>();
        staminaManager = GetComponentInChildren<PlayerStaminaManager>();
        currentSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (healthManager.GetHealth() <= 0) return;
        
        if(currentState == GameState.Playing)
        {
            Aim();
            Move();
            Animation();
        }
        
    }

    void FixedUpdate()
    {
        if(currentState == GameState.Playing)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= 1.0f)
            {
                // Mushroom effect
                if (moveDirection == Vector3.zero)
                {
                    StartCoroutine(MushroomRecover());
                }

                timer = 0.0f;
            }
        }
    }


    private void Aim() {
        var ray = this.playerCamera.ScreenPointToRay(Input.mousePosition);

        if (this.gamePlane.Raycast(ray, out var distance)) {
            var target = ray.GetPoint(distance);
            target.y = transform.position.y;
            this.aimDirection = (target - this.playerCamera.transform.position).normalized;

            this.transform.LookAt(target);
        }
    }
    
    private void Move() {
        
        moveDirection = Vector3.zero;

        // relative to world position
        if (Input.GetKey(KeyCode.W)) {
            moveDirection += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveDirection += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveDirection += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveDirection += Vector3.back;
        }
        if (Input.GetKey(KeyCode.LeftShift) && moveDirection != Vector3.zero) {
            if (staminaManager.GetStamina() > 3) {
                currentSpeed = baseSpeed + staminaSpeedIncrease;
                staminaManager.Run();
            } else {
                currentSpeed = baseSpeed;
            }
        } else {
            currentSpeed = baseSpeed;
        }

        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        float animatorSpeed = currentSpeed / baseSpeed;

        animator.speed = animatorSpeed;

        Animation();
    }

    private void Animation() {
    Vector3 localMoveDirection = transform.InverseTransformDirection(moveDirection);

    // Acceleration for all directions
    velocityZ = Mathf.MoveTowards(velocityZ, localMoveDirection.z * maxWalkVelocity, acceleration * Time.deltaTime);
    velocityX = Mathf.MoveTowards(velocityX, localMoveDirection.x * maxWalkVelocity, acceleration * Time.deltaTime);

    if (Math.Abs(velocityX) <= 0.003f && Math.Abs(velocityZ) <= 0.003f) {
        velocityZ = 0.0f;
        velocityX = 0.0f;
    }

    animator.SetFloat("Velocity Z", velocityZ);
    animator.SetFloat("Velocity X", velocityX);
    }

    public float getCurrentSpeed() {
        return currentSpeed;
    }

    public void setCurrentSpeed(float newSpeed) {
        currentSpeed = newSpeed;
    }

    public float getBaseSpeed() {
        return baseSpeed;
    }

    public void setBaseSpeed(float newSpeed) {
        baseSpeed = newSpeed;
    }

    public void addMushroom() {
        mushroomCount++;
    }

    private IEnumerator MushroomRecover() {
        yield return new WaitForSeconds(1);
        healthManager.NotMovingRecoverHealth(mushroomCount);
    }

}
