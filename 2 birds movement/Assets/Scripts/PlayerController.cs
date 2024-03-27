using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.Burst.Intrinsics.Arm;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject pigeon;

    private Tweener tweener;
    private char currentInput;

    // Map of keys and their corresponding directions
    private Dictionary<char, Vector2> charToDirection = new Dictionary<char, Vector2>
    {
        { 'W', Vector2.up },
        { 'A', Vector2.left },
        { 'S', Vector2.down },
        { 'D', Vector2.right }
    };

    // To record KeyCodes as chars
    private Dictionary<KeyCode, char> keyToChar = new Dictionary<KeyCode, char>
    {
        { KeyCode.W, 'W' },
        { KeyCode.A, 'A' },
        { KeyCode.S, 'S' },
        { KeyCode.D, 'D' }
    };

    // To rotate air particles based on movement - can be used later
    private Dictionary<Vector2, float> directionToRotation = new Dictionary<Vector2, float>
    {
        { Vector2.up, 0f },
        { Vector2.down, 180f },
        { Vector2.left, 90f },
        { Vector2.right, -90f }
    };

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();

    }

    // Update is called once per frame
    void Update()
    {
        getPlayerInput();
        // movePigeon();
    }

    private void getPlayerInput()
    {
        if (pigeon == null) return;

        foreach (var key in keyToChar.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                currentInput = keyToChar[key];
                movePigeon();
            }
            if (Input.GetKeyUp(key)) return;
        }
    }


    private void movePigeon()
    {
        if (pigeon == null || !charToDirection.ContainsKey(currentInput)) return;

        Vector2 direction = charToDirection[currentInput];

        // float moveSpeed = 5f; // Customize this speed as needed
        // Debug.Log("Move amount: " + moveAmount);

        // Calculating the new position based on the direction
        Vector3 newPosition = pigeon.transform.position + new Vector3(direction.x, direction.y, 0);
        Debug.Log("Previous pos: " + pigeon.transform.position);
        pigeon.transform.position = newPosition;
        Debug.Log("New pos: " + pigeon.transform.position);
    }

}