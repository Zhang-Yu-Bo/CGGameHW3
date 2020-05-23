using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    public Transform rebornPoint;
    public float lookRadius;

    private NavMeshAgent _agent;
    private Animator _animator;
    private GameObject _player;
    private Vector3 _myPosition;

    private void Awake()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _animator = gameObject.GetComponent<Animator>();
        if (_player == null)
            FindPlayer();
        _myPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent == null)
            return;

        // Debug
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.point);
            }
        }
        
        if (_player == null)
            FindPlayer();
        else
        {
            if ((_player.transform.position - transform.position).magnitude <= lookRadius)
            {
				// track
				lookRadius = 20;
                _agent.SetDestination(_player.transform.position);
            }
            else
            {
				// untrack
				lookRadius = 10;
                _agent.SetDestination(_myPosition);
            }
        }

        // animation
        if (Mathf.Abs((_agent.destination - transform.position).magnitude) <= _agent.stoppingDistance)
            _animator.SetBool("Run", false);
        else
            _animator.SetBool("Run", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<CharacterControl>().TranslateTo(rebornPoint.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void FindPlayer()
    {
        CharacterControl temp = FindObjectOfType<CharacterControl>();
        if (temp != null)
            _player = temp.gameObject;
    }
}
