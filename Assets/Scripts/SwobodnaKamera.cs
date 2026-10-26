using UnityEngine;

// Swobodne latanie kamerą: WASD - ruch, Spacja/E - w górę, Ctrl/Q - w dół,
// Shift - szybciej, mysz - rozglądanie się. Esc odblokowuje kursor, LPM blokuje go ponownie.
public class SwobodnaKamera : MonoBehaviour
{
    [Header("Ruch")]
    [Min(0f)] public float predkosc = 10f;
    [Min(1f)] public float mnoznikSprintu = 3f;

    [Header("Mysz")]
    [Min(0f)] public float czuloscMyszy = 2f;
    public bool blokujKursor = true;

    private float obrotX;
    private float obrotY;

    void Start()
    {
        Vector3 katy = transform.eulerAngles;
        obrotX = katy.x > 180f ? katy.x - 360f : katy.x;
        obrotY = katy.y;

        if (blokujKursor)
            ZablokujKursor(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ZablokujKursor(false);
        else if (blokujKursor && Input.GetMouseButtonDown(0))
            ZablokujKursor(true);

        // Rozglądanie się tylko gdy kursor jest zablokowany albo trzymany jest PPM.
        if (Cursor.lockState == CursorLockMode.Locked || Input.GetMouseButton(1))
        {
            obrotY += Input.GetAxis("Mouse X") * czuloscMyszy;
            obrotX -= Input.GetAxis("Mouse Y") * czuloscMyszy;
            obrotX = Mathf.Clamp(obrotX, -89f, 89f);
            transform.rotation = Quaternion.Euler(obrotX, obrotY, 0f);
        }

        Vector3 kierunek = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) kierunek += transform.forward;
        if (Input.GetKey(KeyCode.S)) kierunek -= transform.forward;
        if (Input.GetKey(KeyCode.D)) kierunek += transform.right;
        if (Input.GetKey(KeyCode.A)) kierunek -= transform.right;
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.E)) kierunek += Vector3.up;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.Q)) kierunek -= Vector3.up;

        float v = predkosc;
        if (Input.GetKey(KeyCode.LeftShift)) v *= mnoznikSprintu;

        transform.position += kierunek.normalized * v * Time.deltaTime;
    }

    private void ZablokujKursor(bool zablokuj)
    {
        Cursor.lockState = zablokuj ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !zablokuj;
    }
}
