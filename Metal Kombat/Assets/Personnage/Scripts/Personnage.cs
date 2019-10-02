using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personnage : MonoBehaviour
{
    int pointsDeVie;
    public int PointsDeVie
    {
        get { return pointsDeVie; }
        set { pointsDeVie = value; }
    }

    public Personnage()
    {
        pointsDeVie = 0;
    }
    public Personnage(int inPointsDeVie)
    {
        pointsDeVie = inPointsDeVie;
    }

    public void Attack()
    {

    }
    public void Die()
    {

    }
    public void Move()
    {

    }
    public void TakeDammage()
    {

    }



}
