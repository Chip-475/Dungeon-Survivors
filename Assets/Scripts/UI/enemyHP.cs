using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class enemyHP : MonoBehaviour
{
    public enemyClass enemyClass;
    public Image hpBar;
    public Quaternion rot;
    private Transform parentrans;
    void Start()
    {
        parentrans = this.transform.parent.parent.parent;
    }
    void Update()
    {
        hpBar.fillAmount = enemyClass.hp/enemyClass.hpMax;
        float scalex = parentrans.localScale.x;

        if (scalex==-1)
        {
            GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}
