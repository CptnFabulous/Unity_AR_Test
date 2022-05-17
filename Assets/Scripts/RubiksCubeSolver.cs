using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubiksCubeSolver : MonoBehaviour
{
    public RubiksCube currentCube;

    [Header("Setup")]
    public Camera viewingCamera;
    public SwipeInput rotateSlice;
    public LayerMask touchDetection = ~0;
    public Slider sizeSlider;
    public Button resetButton;
    public Button autoSolveButton;
    

    private void Awake()
    {
        rotateSlice.onSwipe.AddListener(RotateSliceBasedOnTouchPositions);
        sizeSlider.wholeNumbers = true;
        resetButton.onClick.AddListener(() => currentCube.ResetCube(Mathf.RoundToInt(sizeSlider.value)));
        //autoSolveButton.onClick.AddListener(AutoSolve);
    }

    /// <summary>
    /// Uses raycast detection to ensure slices are rotated based on the specific point the player swipes on the cube.
    /// </summary>
    /// <param name="swipe"></param>
    public void RotateSliceFromTouchPositions(SwipeInput.Swipe swipe)
    {
        
        /*
        float distanceToTarget = Vector3.Distance(viewingCamera.transform.position, currentCube.transform.position);
        bool swipeDetectedSomething = Physics.Raycast(viewingCamera.ScreenPointToRay(swipe.startPosition), out RaycastHit rh, distanceToTarget, touchDetection);
        if ((swipeDetectedSomething && rh.collider == currentCube.detectionCollider) == false) return; // Make sure the swipe started on the cube itself

        Vector3 start = swipe.startPosition;
        Vector3 end = swipe.endPosition;
        start.z = rh.distance;
        end.z = rh.distance;
        Vector3 worldOrigin = viewingCamera.ScreenToWorldPoint(start);
        Vector3 worldSwipeDirection = viewingCamera.ScreenToWorldPoint(end) - worldOrigin;


        float distanceInwards = distanceToTarget - rh.distance;



        Vector3 worldOrigin = rh.point + (distanceInwards * -rh.normal);
        Vector3 worldSwipeDirection;

        // From raycast hit point, 


        
        Vector3 worldSliceRotationAxis = Vector3.Cross(currentCube.transform.position - viewingCamera.transform.position, worldSwipeDirection);
        Debug.DrawRay(worldOrigin, worldSwipeDirection, Color.white, 10);
        Debug.DrawRay(worldOrigin, worldSliceRotationAxis, Color.red, 10);

        currentCube.RotateSliceFromWorldDirectionValues(worldOrigin, worldSliceRotationAxis, worldSwipeDirection);
        */
    }

    public void RotateSliceBasedOnTouchPositions(SwipeInput.Swipe swipe)
    {
        Vector3 start = swipe.startPosition;
        Vector3 end = swipe.endPosition;
        float distanceToTarget = Vector3.Distance(viewingCamera.transform.position, currentCube.transform.position);
        start.z = distanceToTarget;
        end.z = distanceToTarget;

        Vector3 worldOrigin = viewingCamera.ScreenToWorldPoint(start);
        Vector3 worldSwipeDirection = viewingCamera.ScreenToWorldPoint(end) - worldOrigin;
        Vector3 worldSliceRotationAxis = Vector3.Cross(currentCube.transform.position - viewingCamera.transform.position, worldSwipeDirection);
        Debug.DrawRay(worldOrigin, worldSwipeDirection, Color.white, 10);
        Debug.DrawRay(worldOrigin, worldSliceRotationAxis, Color.red, 10);

        currentCube.RotateSliceFromWorldDirectionValues(worldOrigin, worldSliceRotationAxis, worldSwipeDirection);
    }
    
}
