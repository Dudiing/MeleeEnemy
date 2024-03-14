using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private bool patrolling;
    [SerializeField] private float speed;
  
    private float visionRange;
    [Header("BoxCast")]
    [SerializeField]  private float boxHeight;
    
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

    void Start()
    {
        patrolling = false;
        _animator = GetComponent<Animator>();
        attackTimer = 0;
        _ph = playerTransform.gameObject.GetComponent<Health>();
    }

    void Update()
    {
        
        attackTimer += Time.deltaTime;

        if (patrolling)
        {
            _animator.SetBool("WithAttackSword", true);
            
            float movementSpeed = speed * Time.deltaTime * direction;
        
             transform.Translate(movementSpeed, 0 ,0);
              if (transform.position.x >= 4f)
              {
                  direction = -direction;
                  transform.localScale = Vector3.one;
              }

              if (transform.position.x <= -2f)
              {
                  direction = -direction;
                  transform.localScale = new Vector3(-1, 1, 1);
              }
              
              CheckForPlayerOnPatrol();

        }

        if (!patrolling)
        {
            _animator.SetBool("WithAttackSword", false);
            CheckForPlayer();
            
            
        }
    }
    
    void CheckForPlayerOnPatrol()
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
            
           // Debug.Log("SWORD ATTACK TRUE");
            _animator.SetTrigger("SwordAttack");

        }
        else
        {
         //   Debug.Log("SWORD ATTACK FALSE");
        }
        
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
        int layerMask = ~LayerMask.GetMask("Fireball");
        RaycastHit2D hit = Physics2D.BoxCast(posicionFrente, new Vector2(visionRange, boxHeight), 0f, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            LaunchFireball();
        }
        else
        {
           // Debug.Log("MAGIC ATTACK FALSE");
        }
        
    }

    public void LaunchFireball()
    {
        //_animator.SetTrigger("attack");
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
    void OnAttackFrame()
    {
        float boxWidth = 1f; 
        float boxHeight = 2f; 
        
        RaycastHit2D hit = Physics2D.BoxCast(swordAttackPoint.transform.position, new Vector3(boxWidth, boxHeight), 0f, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
   
            Debug.Log("PLAYER HIT");
            _ph.TakeDamage(1f);
         
        }
    }

 /*   void OnDrawGizmosSelected()
    {
        // Configura las dimensiones del BoxCast (anchura y longitud).
        float boxWidth = 1.5f; // Cambia esto según tus necesidades.
        float boxHeight = 2f; // Cambia esto según tus necesidades.

        // Dibuja el BoxCast con gizmos.
        Gizmos.color = Color.red; // Puedes elegir otro color si lo prefieres.
        Gizmos.DrawWireCube(swordAttackPoint.transform.position, new Vector3(boxWidth, boxHeight, 0f));
       
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
       
       Gizmos.color = Color.red; // Puedes elegir otro color si lo prefieres.
       Gizmos.DrawWireCube(posicionFrente, new Vector3(visionRange, boxHeight, 0f));
    }*/
}