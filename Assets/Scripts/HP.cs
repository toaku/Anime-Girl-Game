using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public event EventHandler onHit;
    public event EventHandler onDie;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }
}
