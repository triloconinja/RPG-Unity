using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        
        private void Start() {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            //Terrain();
            if (health.IsDead()) return;
            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;
          
        }

        private void Terrain(){
            //ray starts at player position and points down
            Ray ray = new Ray(transform.position, Vector3.down);

            //will store info of successful ray cast
            RaycastHit hitInfo;

            //terrain should have mesh collider and be on custom terrain 
            //layer so we don't hit other objects with our raycast
            LayerMask layer = 1 << LayerMask.NameToLayer("Terrain");

            //cast ray
            if (Physics.Raycast(ray, out hitInfo, layer))
            {
                //get where on the z axis our raycast hit the ground
                float z = hitInfo.point.z;

                //copy current position into temporary container
                Vector3 pos = transform.position;

                //change z to where on the z axis our raycast hit the ground
                pos.z = z;

                //override our position with the new adjusted position.
                transform.position = pos;
                print(pos);
            }
            print("Terrain");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits){
               CombatTarget target =  hit.transform.GetComponent<CombatTarget>();

               if(target == null) continue;

               GameObject targetGameObject = target.gameObject;
               if(!GetComponent<Fighter>().CanAttack(target.gameObject)){

                   continue;
               }

                if(Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0)){
                GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}