using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField]PlayerInput _playerInput;
    [SerializeField]Placable[] _towerPrefabs;
    [SerializeField]GameObject _towerInConstructionPrefab;

    private int _currentTowerId;
    private void Start() {
        Game.instance.gameOver+=(()=>{
            _playerInput.mouseClickEvent-=OnPlayerClickedAtPosition;
            _playerInput.switched-=SwitchTower;
        });
    }
    private void OnEnable() 
    {
        _playerInput.mouseClickEvent+=OnPlayerClickedAtPosition;
        _playerInput.switched+=SwitchTower;
    }
    private void OnDisable() 
    {
        _playerInput.mouseClickEvent-=OnPlayerClickedAtPosition;
        _playerInput.switched-=SwitchTower;
    }
    
    private void SwitchTower(int towerId)
    {
        _currentTowerId = towerId;
    }

    private void OnPlayerClickedAtPosition(Vector2 position)
    {
        if (!_towerPrefabs[_currentTowerId].CompareCost(Game.instance.PlayerStats))
            return;
        Vector2Int gridPos = Game.instance.grid.WorldPositionToGridPosition(position);
        if (Game.instance.grid.GetCellByPosition(gridPos)==null)
            return;
        if (Game.instance.grid.CheckPlacement(gridPos,_towerPrefabs[_currentTowerId]))
        {
            StartCoroutine(GridBuild(gridPos,_towerPrefabs[_currentTowerId]));
            Game.instance.PlayerStats.AddMoney(-_towerPrefabs[_currentTowerId].Cost);
        }
    }
    private IEnumerator GridBuild(Vector2Int gridPos, Placable placable)
    {
        Vector2 position = Game.instance.grid.GridPositionToWorldPosition(gridPos);
        GameObject inCountruction = Instantiate(_towerInConstructionPrefab,position,Quaternion.identity);
        Game.instance.grid.AddConstruction(gridPos,inCountruction.transform);
        yield return new WaitForSeconds(placable.BuildTime);
        Destroy(inCountruction);
        Game.instance.grid.Build(gridPos,placable);
    }
}
