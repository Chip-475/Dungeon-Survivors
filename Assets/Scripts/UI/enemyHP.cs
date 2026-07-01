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
        parentrans = gameObject.transform.parent.parent.parent;
    }
    void Update()
    {
        if (enemyClass == null) return;
        hpBar.fillAmount = enemyClass.hp/enemyClass.hpMax;
        float scalex = parentrans.localScale.x;

        if (scalex<0)
        {
            GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
 
    
}
