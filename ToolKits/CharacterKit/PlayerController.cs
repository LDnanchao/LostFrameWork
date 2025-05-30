
using UnityEngine;

namespace Lost.Character
{
    public class PlayerController:MonoBehaviour
    {
        public CharacterControllerBase characterController;
        void Awake()
        {
            
        }

        void Update()
        {
            characterController.Move(new Vector3(Input.GetAxis("Horizontal") ,Input.GetAxis("Vertical") ,0));

        }
    }
}