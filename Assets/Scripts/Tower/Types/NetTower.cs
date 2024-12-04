using UnityEngine;

public class NetTower : BaseTower {
    [SerializeField] private float netStunDuration;
    [SerializeField] private GameObject netPrefab;
    [SerializeField] private TowerDataSO NetTowerData;


    protected override void Awake() {
        base.Awake();

        InitializeTower(NetTowerData, null);
    }

    protected override void ShootAtTarget(Transform target) {
        GameObject netMissile = Instantiate(netPrefab, firePoint.position, netPrefab.transform.rotation);
        NetMissile missileComponent = netMissile.GetComponent<NetMissile>();

        if (missileComponent != null) {
            missileComponent.SetTarget(target, NetTowerData.damage, netStunDuration); 
        }
    }
}
