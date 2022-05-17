using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCube : MonoBehaviour
{
    [Header("Stats")]
    public int defaultSize = 3;
    public float timer;
    
    [Header("Setup")]
    public MeshRenderer faceSquarePrefab;
    public Transform rotationCarrier;
    Vector3 squareRotationOffsetEulerAngles = new Vector3(90, 0, 0);

    [Header("Cosmetics")]
    public float rotationTime = 0.5f;
    public AnimationCurve rotationAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public Color[] faceColours = new Color[]
    {
        Color.green,
        Color.blue,
        Color.white,
        Color.yellow,
        Color.red,
        new Color(1, 0.25f, 0)
    };



    public BoxCollider detectionCollider { get; private set; }
    IEnumerator currentAction
    {
        get => ca;
        set
        {
            if (ca != null) StopCoroutine(ca);
            ca = value;
            if (ca != null) StartCoroutine(ca);
        }
    }


    IEnumerator ca;
    List<MeshRenderer>[] squaresSortedByColour;
    List<MeshRenderer> allSquares;

    // Function for shuffling the cube

    private void OnValidate()
    {
        // Validate number of direction colours
        if (faceColours.Length != Directions.all.Length)
        {
            Color[] newColours = new Color[Directions.all.Length];
            for (int i = 0; i < Mathf.Min(faceColours.Length, newColours.Length); i++)
            {
                newColours[i] = faceColours[i];
            }
            faceColours = newColours;
        }
    }
    private void Awake()
    {
        detectionCollider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        ResetCube(defaultSize);
    }

    /// <summary>
    /// Function for generating cube and storing square values
    /// </summary>
    /// <param name="gridSize"></param>
    public void ResetCube(int gridSize)
    {
        detectionCollider.center = Vector3.zero;
        detectionCollider.size = gridSize * Vector3.one;

        squaresSortedByColour = new List<MeshRenderer>[Directions.all.Length];
        allSquares = new List<MeshRenderer>();

        float perpendicularAxisOffset = -(gridSize - 1) * 0.5f;
        Vector3 offset = new Vector3(perpendicularAxisOffset, perpendicularAxisOffset, gridSize * 0.5f);
        Quaternion facingForwards = Quaternion.Euler(squareRotationOffsetEulerAngles);

        for (int i = 0; i < Directions.all.Length; i++)
        {
            squaresSortedByColour[i] = new List<MeshRenderer>();
            Quaternion currentRotation = Quaternion.LookRotation(Directions.all[i], Vector3.up);

            #region Generate squares
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    Vector3 position = new Vector3(x, y, 0) + offset;
                    MeshRenderer square = Instantiate(faceSquarePrefab, transform);
                    squaresSortedByColour[i].Add(square);

                    square.transform.localPosition = currentRotation * position;
                    square.transform.localRotation = currentRotation * facingForwards;
                    square.material.color = faceColours[i];
                }
            }
            #endregion

            allSquares.AddRange(squaresSortedByColour[i]);
        }
    }
    public void RotateSliceFromWorldDirectionValues(Vector3 worldSelectionPoint, Vector3 worldAxis, Vector3 worldSwipeDirection)
    {
        Vector3 localSelectionPoint = transform.InverseTransformPoint(worldSelectionPoint);
        Vector3 localAxis = transform.InverseTransformDirection(worldAxis);
        localAxis = Directions.LocalToCardinal(localAxis);

        List<MeshRenderer> slice = GetSlice(localSelectionPoint, localAxis);

        currentAction = RotateSlice(slice, localAxis);
    }
    List<MeshRenderer> GetSlice(Vector3 localSelectionPoint, Vector3 localAxis)
    {
        Quaternion localRotationOffset = Quaternion.LookRotation(localAxis, Vector3.up);
        localRotationOffset = Quaternion.Inverse(localRotationOffset);

        return allSquares.FindAll((s) =>
        {
            Vector3 localPosition = transform.InverseTransformPoint(s.transform.position);
            localPosition -= localSelectionPoint; // Make localPosition relative to localPoint
            localPosition = localRotationOffset * localPosition; // Rotate localPosition so it's also relative to localDirection

            // Select squares whose Z position is within the desired thresholds
            return localPosition.z > -0.5f && localPosition.z < 0.5f;
        });
    }
    IEnumerator RotateSlice(List<MeshRenderer> squaresToRotate, Vector3 localAxis)
    {
        rotationCarrier.localRotation = Quaternion.LookRotation(Vector3.forward, localAxis);
        Quaternion oldRotation = rotationCarrier.localRotation;
        Quaternion newRotation = rotationCarrier.localRotation * Quaternion.Euler(0, -90, 0);

        for (int i = 0; i < squaresToRotate.Count; i++)
        {
            squaresToRotate[i].transform.parent = rotationCarrier;
        }
        float timer = 0;
        while (timer < 1)
        {
            timer = Mathf.Clamp01(timer + Time.deltaTime / rotationTime);

            float lerpProgress = rotationAnimationCurve.Evaluate(timer);
            rotationCarrier.localRotation = Quaternion.LerpUnclamped(oldRotation, newRotation, lerpProgress);

            yield return null;
        }
        for (int i = 0; i < squaresToRotate.Count; i++)
        {
            squaresToRotate[i].transform.parent = transform;
        }
    }




    
    
}
