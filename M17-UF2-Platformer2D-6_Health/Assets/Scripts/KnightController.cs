using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private bool patrolling;
    [SerializeField] private float speed;
  
    private float visionRange;
    [Header("BoxCast")]
    [SerializeField] private float boxHeight;
    
    [Header("Sword Attack")]
    [SerializeField] private GameObject swordAttackPoint;
    
    [Header("Magic Attack")]
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackDelay;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile[] fireBalls;
    
    public Transform playerTransform;
    private Animator _animator;
    private int direction = 1;
    private float attackRange = 2;
    private Health _ph;
    private bool isAttacking;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _ph = playerTransform.GetComponent<Health>();
    }

    void Update()
    {
        if (patrolling)
        {
            _animator.SetBool("WithAttackSword", true);
            _animator.SetBool("Walk", true);

            float movementSpeed = speed * Time.deltaTime * direction;
            transform.Translate(movementSpeed, 0 ,0);

            if (transform.position.x >= 4f || transform.position.x <= -2f)
            {
                direction = -direction;
                transform.localScale = new Vector3(direction, 1, 1);
            }

            CheckForPlayerOnPatrol();
        }
        else
        {
            _animator.SetBool("WithAttackSword", false);
            _animator.SetBool("Walk", false);
            CheckForPlayer();
        }
    }

    void OnAttackFrame()
    {
        float boxWidth = 1f; // Ajusta el ancho del área de ataque
        float boxHeight = 2f; // Ajusta la altura del área de ataque
        Debug.Log("OnAttackFrame alehop");

        RaycastHit2D hit = Physics2D.BoxCast(swordAttackPoint.transform.position, new Vector2(boxWidth, boxHeight), 0f, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("OnAttackFrame");
            Health playerHealth = hit.collider.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Ajusta el valor del daño según sea necesario
            }
        }
    }
    
    void CheckForPlayerOnPatrol()
    {
        if (!isAttacking)
        {
            visionRange = 1.8f;
            Vector3 posicionFrente = transform.position + transform.forward * visionRange * 0.5f;
            
            if (transform.localScale.x < 0)
            {
                posicionFrente.x += 1.5f;
            }
            else
            {
                posicionFrente.x -= 1.5f;
            }
                
            RaycastHit2D hit = Physics2D.BoxCast(posicionFrente, new Vector2(visionRange, boxHeight), 0f, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                _animator.SetTrigger("SwordAttack");
                StartCoroutine(AttackCooldown());
            }
        }
    }
    
    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1.5f); // Ajusta este tiempo según lo que necesites
        isAttacking = false;
    }
    
    void CheckForPlayer()
    {
        visionRange = 6.5f;
        Vector3 posicionFrente = transform.position + transform.forward * visionRange * 0.1f;
        
        if (transform.localScale.x < 0)
        {
            posicionFrente.x += 4f;
        }
        else
        {
            posicionFrente.x -= 4f;
        }
        
        RaycastHit2D hit = Physics2D.BoxCast(posicionFrente, new Vector2(visionRange, boxHeight), 0f, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            LaunchFireball();
        }
    }

    public void LaunchFireball()
    {
        Debug.Log("FIREBALL");
        attackTimer = 0.0f;

        int index = FindFireball();
        fireBalls[index].transform.position = firePoint.position;
        fireBalls[index].SetDirection(Mathf.Sign(-transform.localScale.x));
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].gameObject.activeInHierarchy)
            {
                return i;
            }
        }

        return 0;
    }
}
