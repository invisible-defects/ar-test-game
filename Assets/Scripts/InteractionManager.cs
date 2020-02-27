using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Experimental.XR;
using UnityEngine.UI;
using System;

public class InteractionManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject placementIndicator;
    private ARSessionOrigin arOrigin;
    public ARRaycastManager arRaycast;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool placementPoseLocked = false;

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    void Update()
    {
        if(placementPoseLocked) 
        {
            return;
        }
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    public void LockPlacementIndicator()
    {
        // Check GameManager state
        if(gameManager.getState() != "notStarted") 
        {
            return;
        }
        // Check all the placement conditions
        if((placementPoseIsValid && !placementPoseLocked) || placementPoseLocked) 
        {
            placementPoseLocked = !placementPoseLocked;
        }
    }

    public bool getPlacementPoseLocked() 
    {
        return placementPoseLocked;
    }

    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid) 
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else 
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycast.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if(placementPoseIsValid) 
        {
            placementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
