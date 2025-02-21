using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // scale 이 적용되기 전의 키, 단위는 unit
    [SerializeField]
    protected float originHeight;
    public float height
    {
        get;
        protected set;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        height = originHeight * transform.localScale.y;
    }

    protected void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
