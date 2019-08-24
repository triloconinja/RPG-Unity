using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        Health target;
        float timeSinceLastAttack = 0;
      
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;

            if(target.IsDead()) return;

            if (!GetIsRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();

            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            // enemy rotates and face the user;
            target.transform.LookAt(transform);
            if(timeSinceLastAttack > timeBetweenAttacks){

                // This will trigger Hit() event
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
              
            }
            
        }

        //Animation event
        void Hit()
        {
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
           GetComponent<ActionScheduler>().StartAction(this);
           target =  combatTarget.GetComponent<Health>();
        }

        public void Cancel(){

            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }

        
    }
}
