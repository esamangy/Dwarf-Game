using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Unity.VisualScripting;
using System;

public class PlayerController : Entity{
    //References----------------------------
    [Header("References")]
    [SerializeField] private GameObject body;
    private CharacterController controller;
    //--------------------------------------
    [Header("Camera")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private CinemachineFreeLook cinemachineCam;
    [SerializeField] private Vector3 cameraBaseRadius;
    private float currentZoom = 1;
    [SerializeField] private float cameraZoomSensitivity;
    [SerializeField] private Vector2 minAndMaxZoom;
    //Movement------------------------------
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Vector3 moveVal;
    private bool isSprinting = false;
    [SerializeField] private float sprintModifier;
    [SerializeField] private float sprintStaminaDrainDuration;
    [SerializeField] private int sprintStaminaDrainAmount;
    [SerializeField] private float staminaRegenDuration;
    [SerializeField] private float staminaRegenDelay;
    [SerializeField] private int staminaRegenAmount;
    private float timeUntilNextStaminaRegen;
    private float lastStaminaUse;
    private float timeSprinting;
    private float gravity = -9.8f;
    [SerializeField] private float jumpHeight;
    [SerializeField] private int jumpStaminaDrainAmount;
    //--------------------------------------

    void Awake(){
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start(){
        controller = GetComponent<CharacterController>();
        
    }

    void FixedUpdate(){
        Movement();
        updateStamina();
    }

    private void Movement(){
        if(!controller.isGrounded){
            moveVal.z = moveVal.z + (gravity * Time.deltaTime);
        }
        Quaternion orientation = mainCam.transform.rotation;
        Quaternion target = Quaternion.Euler(0, orientation.eulerAngles.y, 0);
        body.transform.rotation = target;

        Vector3 moveDir = body.transform.forward * moveVal.y * moveSpeed + body.transform.right * moveVal.x  * moveSpeed+ body.transform.up * moveVal.z;
        controller.Move(moveDir * Time.deltaTime);
        body.transform.position = controller.transform.position;

        if(controller.isGrounded){
            moveVal.z = 0;
        }
    }

    private void updateStamina(){
        if(isSprinting){
            if(timeSprinting <= 0){
                SpendStamina(sprintStaminaDrainAmount);
                timeSprinting = sprintStaminaDrainDuration;
            }
        }
        
        if(isSprinting){
            timeSprinting -= Time.deltaTime;
        }
        timeUntilNextStaminaRegen -= Time.deltaTime;
    }

    private void OnMove(InputValue value){
        moveVal = value.Get<Vector2>();
    }

    private void OnCameraZoom(InputValue value){
        currentZoom = Math.Clamp(currentZoom - (value.Get<Vector2>().y / cameraZoomSensitivity), minAndMaxZoom.x, minAndMaxZoom.y);
        for(int i = 0; i < 3; i ++){
            cinemachineCam.m_Orbits[i].m_Radius = cameraBaseRadius[i] * currentZoom;
            if(i == 0){
                cinemachineCam.m_Orbits[i].m_Height = cameraBaseRadius[i] * currentZoom;
            }
        }

    }

    private void OnJump(InputValue value){
        if(this.stamina < jumpStaminaDrainAmount){
            return;
        }
        RaycastHit hit;
        Vector3 bottom = new Vector3(transform.position.x, transform.position.y - (controller.height / 2), transform.position.z);
        Physics.Raycast(bottom, Vector3.down, out hit, Mathf.Infinity);
        if(hit.distance <= .001){
            moveVal.z = jumpHeight;
        }
        SpendStamina(jumpStaminaDrainAmount);
    }
    private void sprint(){
    }
    private void OnSprint(InputValue value){
        if(value.isPressed && this.stamina > sprintStaminaDrainAmount){
            moveSpeed *= sprintModifier;
            isSprinting = true;
        } else if(!value.isPressed) {
            moveSpeed /= sprintModifier;
            isSprinting = false;
        }
    }

    private void OnFire(InputValue value){
        Debug.Log(this.ToString());
    }

    //entity implementation
    public override void Hurt(int damage){
        health -= damage;
    }
    public override void Heal(int amount){
        this.health += amount;
        if(this.health > maxHealth){
            this.health = maxHealth;
        }
    }
    public override void SpendMana(int amount){
        this.mana -= amount;
    }
    public override void RestoreMana(int amount){
        this.mana += amount;
        if(this.mana > maxMana){
            this.mana = maxMana;
        }
    }
    public override void SpendStamina(int amount){
        lastStaminaUse = Time.time;
        this.stamina -= amount;
    }
    public override void RestoreStamina(int amount){
        this.stamina += amount;
        if(this.stamina > maxStamina){
            this.stamina = maxStamina;
        }
    }

    public override string ToString(){
        return base.ToString("The Player Stats\n");
    }

}
