using UnityEngine;
public interface IEnemy :IMove,IPool,IShoot, IDieable
{
    public BaseEnemySO Enemydata { get;}
    public void ApplyDifficulty(float DifucultyInc);
    public void SetFParrent(FormationManager parrent);
    public void SetTargetP(PlayerController _pl);
    public Vector2 Formationpos { get; set; }
}
