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
    //Camera--------------------------------
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
    [Tooltip("The amount of stamina regained per second")]
    [SerializeField] private float staminaRegenspeed;
    [SerializeField] private float staminaRegenDelay;
    [SerializeField] private int staminaRegenAmount;
    private float timeUntilNextStaminaRegen;
    private float lastStaminaUse;
    private float timeSprinting;
    private float gravity = -9.8f;
    [SerializeField] private float jumpHeight;
    [SerializeField] private int jumpStaminaDrainAmount;
    //Mana----------------------------------
    [Header("Mana")]
    [Tooltip("The amount of stamina regained per second")]
    [SerializeField] private float manaRegenspeed;
    [SerializeField] private float manaRegenDelay;
    private float lastManaUse;
    //Mana----------------------------------
    [Header("Gameplay")]
    [SerializeField] private float reachDistance;
    

    void Awake(){
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start(){
        controller = GetComponent<CharacterController>();
        
    }

    void Update(){
        updateSprint();
        updateStamina();
        Movement();
        updateMana();
        checkForInteractabes();
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
            } else {
                timeSprinting -= Time.deltaTime;
            }
        }

        if(this.stamina == this.maxStamina){
            return;
        }

        if((Time.time - lastStaminaUse) >= staminaRegenDelay){
            if(timeUntilNextStaminaRegen <= 0){
                RestoreStamina(staminaRegenAmount);
                timeUntilNextStaminaRegen = (.1000f / staminaRegenspeed);
            } else {
                timeUntilNextStaminaRegen -= Time.deltaTime;
            }
        }
        
    }

    private void updateSprint(){
        if(isSprinting){
            if(this.stamina < sprintStaminaDrainAmount){
                isSprinting = false;
                moveSpeed /= sprintModifier;
            }
        }
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
        RaycastHit[] hits = new RaycastHit[5];
        Vector3 bottom = new Vector3(transform.position.x, transform.position.y - (controller.height / 2), transform.position.z);
        Physics.Raycast(bottom, Vector3.down, out hits[0], Mathf.Infinity);
        Physics.Raycast(bottom + new Vector3(.5f, controller.stepOffset ,0f), Vector3.down, out hits[1], Mathf.Infinity);
        Physics.Raycast(bottom + new Vector3(-.5f,controller.stepOffset,0f), Vector3.down, out hits[2], Mathf.Infinity);
        Physics.Raycast(bottom + new Vector3(0f,controller.stepOffset,.5f), Vector3.down, out hits[3], Mathf.Infinity);
        Physics.Raycast(bottom + new Vector3(0f,controller.stepOffset,-.5f), Vector3.down, out hits[4], Mathf.Infinity);
        for(int i = 0; i < hits.Length; i ++){
            if(hits[i].distance - controller.stepOffset <= .1f){
                Debug.Log("jump");
                moveVal.z = jumpHeight;
                SpendStamina(jumpStaminaDrainAmount);
            }
        }
    }
    
    private void OnSprint(InputValue value){
        if(!value.isPressed && isSprinting){
            moveSpeed /= sprintModifier;
        } else if(value.isPressed && !isSprinting) {
            moveSpeed *= sprintModifier;
        }
        isSprinting = value.isPressed;
    }

    private void OnFire(InputValue value){
        //Debug.Log(this.ToString());
    }

    private void updateMana(){
        if((Time.time - lastManaUse) >= manaRegenDelay){
            RestoreMana(manaRegenspeed * Time.deltaTime);
        }
    }

    private void OnInteract(){
        RaycastHit hit = drayRayToMiddleOfScreen();
        if(hit.collider){
            if(!hit.collider.GetComponent<Interactable>()){
                return;
            }
            if(Vector3.Distance(hit.collider.transform.position, this.transform.position) <= reachDistance){
                hit.collider.gameObject.GetComponent<Interactable>().interact();
            }
        }
    }

    private RaycastHit drayRayToMiddleOfScreen(){
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
        return hit;
    }

    private void checkForInteractabes(){
        RaycastHit hit = drayRayToMiddleOfScreen();
        if(hit.collider){
            if(!hit.collider.GetComponent<Interactable>()){
                return;
            }
            if(Vector3.Distance(hit.collider.transform.position, this.transform.position) <= reachDistance){
                hit.collider.gameObject.GetComponent<Interactable>().Highlight();
            }
        }
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
    public override void SpendMana(float amount){
        lastManaUse = Time.time;
        this.mana -= amount;
    }
    public override void RestoreMana(float amount){
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
