using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Fruit[] fruitPrefabs;
    [SerializeField] private LineRenderer fruitSpawnLine;
    private Fruit currentFruit;

    [Header("Setting")]
    [SerializeField] private float fruitSpawnPosY;
    bool canControl;
    bool isControlling;

    [Header("Debug")]
    [SerializeField] private bool enableGizmo;

    // Start is called before the first frame update
    void Start()
    {
        HideDropLine();
        canControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canControl)
            ManagePlayerInput();
    }

    void ManagePlayerInput()
    {
        if (Input.GetMouseButtonDown(0)) //click
            MouseDownCallBack();

        else if (Input.GetMouseButton(0)) //drag
        {
            if (isControlling)
                MouseDragCallBack();
            else
                MouseDownCallBack();
        }
            
        else if (Input.GetMouseButtonUp(0) && isControlling) //unclick
            MouseUpCallBack();
    }

    void MouseDownCallBack()
    {
        DisplayDropLine();
        PlaceLineAtClickedPosition();
        SpawnFruit();

        isControlling = true;
    }

    void MouseDragCallBack()
    {
        PlaceLineAtClickedPosition();
        currentFruit.MoveTo(GetSpawnPosition());
    }

    void MouseUpCallBack()
    {
        HideDropLine();
        currentFruit.EnablePhysic();

        isControlling = false;
        canControl = false;
        StartControlTimer(.5f);
    }

    void PlaceLineAtClickedPosition() {
        fruitSpawnLine.SetPosition(0, GetSpawnPosition());
        fruitSpawnLine.SetPosition(1, GetSpawnPosition() + Vector2.down * 10);
    }

    void HideDropLine()
    {
        fruitSpawnLine.enabled = false;
    }

    void DisplayDropLine()
    {
        fruitSpawnLine.enabled = true;
    }

    void StartControlTimer(float timeSecond)
    {
        Invoke("StopControlTimer", timeSecond);
    }

    void StopControlTimer()
    {
        canControl = true;
    }

    void SpawnFruit()
    {
        Vector2 spawnPosition = GetSpawnPosition();

        currentFruit = Instantiate(fruitPrefabs[Random.Range(0, fruitPrefabs.Length)], spawnPosition, Quaternion.identity);
    }

    Vector2 GetClickedWorlPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    Vector2 GetSpawnPosition()
    {
        Vector2 worldClickedPosition = GetClickedWorlPosition();

        worldClickedPosition.y = fruitSpawnPosY;
        return worldClickedPosition;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!enableGizmo)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-5, fruitSpawnPosY, 0), new Vector3(5, fruitSpawnPosY, 0));
    }
}
#endif
