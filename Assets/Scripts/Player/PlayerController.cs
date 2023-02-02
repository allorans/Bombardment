using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed=10;

    public float jumpPower=10;

    public float jumpMovementFactor = 1f;

    [HideInInspector] public StateMachine stateMachine;
 
    [HideInInspector] public Idle idleState;

     [HideInInspector] public Walking walkingState;

     [HideInInspector] public Jump jumpState;

     [HideInInspector] public Dead deadState;


    [HideInInspector] public Vector2 movementVector;

    [HideInInspector] public bool hasJumpInput;
    
    [HideInInspector] public bool isGrounded;

    [HideInInspector] public Collider thisCollider;

    [HideInInspector] public Rigidbody thisRigidBody;

    [HideInInspector] public Animator thisAnimator;
   void Awake() {
     thisRigidBody=GetComponent<Rigidbody>();
     thisAnimator=GetComponent<Animator>(); 
     thisCollider=GetComponent<Collider>();
    }
        // Start is called before the first frame update
    void Start()
    {
        stateMachine= new StateMachine();
        idleState= new Idle(this);
        walkingState= new Walking(this);
        jumpState=new Jump(this);
        deadState=new Dead(this);
        stateMachine.ChangeState(idleState);
        
    }

    // Update is called once per frame
    void Update()
    {

          if (GameManager.Instance.isGameOver){

            if(stateMachine.currentStateName != deadState.name){
                stateMachine.ChangeState(deadState);

            }

            }


       bool isUp = Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow);
       bool isLeft = Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow);
       bool isRight = Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow);
       bool isDown = Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow);
    
       float inputX = isRight ? 1 : isLeft ? -1 : 0;
       float inputY = isUp ? 1 : isDown ? -1 : 0;
       
       movementVector = new Vector2(inputX,inputY);
       
       hasJumpInput=Input.GetKey(KeyCode.Space);
       
       float velocity=thisRigidBody.velocity.magnitude;

       float velocityRate = velocity/movementSpeed;

       thisAnimator.SetFloat("fVelocity", velocityRate);

          

       DetectGround();
       
       stateMachine.Update();
        
    }

    void LateUpdate()
        {
        stateMachine.LateUpdate();

        }
    
    void FixedUpdate()
        {
        stateMachine.FixedUpdate();
        }

    //  void OnGUI(){
    //     Rect rect= new Rect(5,5,200,50);
    //     string text=stateMachine.currentStateName;
    //     GUIStyle style= new GUIStyle();
    //     style.fontSize=(int)(70f *(Screen.width/1920f));
    //     GUI.Label(rect,text,style);
    // }

    public Quaternion GetForward(){
         Camera camera=Camera.main;
         float eulerY = camera.transform.eulerAngles.y;
         return Quaternion.Euler(0,eulerY,0);
    }

    public void RotateBodyToFaceInput(){
        if(movementVector.IsZero()) return;
        Camera camera=Camera.main;
        Vector3 inputVector= new Vector3(movementVector.x,0,movementVector.y);
        Quaternion q1= Quaternion.LookRotation(inputVector,Vector3.up);
        Quaternion q2= Quaternion.Euler(0,camera.transform.eulerAngles.y,0);
        Quaternion toRotation = q1*q2;
        Quaternion newRotation = Quaternion.LerpUnclamped(transform.rotation, toRotation,0.15f);

        thisRigidBody.MoveRotation(newRotation);

    }

    private void DetectGround(){

        isGrounded=false;

        Vector3 origin =transform.position;
        Vector3 direction =Vector3.down;
        Bounds bounds=thisCollider.bounds;
        float radius=bounds.size.x * 0.33f;
        float maxDistance=bounds.size.y * 0.25f;
        if(Physics.SphereCast(origin, radius, direction,out var hitInfo,maxDistance)){
            GameObject hitObject= hitInfo.transform.gameObject;
            if(hitObject.CompareTag("Platform")){
                isGrounded=true;
            }
        }

    }


    // void OnDrawGizmos(){

    //     if(!thisCollider) return;

    //     Vector3 origin =transform.position;
    //     Vector3 direction =Vector3.down;
    //     Bounds bounds=thisCollider.bounds;
    //     float radius=bounds.size.x * 0.33f;
    //     float maxDistance=bounds.size.y * 0.25f;
        

    //     Gizmos.color=Color.yellow;
    //     Gizmos.DrawRay(new Ray(origin,direction*maxDistance));


    //     Gizmos.color=Color.gray;
    //     Gizmos.DrawSphere(origin, 0.1f);

        
    //     Vector3 spherePosition= direction * maxDistance + origin;
    //     Gizmos.color=isGrounded?Color.magenta:Color.cyan;
    //     Gizmos.DrawSphere(spherePosition, radius);
    // }


}
