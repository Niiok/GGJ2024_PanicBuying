using PanicBuying;
using Unity.Netcode;
using UnityEngine;

public class RotatingCoin : NetworkBehaviour, IPickable
{
    public float rotateSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    public void OnPick()
    {

        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        OnPick();
    }
}
