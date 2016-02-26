using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
//using UnityStandardAssets.Characters.ThirdPerson;

namespace Swordsmanship
{
    [RequireComponent(typeof (SwordsmanCharacter))]
    public class SwordsmanControl : MonoBehaviour
    {
        private SwordsmanCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.


        //private MouseInputStruct mouseInput;

        


        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<SwordsmanCharacter>();


            // init mouseInput
            //mouseInput = new MouseInputStruct();

            MouseBehavior.mouseInputDelegate += MouseInputHandle;
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButtonDown("Jump");
                if(m_Jump)
                {
                    Debug.Log(m_Jump);
                }
                
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Horizontal");
            float v = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);


            // DELETE ME: Test input
            

            //if(Input.GetKey(KeyCode.B))
            //{
            //    mouseInput.mouseMovementInput = MouseMovementsInput.Block_Front;
            //}
            //else
            //{
            //    mouseInput.mouseMovementInput = MouseMovementsInput.Block_Exit;
            //}


            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    mouseInput.mouseMovementInput = MouseMovementsInput.Attack_SwingLeftIdle;
            //}

            //if (Input.GetKeyDown(KeyCode.X))
            //{
            //    mouseInput.mouseMovementInput = MouseMovementsInput.Attack_SwingLeft;
            //}

            /////////////////////////


            //attack
            //Attack();
            


            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }


        private void MouseInputHandle(MouseInputStruct m_input)
        {

            switch(m_input.mouseMovementInput)
            {
                case MouseMovementsInput.Idle:
                    m_Character.IdleClearStates();
                    break;
                case MouseMovementsInput.Attack_SwingLeftIdle:
                    m_Character.AttackSwingLeftIdle();
                    break;
                case MouseMovementsInput.Attack_SwingLeft:
                    m_Character.AttackSwingLeftAttack();
                    break;
                case MouseMovementsInput.Attack_SwingRightIdle:
                    m_Character.AttackSwingRightIdle();
                    break;
                case MouseMovementsInput.Attack_SwingRight:
                    m_Character.AttackSwingRightAttack();
                    break;
                case MouseMovementsInput.Attack_StabIdle:
                    m_Character.AttackStabIdle();
                    break;
                case MouseMovementsInput.Attack_Stab:
                    m_Character.AttackStabAttack();
                    break;

                case MouseMovementsInput.Block_Front:
                    m_Character.BlockFront();
                    break;
                case MouseMovementsInput.Block_Left:
                    m_Character.BlockLeft();
                    break;
                case MouseMovementsInput.Block_Right:
                    m_Character.BlockRight();
                    break;

                case MouseMovementsInput.Block_Exit:
                    m_Character.BlockExit();
                    break;

                default:
                    break;
            }

        }


    }
}
