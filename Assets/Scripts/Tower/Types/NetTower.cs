using UnityEngine;

public class Level1NetTower : BaseTower {
    [SerializeField] private float netStunDuration;
    [SerializeField] private GameObject netPrefab;
    [SerializeField] private TowerDataSO towerData;


    protected override void Awake() {
        base.Awake();

        InitializeTower(towerData, null);
        TowerName = towerData.name;
    }

    protected override void ShootAtTarget(Transform target) {
        GameObject netMissile = Instantiate(netPrefab, firePoint.position, netPrefab.transform.rotation);
        NetMissile missileComponent = netMissile.GetComponent<NetMissile>();

        if (missileComponent != null) {
            missileComponent.SetTarget(target, towerData.Damage, netStunDuration); 
        }
    }
}
