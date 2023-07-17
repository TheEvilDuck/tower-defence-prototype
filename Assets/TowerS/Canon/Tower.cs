using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tower : MonoBehaviour
{
    [SerializeField]Transform _canon;
    [SerializeField]float _attackDistance = 2f;
    [SerializeField]float _attackRate = 1f;
    [SerializeField]int _damage = 10;
    [SerializeField] SimpleSpriteAnimatorComponent _animator;
    protected Enemy _target;
    private float _attackTimer = 0;
    private Transform _transform;
    
    void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _attackTimer+=Time.deltaTime;
        if (_target!=null)
        {
            if (_attackTimer>=_attackRate)
            {
                _attackTimer = 0;
                Hit();
            }
            Vector3 direction = _target.transform.position-_transform.position;
            float angle = Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg;
            _canon.rotation = Quaternion.AngleAxis(angle-90,Vector3.forward);

            
        }
        foreach (Enemy enemy in Game.instance.EnemySpawner.enemies)
        {
            if ((enemy.transform.position-_transform.position).magnitude<=_attackDistance)
            {
                _target = enemy;
                return;
            }
        }
        _target = null;
    }
    public virtual void Hit()
    {
        if (_target==null)
            return;
        _animator?.Play();
        DamagableComponent damagableComponent = _target.GetComponent<DamagableComponent>();
        damagableComponent.ChangeHealth(-_damage);
    }
}
