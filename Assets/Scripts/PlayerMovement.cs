using DG.Tweening;
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

    public float maxArm;
    public float arm;

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
        Manager.manager.SetStart();
        Manager.manager.pause = false;
        Manager.manager.HideContract(3);

        Manager.manager.RandomItem();
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

            if (dir != Vector2.zero)
            {
                Quaternion quaternion = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));

                transform.DORotateQuaternion(quaternion, .5f);
            }
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
            GameObject b = null;
            switch (weapon)
            {
                case Weapon.Knife:
                    break;
                case Weapon.Sword:
                    break;
                case Weapon.Kunai:
                    break;
                case Weapon.Shuriken:
                    break;
                case Weapon.Gun:
                    b = Instantiate(Resources.Load("Bullet") as GameObject, transform.position, Quaternion.identity);
                    b.transform.DOMove(g.transform.position, .25f);
                    break;
            }
            Destroy(b, .25f);

            delayAttack = 1;
            g.TakeDamage();
        }
    }

    public void TakeDamage(float dmg)
    {
        if (arm > 0)
        {
            arm -= dmg;
            healthUI.SetImgArmor(arm/ maxArm);
        }
        else
        {
            hp -= dmg;
            healthUI.SetImg(hp / maxHp);
        }
    }

    public void TakeMoney()
    {
        Manager.manager.TakeMoney(1);
    }

    public void TakeHp()
    {
        hp = Mathf.Clamp(hp + 25, 0, 100);
        healthUI.SetImg(hp / maxHp);
    }

    public void TakeArmor()
    {
        arm = Mathf.Clamp(arm + 10, 0, 100);
        healthUI.SetImgArmor(arm / maxArm);
    }

    public void TakeSpeed()
    {
        speed += .5f;
    }

    public void ResetStats()
    {
        arm = 0;
        hp = 100;
        speed = 5;
    }
}
