using Assets.Scripts.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CustomYieldInstructions
{
    public class WaitUntilGameOver : CustomYieldInstruction
    {
        private bool isKeepWaiting = true;
        public WaitUntilGameOver(CharacterComponent playerCharacters, CharacterComponent enemyCharacters)
        {

        }


        public override bool keepWaiting
        {
            get { return isKeepWaiting; }
        }
    }
}
