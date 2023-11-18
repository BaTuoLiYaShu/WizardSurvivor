using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    public int rotationSpeed;
    public float fireSpeed;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public AudioSource gunSoundSource;
    public bool isFiring;

    private Quaternion defaultRotation;
    
    private Transform bulletParent;
    private Transform enemy;
    public Transform muzzle;

    public Animator anim;
    public AttackRadius attackRadius;

    private void Start()
    {   
        enemy = null;
        defaultRotation = transform.rotation;
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += AtAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= AtAfterSceneLoadedEvent;
    }

    private void AtAfterSceneLoadedEvent()
    {
        bulletParent = GameObject.FindWithTag("Bullet Parent").transform;
    }

    private void Update()
    {
        GunRotation();
        if (!isFiring)
        {
            StartCoroutine(Fire());
        }
    }

    private void GetEnemyInTrigger()
    {
        var temp = attackRadius.GetNearestEnemy();
        enemy = temp != null ? temp.transform : null;
    }

    /// <summary>
    /// 武器跟踪敌人
    /// </summary>
    private void GunRotation()
    {
        GetEnemyInTrigger();
        if (enemy == null)
        {
            transform.rotation = defaultRotation;
            transform.localScale = new Vector3(1, 1, 1);
            return;
        }
        
        //设置武器旋转
        Vector3 vectorToEnemy = enemy.position - transform.position;
        float angle = Mathf.Atan2(vectorToEnemy.y, vectorToEnemy.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
        
        var weaponLocalScale = transform.localScale;
        if (angle < 90 && angle > -90)
        {
            transform.localScale = new Vector3(weaponLocalScale.x, 1f, weaponLocalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(weaponLocalScale.x, -1f, weaponLocalScale.z);
        }
    }

    /// <summary>
    /// 射击生成子弹并播放射击动画
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fire()
    {
        if (enemy == null)
        {
            yield break;
        }
        
        isFiring = true;
        anim.SetTrigger("Fire");
        yield return new WaitForSeconds(0.2f);
        
        //生成子弹
        gunSoundSource.Play();
        var bullet = Instantiate(bulletPrefab, muzzle.position, transform.rotation, bulletParent);
        var direction = (enemy.position - transform.position).normalized;

        //子弹偏移
        float angle = Random.Range(-8f, 8f);
        bullet.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(angle, Vector3.forward) * direction * bulletSpeed;
        
        yield return new WaitForSeconds(fireSpeed);
        isFiring = false;
    }
}

