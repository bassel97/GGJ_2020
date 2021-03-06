﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Human : MonoBehaviour
{
    private NavMeshAgent navAgent;
    [SerializeField] private Animator anim = null;
    [SerializeField] private Renderer rendererComp = null;
    private CapsuleCollider capsCollider = null;

    [Header("Human Info")]
    [SerializeField] private int noOfActions = 0;
    [SerializeField] private bool isCircular = true;

    [Header("Robber Info")]
    [SerializeField] private GameObject weapon = null;
    [SerializeField] private ParticleSystem weaponEffect = null;
    [SerializeField] private AudioSource weaponAudioEffect = null;
    [SerializeField] private LayerMask attackLayer = 0;
    private bool isPublicAttacker = false;
    private float attackCoolDown = 0;

    [SerializeField] private List<Action> actions = new List<Action>();
    private Action currentAction = null;
    int currActionIndex = 0;

    private Quaternion targetRotation = Quaternion.identity;
    private bool goToRotation = false;

    private bool inActive = false;
    private Vector3 startPos = Vector3.zero;
    private Quaternion startRot = Quaternion.identity;

    private bool isAttacker = false;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        capsCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        currentAction.CheckForAction();
        anim.SetFloat("Walk Speed", navAgent.velocity.sqrMagnitude);

        if (inActive)
            return;

        if (currentAction.IsActionFinished())
        {
            StartNextAction();
        }

        if (goToRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 8.0f);
        }
    }

    public void SetNoOfActions(int noOfActions, bool isCircular = true)
    {
        this.noOfActions = noOfActions;
        this.isCircular = isCircular;
    }

    public void SetUpActions()
    {
        actions.Clear();

        for (int i = 0; i < noOfActions; i++)
        {
            ActionTypes actionType = (ActionTypes)Random.Range(0, Action.noOfActionTypes);
            Action action = null;

            switch (actionType)
            {
                case ActionTypes.DrinkWater:
                    action = new DrinkWater();
                    break;
                case ActionTypes.SitDown:
                    action = new SitDown();
                    break;
                case ActionTypes.Exit:
                    action = new SitDown();
                    break;
                case ActionTypes.UseCashier:
                    action = new UseCashier();
                    break;
                case ActionTypes.StartAttack:
                    action = new SitDown();
                    break;
                default:
                    break;
            }

            action.SetActionTaker(this);
            actions.Add(action);
        }

        Action exitAction = new ExitAction();
        exitAction.SetActionTaker(this);
        actions.Add(exitAction);

        currActionIndex = 0;
    }

    public void StartNextAction()
    {
        Action action = actions[currActionIndex];
        currActionIndex++;
        currActionIndex = currActionIndex % (actions.Count);

        while (!action.StartAction())
        {
            action = actions[currActionIndex];
            currActionIndex++;
            currActionIndex = currActionIndex % (actions.Count);
        }

        currentAction = action;
    }

    public void SetIsAttacker()
    {
        isAttacker = true;
        int lastNormalAction = (int)(actions.Count / 3.0f);
        int attackAt = Random.Range(0, lastNormalAction);

        Action attackAction = new StartAttack();
        attackAction.SetActionTaker(this);

        Action exitAction = new ExitAction();
        exitAction.SetActionTaker(this);

        actions.Insert(lastNormalAction + attackAt, attackAction);
        actions.Insert(lastNormalAction + attackAt + 1, exitAction);
    }

    public void GoToPosition(Vector3 position)
    {
        if (!gameObject.activeSelf)
            return;
        navAgent.SetDestination(position);
    }

    public void PlayAnimation(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void SetRotation(Quaternion rot)
    {
        targetRotation = rot;
        goToRotation = true;
    }

    public void FreeRotation()
    {
        goToRotation = false;
    }

    public void SetInActive()
    {
        inActive = true;
    }

    public void StopHumanActions()
    {
        navAgent.isStopped = true;
    }

    //TODO move robber logic to a child class
    public void KillNonPublicAttackers()
    {

        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
            return;
        }

        attackCoolDown = 0.5f;

        if (Physics.Raycast(transform.position + transform.up * 1.5f, transform.forward, out RaycastHit hit, 50.0f, attackLayer))
        {
            Human attacked = hit.transform.GetComponent<Human>();
            if (!attacked)
            {
                Debug.LogWarning("Not Human ?? " + hit.transform.name);
                return;
            }
            if (attacked.isPublicAttacker)
                return;

            //Debug.Log(name + " attack " + hit.transform.name);
            weaponAudioEffect.Play();
            weaponEffect.Play();
            attacked.KillHuman();
        }
    }

    public void SetStartParams(Vector3 pos, Quaternion rot)
    {
        startPos = pos;
        startRot = rot;
    }

    public void ResetHuman()
    {
        gameObject.SetActive(true);

        inActive = false;

        capsCollider.enabled = true;

        transform.position = startPos;
        transform.rotation = startRot;

        navAgent.Warp(startPos);
        FreeRotation();

        rendererComp.material.color = Color.white;

        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].ResetAction();
        }

        weapon.SetActive(false);

        isPublicAttacker = false;

        anim.SetTrigger("Stand Up");

        currActionIndex = 0;
        StartNextAction();
    }

    public void GetTheGun()
    {
        isPublicAttacker = true;
        weapon.SetActive(true);
        anim.SetTrigger("Got Gun");
        rendererComp.material.color = Color.black;
    }

    public void KillHuman()
    {
        capsCollider.enabled = false;

        anim.SetTrigger("Death");

        navAgent.isStopped = true;

        inActive = true;
    }

    public bool IsPublicAttacker()
    {
        return isPublicAttacker;
    }
}
