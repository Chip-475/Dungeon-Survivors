using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class enemyHP : MonoBehaviour
{
    public enemyClass enemyClass;
    public Image hpBar;
    public Quaternion rot;
    private Transform parentrans;
    /*
     *
    void Start()
    {
        parentrans = this.transform.parent.parent.parent;
    }
    void Update()
    {
        if (enemyClass == null) return;
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
    */
    void Update()
    {
        if (enemyClass == null) return;
        hpBar.fillAmount = enemyClass.hp / enemyClass.hpMax;
        float scalex = enemyClass.transform.localScale.x;//cosi la catena non riscia di essere null (giulio shit)
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (scalex < 0) rectTransform.localScale = new Vector3(-1, 1, 1);
        else rectTransform.localScale=new Vector3(1, 1, 1);
    }
}
