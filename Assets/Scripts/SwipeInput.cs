using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInput))]
public class SwipeInput : MonoBehaviour
{
    public struct Swipe
    {
        public Swipe(Vector2 startPosition, Vector2 endPosition, float startTime, float endTime)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public Vector2 startPosition { get; private set; }
        public Vector2 endPosition { get; private set; }
        public float startTime { get; private set; }
        public float endTime { get; private set; }

        public Vector2 direction => endPosition - startPosition;
        public float distance => Vector2.Distance(startPosition, endPosition);
        public float duration => endTime - startTime;
    }
    
    public float minDistance = 0.2f;
    public float maxTime = 1;
    public UnityEvent<Swipe> onSwipe;

    Vector2 fingerPosition;
    Vector2 swipeStart;
    Vector2 swipeEnd;
    float swipeStartTime;
    float swipeEndTime;

    public void OnSwipePosition(InputValue input) => fingerPosition = input.Get<Vector2>();
    public void OnSwipeContact(InputValue input)
    {
        if (input.isPressed) // Swipe has started
        {
            swipeStart = fingerPosition;
            swipeStartTime = Time.time;
        }
        else // Player has released their mouse
        {
            swipeEnd = fingerPosition;
            swipeEndTime = Time.time;

            // Check if finger moved fast enough to not be a drag
            if (swipeEndTime - swipeStartTime <= maxTime == false) return;
            // Check if finger moved far enough to not be a tap
            if (Vector2.Distance(swipeStart, swipeEnd) > minDistance == false) return;
            
            onSwipe.Invoke(new Swipe(swipeStart, swipeEnd, swipeStartTime, swipeEndTime));
        }
    }

    public static void DrawDebugScreenLine(Camera camera, Vector2 screenStart, Vector2 screenEnd, Color colour, float duration = 0)
    {
        Vector3 start = screenStart;
        Vector3 end = screenEnd;
        start.z = camera.nearClipPlane;
        end.z = camera.nearClipPlane;
        start = camera.ScreenToWorldPoint(start);
        end = camera.ScreenToWorldPoint(end);

        Debug.DrawLine(start, end, colour, duration);
    }
}
