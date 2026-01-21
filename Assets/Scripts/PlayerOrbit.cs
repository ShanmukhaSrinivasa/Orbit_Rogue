using System;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class PlayerOrbit : MonoBehaviour
{
    [Header("References")]
    public Transform centerPoint;

    [Header("Movement Stats")]
    public float orbitSpeed = 120f; // Speed in Degrees per second
    public float orbitRadius = 4f; // Distance from the boss

    [Header("Juice / Feel")]
    public float switchSmoothness = 10f; // How fast we flip direction visually

    //Internal Variables
    private float currentAngle = 0f;
    private float currentDirection = 1f; // 1 = clockwise, -1 = Counter-Clockwise
    private float visualDirection = 1f; // used for smooth turning animation

    void Start()
    {
        //Calculate inital angle based on where we placed the player in editor
        Vector3 dir = transform.position - centerPoint.position;
        currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    
    void Update()
    {
        // Safety Check : If the boss is dead/destroyed, stop moving to prevent crash
        if (centerPoint == null)
        {
            return;
        }

        HandleInput();
        MovePlayer();
        RotatePlayerSprite();
    }


    private void HandleInput()
    {
        // The Single Mechanic : Switch Direction
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            currentDirection *= -1f;
        }
    }
    private void MovePlayer()
    {
        // 1. Update Angle
        // We Substract direction because usually C# angles go Counter-Clokwise,
        // but we want positive speed to look ClockWise
        currentAngle -= currentDirection * orbitSpeed * Time.deltaTime;

        // 2. Keep angle within 0-360 
        currentAngle = currentAngle % 360;

        // 3. Covert Polar (Angle / Radius) to Cartesian (X/Y)
        float radians = currentAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians) * orbitRadius;
        float y = Mathf.Sin(radians) * orbitRadius;

        // 4. Apply Position
        transform.position = centerPoint.position + new Vector3(x, y, 0);
    }

    private void RotatePlayerSprite()
    {
        // This makes the ship face the direction it is moving

        // Calculate the tangent angle (90 degrees offset from the radius)
        float tangentAngle = currentAngle + (currentDirection > 0 ? -90f : 90f);

        // Smoothly rotate to that angle (Juice!)
        Quaternion targetRotation = Quaternion.Euler(0, 0, tangentAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * switchSmoothness);
    }

    public void SetCenter(Transform newCenter)
    {
        centerPoint = newCenter;
    }
}
