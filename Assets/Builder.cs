using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Builder : MonoBehaviour
{
    [SerializeField]PlayerInput _playerInput;
    [SerializeField]Placable[] _towerPrefabs;
    [SerializeField]UnderConstruction _towerInConstructionPrefab;

    private int _currentTowerId;
    private Game _game;
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
        Placable currentPlacable = _towerPrefabs[_currentTowerId];
        if (!currentPlacable.CompareCost(_game.PlayerStats))
            return;
        Vector2Int gridPos = _game.grid.WorldPositionToGridPosition(position);
        if (_game.grid.GetCellByPosition(gridPos)==null)
            return;
        if (_game.grid.CheckPlacement(gridPos,currentPlacable))
        {
            _game.PlayerStats.AddMoney(-currentPlacable.Cost);
            UnderConstruction underConstruction = Instantiate(_towerInConstructionPrefab,_game.grid.GridPositionToWorldPosition(gridPos),Quaternion.identity);
            _game.grid.AddConstruction(gridPos, underConstruction);
            underConstruction.ConstructionDone+=(bool isCompleted)=>
            {
                if (_game.grid.TryRemovePlacableAt(gridPos, out PlacableOnGrid placableToDestroy))
                {
                    placableToDestroy.OnRemove();
                    Destroy(placableToDestroy.gameObject);
                }
                if (!isCompleted)
                    _game.PlayerStats.AddMoney(currentPlacable.Cost);
                else
                {
                    PlacableOnGrid placable = Instantiate(currentPlacable.Prefab,_game.grid.GridPositionToWorldPosition(gridPos),Quaternion.identity);
                    placable?.ProvideBase(_game.Base);
                    placable?.ProvideEnemySpawner(_game.EnemySpawner);
                    placable?.ProvidePlayerStats(_game.PlayerStats);
                    placable?.OnBuild();
                    _game.grid.Build(gridPos,placable);
                }

            };
            underConstruction.SetTier(currentPlacable.BuildTime);
        }
    }
    private void OnPlayerClickedRightAtPosition(Vector2 position)
    {
        Vector2Int gridPos = _game.grid.WorldPositionToGridPosition(position);
        if (_game.grid.TryRemovePlacableAt(gridPos,out PlacableOnGrid placableToDestroy))
        {
            placableToDestroy.OnRemove();
            Destroy(placableToDestroy.gameObject);
        }
    }
        

    
    public void Init(Game game)
    {
        _game = game;
        _playerInput.mouseClickEvent+=OnPlayerClickedAtPosition;
        _playerInput.mouseRightClickEvent+=OnPlayerClickedRightAtPosition;
        _playerInput.switched+=SwitchTower;
    }
}
