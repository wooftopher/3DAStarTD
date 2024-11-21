using UnityEngine;

public class Level1Tower : BaseTower {
    [SerializeField] private GameObject levelMissilePrefab;
    [SerializeField] public TowerDataSO towerData;

    protected override void Awake() {
        base.Awake();
        
        InitializeTower(towerData, levelMissilePrefab);
        TowerName = towerData.name;
    }

    protected override void ShootAtTarget(Transform target) {
        base.ShootAtTarget(target);
    }

    public int GetPrice() {
        return towerData.price;
    }
}
