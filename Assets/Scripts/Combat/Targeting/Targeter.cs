using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup targetGroup;

    public Target CurrentTarget { get; private set; }

    private List<Target> targets = new List<Target>();
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target)) { return; }

        targets.Add(target);
        target.OnTargetDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target)) { return; }

        RemoveTarget(target);
    }

    public bool SetTarget()
    {
        if(targets.Count == 0) { return false; }

        Target closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach(Target target in targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            if(viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) { continue; }

            float distanceToCenter = Vector2.Distance(viewPos, new Vector2(.5f, .5f));

            if(distanceToCenter < closestDistance)
            {
                closestTarget = target;
                closestDistance = distanceToCenter;
            }
        }

        if(closestTarget == null)
        {
            return false;
        }

        CurrentTarget = closestTarget;
        targetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    public void Cancel()
    {
        if(CurrentTarget == null) { return; }

        targetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target)
        {
            CurrentTarget = null;
            targetGroup.RemoveMember(target.transform);
        }

        targets.Remove(target);
        target.OnTargetDestroyed -= RemoveTarget;
    }
}
