using System.Collections.Generic;
using UnityEngine; 

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float mouseSensitivity = 100.0f; //���콺 ����
    
    public Transform camTransform;
    public CharacterController characterController;
    public Transform playerHead; //�÷��̾� �Ӹ� ��ġ(1��Ī cam)
    public float thirdPersonDistance = 3.0f; //�÷��̾� - cam �Ÿ�
    public Vector3 thirdPersonOffset = new Vector3(0f, 1.5f, 0f); 
    public Transform playerLookObj; //�÷��̾� �þ� ��ġ (��� �����)

    public float zoomDistance = 1.0f; //3��Ī
    public float zoomSpeed = 5.0f;
    public float defaultFov = 60.0f;
    public float zoomFov = 30.0f; //Ȯ�� �� ī�޶� �þ߰� (1��Ī)


    private void Start()
    {

    }

    private void Update()
    {
        
    }
}
