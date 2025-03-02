using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
	[SerializeField]
	private	KeyCode		keyCodeFire = KeyCode.Space;
	[SerializeField]
	private	GameObject	projectilePrefab;
	private	float		moveSpeed = 3;
	private	Vector3		lastMoveDirection = Vector3.right;

	public	bool		IsMoved		{ set; get; } = false;	// �̵� ���� ����
	public	bool		IsAttacked	{ set; get; } = false;	// ���� ���� ����

	private void Update()
	{
		if ( IsMoved == true )
		{
			// �÷��̾� ������Ʈ �̵�
			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");

			transform.position += new Vector3(x, y, 0) * moveSpeed * Time.deltaTime;

			// �������� �Էµ� ����Ű�� ������ �Ѿ��� �߻� �������� Ȱ��
			if ( x != 0 || y != 0 )
			{
				lastMoveDirection = new Vector3(x, y, 0);
			}
		}

		if ( IsAttacked == true )
		{
			// �����̽� Ű�� ���� �߻�ü ����
			if ( Input.GetKeyDown(keyCodeFire) )
			{
				GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
				clone.GetComponent<Projectile>().Setup(lastMoveDirection);
			}
		}
	}
}

