using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [SerializeField]
    private float _maxHP;
    public float maxHP
    {
        get { return _maxHP; }
        private set { _maxHP = value; }
    }
    private float _currentHP;
    public float currentHP
    {
        get
        {
            return _currentHP;
        }
        private set
        {
            if(value > 0)
            {
                _currentHP = value;
            }
            else
            {
                _currentHP = 0;
            }
        }
    }

    [SerializeField]
    private Slider HPBar;

    public event EventHandler onHit;
    public event EventHandler onDie;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        HPBar.maxValue = maxHP;
        HPBar.value = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        FixHPBarUnder();
    }

    public void Hit(float damage)
    {
        if(currentHP > 0)
        {
            currentHP -= damage;

            if (currentHP > 0)
            {
                onHit?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                onDie?.Invoke(this, EventArgs.Empty);
            }

            HPBar.value = currentHP;
        }
    }

    private void FixHPBarUnder()
    {
        if(HPBar != null)
        {
            HPBar.transform.position = gameObject.transform.position + new Vector3(0, -0.2f, 0);
        }
    }
}
