using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public List<Transform> waypoints;
    [SerializeField] private float _speed;

    private List<Vector3> _waypointPositions;
    private int _currentWaypointIndex;
    private short _currentWaypointDirection = 1;

    private Vector3 _currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        _waypointPositions = new List<Vector3>();
        foreach(Transform t in waypoints)
        {
            _waypointPositions.Add(t.position);
        }

        SelectWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, _currentTarget) < 0.1f)
        {
            SelectWaypoint();
        }

        Vector3 dir = _currentTarget - transform.position;
        dir.Normalize();



        transform.position += dir * _speed * Time.deltaTime;
    }

    private void SelectWaypoint()
    {

        if(_currentWaypointDirection == 1)
        {
            if(_currentWaypointIndex >= _waypointPositions.Count - 1)
            {
                _currentWaypointDirection = -1;
            }
        }
        else
        {
            if (_currentWaypointIndex <= 0)
            {
                _currentWaypointDirection = 1;
            }
        }
        _currentWaypointIndex += _currentWaypointDirection;
        _currentTarget = _waypointPositions[_currentWaypointIndex];
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
