using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;
using UnityEngine.Events;

public class PlayerController : Entity{
    //References----------------------------
    [Header("References")]
    [SerializeField] private GameObject body;
    private CharacterController controller;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PlayerHUD hud;
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
    [SerializeField] private float regularMoveSpeed;
    [SerializeField] private float accelSpeed;
    [SerializeField] private float decelSpeed;
    private float curMaxSpeed;
    private Vector3 targetMove;
    private Vector2 moveVal;
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
    [SerializeField] private float DashCooldownTime;
    private float lastDash;
    [SerializeField] private float DashDistance;
    [SerializeField] private int DashStaminaUse;
    //Mana----------------------------------
    [Header("Mana")]
    [Tooltip("The amount of mana regained per second")]
    [SerializeField] private float manaRegenspeed;
    [SerializeField] private float manaRegenDelay;
    private float lastManaUse;
    //Gameplay----------------------------------
    [Header("Gameplay")]
    [SerializeField] private float reachDistance;
    //events----------------------------------
    [Header("Events")]
    public UnityEvent statusChanged;

    public override void Awake(){
        base.Awake();
        Cursor.lockState = CursorLockMode.Locked;
        targetMove = new Vector3();
        lastDash = Time.time;
        curMaxSpeed = regularMoveSpeed;

        statusChanged = new UnityEvent();
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
        updateRotation();
        checkGravity();
        if(moveVal.x == 0){
            targetMove.x = moveLerp(targetMove.x, 0f);
        } else  if(moveVal.x > 0){
            targetMove.x = moveLerp(targetMove.x, curMaxSpeed);
        } else {
            targetMove.x = moveLerp(targetMove.x, -curMaxSpeed);
        }
        if(moveVal.y == 0){
            targetMove.z = moveLerp(targetMove.z, 0f);
        } else  if(moveVal.y > 0){
            targetMove.z = moveLerp(targetMove.z, curMaxSpeed);
        } else {
            targetMove.z = moveLerp(targetMove.z, -curMaxSpeed);
        }  
         
        Vector3 moveDir = body.transform.forward * targetMove.z + body.transform.right * targetMove.x + body.transform.up * targetMove.y;
        controller.Move(moveDir * Time.deltaTime);
        body.transform.position = controller.transform.position;
    }

    private float moveLerp(float curSpeed, float targetSpeed){
        float toReturn = curSpeed;
        if(curSpeed < targetSpeed){
            toReturn = Mathf.Clamp(toReturn + accelSpeed * Time.deltaTime, curSpeed, targetSpeed);
        } else {
            toReturn = Mathf.Clamp(toReturn - decelSpeed * Time.deltaTime, targetSpeed, curSpeed);
        }
        return toReturn;
    }

    private void checkGravity(){
        if(!controller.isGrounded){
            targetMove.y = targetMove.y + (gravity * Time.deltaTime);
        } else {
            targetMove.y = 0;
        }
    }
    private void updateRotation(){
        Quaternion orientation = mainCam.transform.rotation;
        Quaternion target = Quaternion.Euler(0, orientation.eulerAngles.y, 0);
        body.transform.rotation = target;
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
                regularMoveSpeed /= sprintModifier;
            }
        }
    }

    private void OnMove(InputValue value){
        moveVal = value.Get<Vector2>().normalized;
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

    private void OnDash(InputValue value){
        if(Time.time - lastDash < DashCooldownTime){
            Debug.Log(Time.time - lastDash);
            Debug.Log("too soon to dash");
            return;
        }
        if(stamina < DashStaminaUse){
            Debug.Log("not enough stamina to dash");
            return;
        }
        Vector3 moveDir = body.transform.forward * moveVal.y * DashDistance + body.transform.right * moveVal.x  * DashDistance + body.transform.up * targetMove.y;
        controller.Move(moveDir);
        lastDash = Time.time;
        SpendStamina(DashStaminaUse);
    }
    
    private void OnSprint(InputValue value){
        if(!value.isPressed && isSprinting){
            curMaxSpeed = regularMoveSpeed;
        } else if(value.isPressed && !isSprinting) {
            curMaxSpeed = regularMoveSpeed * sprintModifier;
        }
        isSprinting = value.isPressed;
    }

    private void OnPrimary(InputValue value){
        Debug.Log("Primary");
        Hurt(5);
    }
    private void OnSecondary(InputValue value){
        Debug.Log("Secondary");
        Heal(5);
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

    public PlayerHUD getPlayerHUD(){
        return hud;
    }

    //entity implementation
    public override void Hurt(int damage){
        health -= damage;
        statusChanged.Invoke();
    }
    public override void Heal(int amount){
        this.health += amount;
        if(this.health > maxHealth){
            this.health = maxHealth;
        }
        statusChanged.Invoke();
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
        statusChanged.Invoke();
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
        statusChanged.Invoke();
    }

    public override string ToString(){
        return base.ToString("The Player Stats\n");
    }

}
