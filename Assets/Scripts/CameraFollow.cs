using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       
    public Vector3 offset;         
    public float followSpeed = 10f; 

    private void Start()
    {
       
        if (player != null)
        {
            offset = transform.position - player.position;
        }
    }

        private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            // Округление позиции камеры до пикселей
            PixelPerfectSnap();
        }
    }

    private void PixelPerfectSnap()
    {
        // Получаем ссылку на Pixel Perfect Camera, если она есть
        var pixelPerfectCamera = GetComponent<UnityEngine.U2D.PixelPerfectCamera>();
        if (pixelPerfectCamera != null)
        {
            float pixelRatio = pixelPerfectCamera.assetsPPU;
            Vector3 snappedPosition = transform.position;
            snappedPosition.x = Mathf.Round(snappedPosition.x * pixelRatio) / pixelRatio;
            snappedPosition.y = Mathf.Round(snappedPosition.y * pixelRatio) / pixelRatio;
            transform.position = snappedPosition;
        }
    }

}
