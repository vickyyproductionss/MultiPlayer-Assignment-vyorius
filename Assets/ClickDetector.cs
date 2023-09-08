using Photon.Pun;
using TMPro;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
	private Camera mainCamera;
	private int ObjectsDestroyed = 0;
	public int TotalBoxes = 10;
	private PhotonView photonView;
	[SerializeField] GameObject WinPanel;
	[SerializeField] GameObject LosePanel;
	public TMP_Text BoxesLeftText;
	[SerializeField] TMP_Text ScoreText;
	[SerializeField] TMP_Text WinnerNameText;

	public static ClickDetector Instance;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		photonView = GetComponent<PhotonView>();
		mainCamera = Camera.main;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider != null)
				{
					HandleClick(hit.collider.gameObject);
				}
			}
		}
	}

	private void HandleClick(GameObject clickedObject)
	{
		DestroyMeOverNetwork destroyHandler = clickedObject.GetComponent<DestroyMeOverNetwork>();
		if(destroyHandler != null)
		{
			destroyHandler.DestroyObject();
			ObjectsDestroyed++;
			ScoreText.text = $"Score: {ObjectsDestroyed}";
			CheckWinningCase(ObjectsDestroyed);
		}
	}
	void CheckWinningCase(int count)
	{
		if(count >= 8)
		{
			photonView.RPC("GameFinish", RpcTarget.Others,PhotonNetwork.NickName);
			WinPanel.SetActive(true);
			Invoke("ReturnToHomeScene", 3);
		}
	}
	[PunRPC]
	private void GameFinish(string winnerName)
	{
		WinnerNameText.text = $"Winner is {winnerName}";
		LosePanel.SetActive(true);
		Invoke("ReturnToHomeScene", 3);
	}

	void ReturnToHomeScene()
	{
		if(PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LoadLevel("Menu");
		}
	}
}
