using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamagableComponent))]
public class Enemy : MonoBehaviour
{
    private Grid _grid;
    [SerializeField] float _moveSpeed = 0.003f;
    [SerializeField]int _moneyForKilling = 1;
    [SerializeField]int _damage = 1;
    [SerializeField]float _attackDistance = 1f;
    [SerializeField]float _attackRate = 1f;
    private float _statsMultiplier = 1f;
    private float _minDistanceToTarget = 1f;
    private Vector2Int _cellDestination;
    private Transform _tranfrosm;
    private List<Vector2Int>_visitedCells;
    private float _attackTimer = 0;
    DamagableComponent _baseHealth; 

    

    public int MoneyForKilling{get => _moneyForKilling*Mathf.CeilToInt(_statsMultiplier);}


    private readonly Vector2Int[]_directions = {Vector2Int.down,Vector2Int.left,Vector2Int.right,Vector2Int.up};

    public void Init(float statsMultiplier, DamagableComponent baseHealth)
    {
        _statsMultiplier = statsMultiplier;
        _tranfrosm = transform;
        _baseHealth = baseHealth;
        _visitedCells = new List<Vector2Int>();
        _damage = Mathf.RoundToInt((float)_damage*_statsMultiplier);
        DamagableComponent damagableComponent = GetComponent<DamagableComponent>();
        damagableComponent.ModifyMaxHealth(Mathf.RoundToInt((float)damagableComponent.MaxHealth*_statsMultiplier)-damagableComponent.MaxHealth);
        damagableComponent.RestoreHealth();
        MultiplySpeed(UnityEngine.Random.Range(0.9f,1.1f)*_statsMultiplier);
    }
    public void SetGrid(Grid grid)
    {
        _grid = grid;
        _cellDestination = _grid.RoadStart;
    }
    
    public void MultiplySpeed(float multiplier)
    {
        _moveSpeed*=multiplier;
    }
    private void HandleMovement()
    {
        Vector2 directionVector = _tranfrosm.position-_baseHealth.transform.position;
        if (_grid.WorldPositionToGridPosition(_tranfrosm.position)!=_grid.RoadEnd)
        {
            directionVector = _grid.GridPositionToWorldPosition(_cellDestination)-new Vector2(_tranfrosm.position.x,_tranfrosm.position.y);
        }
        float distanceToTarget = directionVector.magnitude;
        if (distanceToTarget<=_minDistanceToTarget)
        {
            _visitedCells.Add(_cellDestination);
            foreach (Vector2Int direction in _directions)
            {
                Vector2Int possibleCellPossition = _cellDestination+direction;
                if (!_visitedCells.Contains(possibleCellPossition))
                {
                    Cell cell = _grid.GetCellByPosition(possibleCellPossition);
                    if (cell!=null)
                    {
                        if (cell.hasRoad)
                        {
                            _cellDestination = possibleCellPossition;
                            break;
                        }
                    }
                }
            }

        }
        _tranfrosm.Translate(Vector2.ClampMagnitude(directionVector,1f)*_moveSpeed/100f);
    }
    private void AttackBase()
    {
        if (_attackTimer<_attackRate)
        {
            _attackTimer+=Time.deltaTime;
            return;
        }
        _attackTimer=0;
        if (_baseHealth!=null)
            _baseHealth.ChangeHealth(-_damage);
    }
    void Update()
    {
        if (_grid==null)
            return;
        Vector3 vectorToBase = _tranfrosm.position-_baseHealth.transform.position;
        if (vectorToBase.magnitude<=_attackDistance)
            AttackBase();
        else
            HandleMovement();
        
    }
}
