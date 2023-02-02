using UnityEngine;

public class Walking:State{

    private PlayerController controller;

    public Walking(PlayerController controller) : base("Walking")
    {
       this.controller=controller; 
    }


    public override void Enter()
    {
        base.Enter();
        // Debug.Log("Entrou no Walking");
    }

    public override void Exit()
        {
            base.Exit();
            // Debug.Log("Saiu do Walking");
        }

    public override void Update()
        {
            base.Update();

            if(controller.hasJumpInput){

            controller.stateMachine.ChangeState(controller.jumpState);
            return;
            }

            if(controller.movementVector.IsZero()){

            controller.stateMachine.ChangeState(controller.idleState);
            return;
            }



            
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

        }
    
    public override void FixedUpdate()
        {
            base.FixedUpdate();
            Vector3 walkVector= new Vector3(controller.movementVector.x,0,controller.movementVector.y);
            Camera camera=Camera.main;
            walkVector = controller.GetForward()*walkVector;
            walkVector*=controller.movementSpeed;

            // controller.transform.Translate(walkVector);
            controller.thisRigidBody.AddForce(walkVector, ForceMode.Force);
            controller.RotateBodyToFaceInput();


}
}