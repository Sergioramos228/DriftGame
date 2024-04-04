using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(ConstantForce))]
public class Player : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _forwardForce;
    [SerializeField] private float _strafeForce;
    [SerializeField] private float _rollForce;
    [SerializeField] private float _sensitivityX;
    [SerializeField] private float _sensitivityY;

    private ConstantForce _force;
    private Rigidbody _rigidbody;
    private PhotonView _photonView;

    private void Awake()
    {
        _force = GetComponent<ConstantForce>();
        _rigidbody = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            _force.relativeForce = Vector3.forward * _forwardForce * Input.GetAxis("Vertical") + Vector3.right * _strafeForce * Input.GetAxis("Horizontal");
            _force.relativeTorque = Vector3.forward * _sensitivityX * Input.GetAxis("Mouse X") + Vector3.right * _sensitivityY * Input.GetAxis("Mouse Y") + Vector3.up * _rollForce * Input.GetAxis("Roll");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
