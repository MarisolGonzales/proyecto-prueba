using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canTripleShoot = false;
    public bool faster = false;
    public bool shield = false;

    public int lives = 3;

    //[SerializeField] private int hearts = 3;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShootPrefab;
    [SerializeField] private GameObject _explosionPlayerPrefab;
    [SerializeField] private GameObject _shieldGameObject;

    [SerializeField]
    private float _fireRate = 0.25f;

    private float _canFire = 0.0f;

    [SerializeField]
    private float _speed = 5.0f;


    private UIManager _uiManager;
    private MenuManager _menuManager;

    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_uiManager != null)
        {
            _uiManager.UpdateLives(lives);
        }

        _menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();

    }

    private void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        if (Time.time > _canFire)
        {
            if (canTripleShoot == true)
            {
                Instantiate(_tripleShootPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
            }

            _canFire = Time.time + _fireRate;
        }
        
        /*
        if (canTripleShoot)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
            Instantiate(_laserPrefab, transform.position + new Vector3(0.552f, -0.098f, 0), Quaternion.identity);
            Instantiate(_laserPrefab, transform.position + new Vector3(-0.545f, -0.098f, 0), Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
        */
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);

        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);



        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.2f)
        {
            transform.position = new Vector3(transform.position.x, -4.2f, 0);
        }


        if (transform.position.x < -8.14f)
        {
            transform.position = new Vector3(-8.14f, transform.position.y, 0);
        }
        else if (transform.position.x > 8.14f)
        {
            transform.position = new Vector3(8.14f, transform.position.y, 0);
        }

        if (faster == true)
        {
            transform.Translate(Vector3.right * Time.deltaTime * _speed * 5f * horizontalInput);
            transform.Translate(Vector3.up * Time.deltaTime * _speed * 5f * verticalInput);
        }
        else
        {
            return;
        }


    }

    //TripleShoot
    public void TripleShotPowerupOn()
    {
        canTripleShoot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShoot = false;
    }

    //Speed
    public void speedBoostOn()
    {
        faster = true;
        StartCoroutine(speedBoostRoutine());
    }

    public IEnumerator speedBoostRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        faster = false;
    }

    //Shields

    public void EnableShield()
    {
        shield = true;
        _shieldGameObject.SetActive(true);
    }


    /*private void OnTriggerEnter2D(Collider2D other)
    {
      
        
        
        
       if (other.tag == "Enemy")
        {
            hearts -= 1;

            if (hearts == 0)
            {
                Instantiate(_explosionPlayerPrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }*/

    public void Damage()
    {

        if (shield == true)
        {
            shield = false;
            _shieldGameObject.SetActive(false);
            return;
        }

        lives--;
        _uiManager.UpdateLives(lives);
        
        if (lives <= 0)
        {
            Instantiate(_explosionPlayerPrefab, transform.position, Quaternion.identity);
            _menuManager.gameOver = true;
            _uiManager.ShowTitleScreen();
            Destroy(this.gameObject);
        }
    }
}
