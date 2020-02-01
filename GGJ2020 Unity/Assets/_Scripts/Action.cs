
using System;
using UnityEngine;

public enum ActionTypes
{
    DrinkWater,
    SitDown,
    Exit
}

public abstract class Action
{
    public static int noOfActionTypes
    {
        private set { }
        get
        {
            return Enum.GetNames(typeof(ActionTypes)).Length;
        }
    }

    protected Human actionTaker;

    public abstract void StartAction();

    public abstract bool IsActionFinished();

    public abstract void CheckForAction();

    public abstract void ResetAction();

    public void SetActionTaker(Human human)
    {
        actionTaker = human;
    }
}

public class DrinkWater : Action
{
    Transform waterSource;
    float drinkTime = 2.0f;
    bool playedAnimation = false;
    bool endedAction = false;

    public override void CheckForAction()
    {
        if (endedAction)
            return;

        if (playedAnimation)
        {
            if(drinkTime >= 0)
            {
                drinkTime -= Time.deltaTime;
            }
            else
            {
                endedAction = true;
                actionTaker.FreeRotation();
            }
            return;
        }

        if (Vector3.SqrMagnitude(waterSource.position - actionTaker.transform.position) < 2.0f)
        {
            actionTaker.PlayAnimation("Drink Water");
            playedAnimation = true;
            actionTaker.SetRotation(Quaternion.LookRotation(waterSource.position - actionTaker.transform.position));
        }
    }

    public override bool IsActionFinished()
    {
        return endedAction;
    }

    public override void ResetAction()
    {
        drinkTime = 2.0f;
        playedAnimation = false;
        endedAction = false;
    }

    public override void StartAction()
    {
        waterSource = FacilitiesManager.instance.ReturnNearest(Facility.WaterSource, actionTaker.transform.position);
        actionTaker.GoToPosition(waterSource.position + waterSource.forward);
    }
}

public class SitDown : Action
{
    Transform chair;
    float sitTime = 1.0f;
    bool playedAnimation = false;
    bool endedAction = false;

    public override void CheckForAction()
    {
        if (endedAction)
            return;

        if (playedAnimation)
        {
            if (sitTime >= 0)
            {
                sitTime -= Time.deltaTime;
            }
            else
            {
                endedAction = true;
                actionTaker.FreeRotation();
            }
            return;
        }

        if (Vector3.SqrMagnitude(chair.position - actionTaker.transform.position) < 2.0f)
        {
            actionTaker.PlayAnimation("Sit Down");
            playedAnimation = true;
            actionTaker.SetRotation(chair.rotation);
        }
    }

    public override bool IsActionFinished()
    {
        return endedAction;
    }

    public override void ResetAction()
    {
        sitTime = 1.0f;
        playedAnimation = false;
        endedAction = false;
    }

    public override void StartAction()
    {
        chair = FacilitiesManager.instance.ReturnNearest(Facility.Chair, actionTaker.transform.position);
        actionTaker.GoToPosition(chair.position + chair.forward);
    }
}

public class ExitAction : Action
{
    Transform exitLoc;
    bool actionEnded = false;

    public override void CheckForAction()
    {
        if (Vector3.SqrMagnitude(exitLoc.position - actionTaker.transform.position) < 1.0f)
        {
            actionTaker.StopHumanActions();
            actionTaker.gameObject.SetActive(false);
            actionEnded = true;
        }
    }

    public override bool IsActionFinished()
    {
        return actionEnded;
    }

    public override void ResetAction()
    {
        actionEnded = false;
    }

    public override void StartAction()
    {
        exitLoc = FacilitiesManager.instance.ReturnNearest(Facility.ExitLoc, actionTaker.transform.position);
        actionTaker.GoToPosition(exitLoc.position);
    }
}