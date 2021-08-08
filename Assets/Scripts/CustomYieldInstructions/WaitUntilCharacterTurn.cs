using Assets.Scripts.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CustomYieldInstructions
{
    public class WaitUntilCharacterTurn : CustomYieldInstruction
    {
        private bool isKeepWaiting = true;
        public WaitUntilCharacterTurn(CharacterComponent character)
        {
            Action action = null;
            action = () =>
            {
                isKeepWaiting = false;
                character.AttackComponent.OnAttackFinished -= action;
            };

            character.AttackComponent.OnAttackFinished += action;
        }

        public override bool keepWaiting
        {
            get { return isKeepWaiting; }
        }
    }
}
