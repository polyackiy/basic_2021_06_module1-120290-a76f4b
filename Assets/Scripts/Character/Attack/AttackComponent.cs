using Assets.Scripts.Character.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Character.Attack
{
    public class AttackComponent : MonoBehaviour
    {
        [SerializeField] private int damage;
        public int Damage { get => damage; }

        public Action<int> OnDamageChanged;
        public Action OnAttackFinished;
        public string soundName;

        private PlaySound playSound;

        private void Start()
        {
            playSound = GetComponentInChildren<PlaySound>();
        }

        public void Attack(HealthComponent healthComponent)
        {
            if (healthComponent.IsDead)
            {
                OnAttackFinished?.Invoke();
                return;
            }
            
            if (playSound) playSound.Play(soundName);
            healthComponent.ApplyDamage(this);
            OnAttackFinished?.Invoke();
        }

    }
}
