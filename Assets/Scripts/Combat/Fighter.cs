using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float weaponRange = 2f;

        Transform target;

      
        private void Update() 
        {
            bool isRange = Vector3.Distance(transform.position, target.position) < weaponRange;
            if(target != null && !isRange)
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Stop();
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
           target =  combatTarget.transform;
        }
    }
}
