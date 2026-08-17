using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyClass), true)]
public class fovEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyClass enemy = (EnemyClass)target;

        Color x = Color.red;
        Handles.color = new Color(x.r, x.g, x.b, 0.1f);
        Handles.DrawSolidArc(
            enemy.transform.position,
            enemy.transform.forward,
            Quaternion.AngleAxis(-enemy.info.fovAngle / 2, enemy.transform.forward) * enemy.transform.right,
            enemy.info.fovAngle,
            enemy.info.fovRange);

        Handles.color = Color.white;
        enemy.info.fovRange = Handles.ScaleValueHandle(
            enemy.info.fovRange,
            enemy.transform.position + enemy.transform.right * enemy.info.fovRange,
            enemy.transform.rotation,
            2,
            Handles.SphereHandleCap,
            1);
    }
}
