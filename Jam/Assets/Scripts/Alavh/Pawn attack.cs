using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Pawnattack : MonoBehaviour
{
    public GameObject chessPiece;
    public GameObject redBall;
    public GameObject whiteBall;
    public GameObject player;
    public AlavhAttacks alavhAttacks;
    public List<GameObject> chessPieces;
    private List<GameObject> _bullets = new List<GameObject>();
    private List<GameObject> _bulletsDelete = new List<GameObject>();
    
    private GameObject _gObj;
    public float speed;
    public float timeConstant;
    private Vector3 _position;
    private bool _isFiring;

    private void Start()
    {
        Application.targetFrameRate = 60;
        timeConstant = Time.deltaTime;
    }

    public void PawnAttack(float yCoord)
    {
        StartCoroutine(Attack(yCoord));
    }

    private IEnumerator Attack(float yCoord)
    {
        yield return new WaitForSeconds(2);
        
        foreach (float i in alavhAttacks.xCoord)
        {
            _gObj = Instantiate(chessPiece);
            _gObj.transform.position = new Vector3(i, yCoord, -12.8f);
            _gObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
            chessPieces.Add(_gObj);
        }
        
        alavhAttacks.Disappear();
        yield return new WaitForSeconds(2);
        
        FireBullet(yCoord);
        yield return new WaitForSeconds(2);
        
        FireBullet(yCoord);
        yield return new WaitForSeconds(1);
        
        FireBullet(yCoord);

        yield return new WaitForSeconds(5);
        foreach (GameObject bullet in _bulletsDelete)
        {
            Destroy(bullet);
        }
        foreach (GameObject piece in chessPieces)
        {
            Destroy(piece);
        }

        yield return new WaitForSeconds(5);
        alavhAttacks.isAttacking = true;
    }

    private void Update()
    {
        if (_isFiring)
        {
            foreach (GameObject bullet in _bullets)
            {
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1) * (speed * timeConstant);

                if (bullet.name == redBall.name + "(Clone)")
                {
                    StartCoroutine(ChangeDirection(bullet));
                }
                _bulletsDelete.Add(bullet);
            }
            _bullets.Clear();
        }
    }

    private void FireBullet(float yCoord)
    {
        foreach (float i in alavhAttacks.xCoord)
        {
            float bias = 0.75f;
            if (Random.value < bias)
            {
                _gObj = Instantiate(whiteBall);
            }
            else
            {
                _gObj = Instantiate(redBall);
            }
            
            _gObj.transform.position = new Vector3(i, yCoord, -12.8f);
            _gObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
            _bullets.Add(_gObj);
        }

        _isFiring = true;
    }

    IEnumerator ChangeDirection(GameObject bullet)
    {
        float time = Random.Range(0f, 2f);
        yield return new WaitForSeconds(time);

        Vector2 alvahVector2 = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 bulletVector2 = new Vector2(bullet.transform.position.x, bullet.transform.position.y);

        Vector2 direction = alvahVector2-bulletVector2;
        Vector2 unitVector = direction.normalized;
        
        bullet.GetComponent<Rigidbody2D>().velocity = unitVector * (speed * timeConstant * 1.5f);
    }
}
