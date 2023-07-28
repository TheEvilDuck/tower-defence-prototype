using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Builder : MonoBehaviour
{
    [SerializeField]PlayerInput _playerInput;
    [SerializeField]Placable[] _towerPrefabs;
    [SerializeField]GameObject _towerInConstructionPrefab;

    private int _currentTowerId;
    private void Start() {
        Game.instance.gameOver+=(()=>{
            OnDisable();
        });
    }
    private void OnEnable() 
    {
        _playerInput.mouseClickEvent+=OnPlayerClickedAtPosition;
        _playerInput.mouseRightClickEvent+=OnPlayerClickedRightAtPosition;
        _playerInput.switched+=SwitchTower;
    }
    private void OnDisable() 
    {
        _playerInput.mouseClickEvent-=OnPlayerClickedAtPosition;
        _playerInput.mouseRightClickEvent-=OnPlayerClickedRightAtPosition;
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
    private void OnPlayerClickedRightAtPosition(Vector2 position)
    {
        Vector2Int gridPos = Game.instance.grid.WorldPositionToGridPosition(position);
        Cell cell = Game.instance.grid.GetCellByPosition(gridPos);
        if (cell==null)
            return;
        cell.DestroyPlacable();
    }
        
    private IEnumerator GridBuild(Vector2Int gridPos, Placable placable)
    {
        bool canceled = false;
        Vector2 position = Game.instance.grid.GridPositionToWorldPosition(gridPos);
        GameObject inCountruction = Instantiate(_towerInConstructionPrefab,position,Quaternion.identity);
        Game.instance.grid.AddConstruction(gridPos,inCountruction.transform);
        Action OnConstructionRemove = () =>
        {
            Game.instance.PlayerStats.AddMoney(placable.Cost);
            canceled = true;
        };
        Cell cell = Game.instance.grid.GetCellByPosition(gridPos);
        cell.placableDestroyed+=OnConstructionRemove;
        yield return new WaitForSeconds(placable.BuildTime);
        cell.placableDestroyed-=OnConstructionRemove;
        if (inCountruction!=null)
            Destroy(inCountruction);
        if (canceled)
            yield break;
        Game.instance.grid.Build(gridPos,placable);
        Action OnPlacableRemove = null;
        OnPlacableRemove = () =>
        {
            cell.placableDestroyed-=OnPlacableRemove;
            GameObject inCountruction = Instantiate(_towerInConstructionPrefab,position,Quaternion.identity);
            Game.instance.grid.AddConstruction(gridPos,inCountruction.transform);
            StartCoroutine(PlacableRemove(placable.BuildTime/2f,placable.Cost/2,cell));
        };
        cell.placableDestroyed+=OnPlacableRemove;
        
    }

    private IEnumerator PlacableRemove(float waitForSeconds,int moneyToReturn, Cell cell)
    {
        yield return new WaitForSeconds(waitForSeconds);
        if (cell==null)
            yield break;
        if (cell.placable!=null)
            Game.instance.PlayerStats.AddMoney(moneyToReturn);
        cell.DestroyPlacable();
    }
    

}
