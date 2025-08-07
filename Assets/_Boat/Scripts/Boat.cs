using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public Hex currentHex;
    [SerializeField]
    private float movementSpeed = 1.5f;

    private Queue<Vector3> path = new();
    public bool isMoving = false;

    public void movementOfBoat(List<Hex> pathOfHexes, GridSystem gridSystem)
    {
        path.Clear();
        foreach (Hex hex in pathOfHexes)
        {
            HexStructure step = gridSystem.GetHexStructure(hex);
            if (step != null)
            {
                path.Enqueue(step.hexView.transform.position);
            }
        }
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving || path.Count == 0) return;

        Vector3 target = path.Peek();

        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        transform.position = Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            transform.position = target;
            path.Dequeue();
        }

        if (path.Count == 0)
        {
            isMoving = false;
        }
    }
}
