using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;

    float x, y;

    Vector2 vMouseDown;

    public RectTransform imageInput;

    public HealthBarUI healthUI;

    public float maxHp;
    public float hp;

    public float delayAttack;
    public enum Weapon
    {
        Knife,
        Sword,
        Kunai,
        Shuriken,
        Gun
    }
    public Weapon weapon;

    void Start()
    {
        Manager.manager.level.SetStart();
        Manager.manager.pause = false;
    }

    void Update()
    {
        if (Manager.manager.pause)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            vMouseDown = Input.mousePosition;
            imageInput.position = Input.mousePosition;
            imageInput.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButton(0))
        { 
            Vector2 dir = ((Vector2)Input.mousePosition - vMouseDown).normalized;
            rb.velocity = new Vector3(dir.x, 0, dir.y) * speed;

            //Quaternion quaternion = Quaternion.LookRotation();
            //Vector3 v = Manager.manager.QuaternionToEuler(quaternion);
            //transform.eulerAngles = new Vector3(0, v.x,0);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            imageInput.gameObject.SetActive(false);
            vMouseDown = Vector2.zero;
        }

        FindEnemy();
        delayAttack -= Time.deltaTime;
    }

    void FindEnemy()
    {
        float range = 0;
        switch (weapon)
        {
            case Weapon.Knife:
                range = 1f;
                break;
            case Weapon.Sword:
                range = 1.5f;
                break;
            case Weapon.Kunai:
                range = 2f;
                break;
            case Weapon.Shuriken:
                range = 2f;
                break;
            case Weapon.Gun:
                range = 3f;
                break;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, range, 1 << 7);
        if (colliders.Length > 0)
        {
            Attack(colliders[0].GetComponentInParent<EnemyMovement>());
        }
    }

    void Attack(EnemyMovement g)
    {
        if (delayAttack <= 0)
        {
            delayAttack = 1;
            g.TakeDamage();
        }
    }

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        healthUI.SetImg(hp / maxHp);
    }
}
