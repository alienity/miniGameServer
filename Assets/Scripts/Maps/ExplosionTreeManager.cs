using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTreeManager : MonoBehaviour
{

    // 要生成南瓜的位置
    public List<Transform> explosionTrans;
    // 爆炸树原型
    public ExplosionTree explosionTree;
    // 管理所有的爆炸树
    private List<ExplosionTree> explosionTreeInstances;
    
	void Start ()
    {
        explosionTreeInstances = new List<ExplosionTree>();
        foreach (Transform trans in explosionTrans)
        {
            ExplosionTree et = Instantiate(explosionTree, transform);
            et.transform.position = trans.position;
            explosionTreeInstances.Add(et);
        }
	}
	
	void Update ()
    {
		
	}

    public void ChangeWeather()
    {
        foreach (ExplosionTree et in explosionTreeInstances)
        {
            //et.ExchangeModel();
        }
    }

}
