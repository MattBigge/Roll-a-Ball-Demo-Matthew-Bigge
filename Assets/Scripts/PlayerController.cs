using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Include the namespace required to use Unity UI and Input System
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    public float speedUp = 2f;
    private List<GameObject> speedUps;

    public Vector3 StartPosition = new Vector3(0f, 2f, -38.5f);
    
    private Rigidbody rb;

    private int count;
    private float movementX;
    private float movementY;

    private bool inAir = false;

    private bool winTextUp = false;
    private 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        //reset/set text UI
        SetSpeedText();
        winTextObject.SetActive(false);

        speedUps = new List<GameObject>();
    }

    void OnMove(InputValue movementValue) {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetSpeedText() 
    {
        countText.text = "Speed: " + speed.ToString();
        
    }

    void FixedUpdate() 
    {
        if (transform.position.y < -10f)
        {
            Restart();
            return;
        }

        Vector3 movement;
        if (inAir)
        {
            return;
        }
        if (movementY >= 0f)
        {
            movement = new Vector3(movementX, 0.0f, movementY);
        } 
        else 
        {
            movement = new Vector3(movementX, 0.0f, 0f);
        }

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            speed += speedUp;
            other.gameObject.SetActive(false);
            speedUps.Add(other.gameObject);

            SetSpeedText();

            if (winTextUp && speed > 3f)
            {
                winTextObject.SetActive(false);
                winTextUp = false;
            }
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            inAir = true;
        }
    }

    private void Restart()
    {
        foreach (GameObject g in speedUps)
        {
            g.SetActive(true);
        }

        speedUps.Clear();


        winTextObject.SetActive(true);
        winTextObject.GetComponent<TextMeshProUGUI>().text = Vector3.Distance(transform.position, StartPosition).ToString() + " m";
        winTextUp = true;
        transform.position = StartPosition;
        speed = 3f;
        inAir = false;
        rb.isKinematic = true;
        rb.isKinematic = false;

    }
}
