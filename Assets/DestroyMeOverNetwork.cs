using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DestroyMeOverNetwork : MonoBehaviour
{
	private PhotonView photonView;
	private void Start()
	{
		photonView = GetComponent<PhotonView>();
	}

	public void DestroyObject()
	{
		photonView.RPC("DestroyObjectRPC", RpcTarget.All, gameObject.name);
	}

	[PunRPC]
	private void DestroyObjectRPC(string objName)
	{
		GameObject objToDestroy = GameObject.Find(objName);

		if (objToDestroy != null)
		{
			ClickDetector.Instance.BoxesLeftText.text = $"Total boxes: {--ClickDetector.Instance.TotalBoxes}";
			PhotonNetwork.Destroy(objToDestroy);
		}
	}
}
