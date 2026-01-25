using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fadeSpeed = 2f;
    public float lifeTime = 1f;

    private TextMeshPro textMesh;
    private Color textColor;
    private float timer;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textColor = textMesh.color;
    }

    public void Setup(int damageAmount, bool isCrit)
    {
        textMesh.text = damageAmount.ToString();

        if (isCrit)
        {
            textMesh.fontSize += 2;         // Make it bigger
            textMesh.color = Color.yellow;  // Crit color
            textMesh.text += "!";           // Add excitement
            textColor = Color.yellow;
        }
        else
        {
            textMesh.color = Color.white;   // Normal color
            textColor = Color.white;
        }

        Destroy(gameObject, lifeTime);
    }

    public void SetupHeal(int amount)
    {
        textMesh.text = "+" + amount;
        textMesh.color = Color.green;       // The universal color for healing
        textMesh.fontSize += 2;             // Make it pop a bit more than normal damage

        textColor = Color.green;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // 1. Float Up
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // 2. Fade out
        timer += Time.deltaTime;
        if (timer > lifeTime / 2)
        {
            float alphaChange = fadeSpeed * Time.deltaTime;
            textColor.a -= alphaChange;
            textMesh.color = textColor;
        }
    }
}
