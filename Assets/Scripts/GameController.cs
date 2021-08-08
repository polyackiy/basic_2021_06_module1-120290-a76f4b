using Assets.Scripts.CustomYieldInstructions;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Character;

public class GameController : MonoBehaviour
{
    [SerializeField] private CharacterComponent[] playerCharacters;
    [SerializeField] private CharacterComponent[] enemyCharacters;
    
    public CanvasGroup victoryMenu;
    public CanvasGroup defeatMenu;
    
    private Coroutine gameLoop;

    public string soundVictory;
    public string soundDefeat;

    private PlaySound playSound;
    
    private void Start()
    {
        gameLoop = StartCoroutine(GameLoop());
        playSound = GetComponentInChildren<PlaySound>();
    }
    private IEnumerator GameLoop()
    {
        Coroutine turn = StartCoroutine(Turn(playerCharacters, enemyCharacters));

        yield return new WaitUntil(() =>
        playerCharacters.FirstOrDefault(c => !c.HealthComponent.IsDead) == null ||
        enemyCharacters.FirstOrDefault(c => !c.HealthComponent.IsDead) == null);

        StopCoroutine(turn);
        GameOver();
    }

    private CharacterComponent GetTarget(CharacterComponent[] characterComponents)
    {
        return characterComponents.FirstOrDefault(c => !c.HealthComponent.IsDead);
    }

    private void GameOver()
    {
        bool isPlayerCharacherAlive = false;
        bool isEnemyCharacherAlive = false;

        bool isVictory;

        for (int i = 0; i < playerCharacters.Length; i++)
        {
            if (!playerCharacters[i].HealthComponent.IsDead)
            {
                isPlayerCharacherAlive = true;
            }
        }

        for (int i = 0; i < enemyCharacters.Length; i++)
        {
            if (!enemyCharacters[i].HealthComponent.IsDead)
            {
                isEnemyCharacherAlive = true;
            }
        }

        isVictory = isPlayerCharacherAlive && !isEnemyCharacherAlive;

        Debug.Log(isVictory ? "Victory" : "Defeat");
        if (isVictory)
        {
            Utility.SetCanvasGroupEnabled(victoryMenu, true);
            if (playSound) playSound.Play(soundVictory);
        }
        else
        {
            Utility.SetCanvasGroupEnabled(defeatMenu, true);
            if (playSound) playSound.Play(soundDefeat);
        }
    }

    private IEnumerator Turn(CharacterComponent[] playerCharacters, CharacterComponent[] enemyCharacters)
    {
        int turnCounter = 0;
        while (true)
        {
            for (int i = 0; i < playerCharacters.Length; i++)
            {
                if(playerCharacters[i].HealthComponent.IsDead)
                {   
                    playerCharacters[i].SetState(CharacterComponent.State.StartDying);
                    Debug.Log("Character: " + playerCharacters[i].name + " is dead");
                    continue;
                }
                playerCharacters[i].SetTarget(GetTarget(enemyCharacters).HealthComponent);
                //TODO: hotfix
                yield return null; // ugly fix need to investigate
                playerCharacters[i].StartTurn();
                yield return new WaitUntilCharacterTurn(playerCharacters[i]);
                Debug.Log("Character: " + playerCharacters[i].name + " finished turn");
            }

            yield return new WaitForSeconds(.5f);

            for (int i = 0; i < enemyCharacters.Length; i++)
            {
                if (enemyCharacters[i].HealthComponent.IsDead)
                {
                    enemyCharacters[i].SetState(CharacterComponent.State.StartDying);
                    Debug.Log("Enemy character: " + enemyCharacters[i].name + " is dead");
                    continue;
                }
                enemyCharacters[i].SetTarget(GetTarget(playerCharacters).HealthComponent);
                enemyCharacters[i].StartTurn();
                yield return new WaitUntilCharacterTurn(enemyCharacters[i]);
                Debug.Log("Enemy character: " + enemyCharacters[i].name + " finished turn");
            }

            yield return new WaitForSeconds(.5f);

            turnCounter++;
            Debug.Log("Turn #" + turnCounter + " has been ended");
        }
    }

}
