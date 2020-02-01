
using System;
using UnityEngine;

public enum ActionTypes
{
    DrinkWater,
    SitDown,
    Exit,
    UseCashier
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

    public abstract bool StartAction();

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
    float drinkTime = 11.0f;
    bool playedAnimation = false;
    bool endedAction = false;

    public override void CheckForAction()
    {
        if (endedAction)
            return;

        if (!waterSource)
            return;

        if (playedAnimation)
        {
            if (drinkTime >= 0)
            {
                drinkTime -= Time.deltaTime;
            }
            else
            {
                waterSource.GetComponent<FacilityObject>().SetFree();
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
        drinkTime = 11.0f;
        playedAnimation = false;
        endedAction = false;
    }

    public override bool StartAction()
    {
        waterSource = FacilitiesManager.instance.ReturnNearest(Facility.WaterSource, actionTaker.transform.position);
        if (waterSource)
        {
            actionTaker.GoToPosition(waterSource.position + waterSource.forward);
            waterSource.GetComponent<FacilityObject>().SetInUse();
            return true;
        }
        return false;
    }
}

public class UseCashier : Action
{
    Transform cashier;
    float useCashierTime = 25.0f;
    bool playedAnimation = false;
    bool endedAction = false;

    public override void CheckForAction()
    {
        if (endedAction)
            return;

        if (!cashier)
            return;

        if (playedAnimation)
        {
            if (useCashierTime >= 0)
            {
                useCashierTime -= Time.deltaTime;
            }
            else
            {
                cashier.GetComponent<FacilityObject>().SetFree();
                endedAction = true;
                actionTaker.FreeRotation();
                actionTaker.PlayAnimation("Stop Thinking");
            }
            return;
        }

        if (Vector3.SqrMagnitude(cashier.position - actionTaker.transform.position) < 2.0f)
        {
            actionTaker.PlayAnimation("Start Thinking");
            playedAnimation = true;
            actionTaker.SetRotation(Quaternion.LookRotation(cashier.position - actionTaker.transform.position));
        }
    }

    public override bool IsActionFinished()
    {
        return endedAction;
    }

    public override void ResetAction()
    {
        useCashierTime = 25.0f;
        playedAnimation = false;
        endedAction = false;
    }

    public override bool StartAction()
    {
        cashier = FacilitiesManager.instance.ReturnNearest(Facility.Cashier, actionTaker.transform.position);
        if (cashier)
        {
            actionTaker.GoToPosition(cashier.position + cashier.forward);
            cashier.GetComponent<FacilityObject>().SetInUse();
            return true;
        }
        return false;
    }
}

public class SitDown : Action
{
    Transform chair;
    float sitTime = 15.0f;
    bool playedAnimation = false;
    bool endedAction = false;

    public override void CheckForAction()
    {
        if (endedAction)
            return;

        if (!chair)
            return;

        if (playedAnimation)
        {
            if (sitTime >= 0)
            {
                sitTime -= Time.deltaTime;
            }
            else
            {
                chair.GetComponent<FacilityObject>().SetFree();
                actionTaker.PlayAnimation("Stand Up");
                endedAction = true;
                actionTaker.FreeRotation();
            }
            return;
        }

        if (Vector3.SqrMagnitude(chair.position - actionTaker.transform.position) < .5f)
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
        sitTime = 15.0f;
        playedAnimation = false;
        endedAction = false;
    }

    public override bool StartAction()
    {
        chair = FacilitiesManager.instance.ReturnNearest(Facility.Chair, actionTaker.transform.position);
        if (chair)
        {
            actionTaker.GoToPosition(chair.position + chair.forward / 6.0f);
            chair.GetComponent<FacilityObject>().SetInUse();
            return true;
        }
        return false;
    }
}

public class ExitAction : Action
{
    Transform exitLoc;
    bool actionEnded = false;

    public override void CheckForAction()
    {
        if (Vector3.SqrMagnitude(exitLoc.position - actionTaker.transform.position) < 2.0f)
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

    public override bool StartAction()
    {
        actionTaker.SetInActive();
        exitLoc = FacilitiesManager.instance.ReturnNearest(Facility.ExitLoc, actionTaker.transform.position);
        actionTaker.GoToPosition(exitLoc.position);
        return true;
    }
}