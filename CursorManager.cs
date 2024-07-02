using UnityEngine;
using NaughtyAttributes;

public class CursorManager : MonoBehaviour
{
    public PlayerSelectedObjectUI PSOUI;
    [SerializeField] private Vector3 mousePosition;

    [SerializeField] private Vector3 centerBaseMousePosition;

    [Range(-1, 1)]
    public float X;
    [Range(-1, 1)]
    public float y;

    [SerializeField] private Texture2D cursorImage;
    [SerializeField] private Vector3 cursorImageOffset;


    [ReadOnly]
    public GameObject trueCursorPointAtObject;



    [Header("Cursor Target Ignoring Player")]
    [SerializeField] private float Range = 100;
    [ReadOnly]
    public GameObject cursorPointAtObjectIgnoringPlayer;

    [Tooltip("These are layers that should be used when ignoring player")]
    [SerializeField] private LayerMask PlayerLayerMask;

    [SerializeField] private GameObject CursorGO;
    private RectTransform cursor;
    private Animator cursorAnim;
    [SerializeField] private RectTransform Canvas;

    [ReadOnly]
    public GameObject LockedTarget;
    public bool TargetLocked = false;

    private float mouseButtonTimePressed = 0;
    [SerializeField] private float timeRequiredToSwitchPlayer = 2;
    // Start is called before the first frame update
    void Start()
    {
        cursor = CursorGO.GetComponent<RectTransform>();
        cursorAnim = CursorGO.GetComponent<Animator>();

        if (cursorImage!=null)
        {
            Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Debug.LogWarning("Cursor image has not been assigned. Assign it!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = (Input.mousePosition - Canvas.localPosition);

        cursor.localPosition = new Vector3(mousePos.x, mousePos.y, mousePos.z) + cursorImageOffset;

        mousePosition = Input.mousePosition;
        centerBaseMousePosition.x = mousePosition.x - (1920 / 2);
        centerBaseMousePosition.y = mousePosition.y - (1080 / 2);

        X = centerBaseMousePosition.x / 1920 * 2;
        y = centerBaseMousePosition.y / 1080 * 2;

        findTrueCursorPointAtObject();
        findCursorPointAtObjectIgnoringPlayer();

        ManageTargetLocking();
        SwitchPlayer();
    }

    private void ManageTargetLocking()
    {
        if (cursorPointAtObjectIgnoringPlayer!= null)
        {
            if (cursorPointAtObjectIgnoringPlayer.GetComponent<Identification>())
            {
                if (cursorPointAtObjectIgnoringPlayer.GetComponent<Identification>().isAUnit == true)
                {
                    
                    if (Input.GetMouseButtonDown(1) && LockedTarget == null)
                    {
                        LockedTarget = cursorPointAtObjectIgnoringPlayer;
                        cursorAnim.SetTrigger("Lock Target");
                        TargetLocked = true;
                    }

                    if (Input.GetMouseButton(1) && LockedTarget!=null)
                    {
                        mouseButtonTimePressed += Time.deltaTime;

                        LockOnToTarget();
                        PSOUI.AssignTargetDetails(cursorPointAtObjectIgnoringPlayer);
                    }

                    else if (Input.GetMouseButtonUp(1) && LockedTarget != null)
                    {
                        UnlockTarget();
                        PSOUI.ClearValues();
                    }

                    if (LockedTarget == null && cursorAnim.GetCurrentAnimatorStateInfo(0).IsName("TargetLocked"))
                    {
                        UnlockTarget();
                        PSOUI.ClearValues();
                    }

                }
            }

            //If cursor is not the locked target
            if (cursorPointAtObjectIgnoringPlayer!=LockedTarget)
            {
                if (TargetLocked == true && Input.GetMouseButton(1))
                {
                    LockOnToTarget();
                    mouseButtonTimePressed += Time.deltaTime;
                }
            }
        }

    }


    private void SwitchPlayer()
    {
        if (mouseButtonTimePressed >= timeRequiredToSwitchPlayer && LockedTarget!=null)
        {
            if (PlayerManager.playerTeam.Contains(LockedTarget))
            {
                PlayerManager.CurrentPlayer = LockedTarget;
                UnlockTarget();
                PSOUI.ClearValues();
            }
        }
    }

    private void LockOnToTarget()
    {
        if (LockedTarget!=null)
        {
            Vector3 targetPos = LockedTarget.transform.position;
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camPos = Camera.main.transform.position + camForward;

            float distInFrontOfCamera = Vector3.Dot(targetPos - camPos, camForward);
            if (distInFrontOfCamera < 0f)
            {
                targetPos -= camForward * distInFrontOfCamera;
            }

            cursor.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPos);
        }
    }

    private void UnlockTarget()
    {
        mouseButtonTimePressed = 0;
        LockedTarget = null;
        cursorAnim.SetTrigger("Unlock Target");
        TargetLocked = false;
    }



    private void findTrueCursorPointAtObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit RayHit = new RaycastHit();
        Vector3 HitPoint = new Vector3();
        if (Physics.Raycast(ray, out RayHit))
        {
            GameObject ObjectHit = RayHit.transform.gameObject;
            HitPoint = RayHit.point;
            if (ObjectHit!=null)
            {
                Debug.DrawLine(Camera.main.transform.position, HitPoint, Color.black);
                trueCursorPointAtObject = ObjectHit;
            }
        }
    }

    private void findCursorPointAtObjectIgnoringPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit RayHit = new RaycastHit();
        Vector3 HitPoint = new Vector3();

        if (Physics.Raycast(ray, out RayHit, Range,PlayerLayerMask))
        {
            GameObject ObjectHit = RayHit.transform.gameObject;
            HitPoint = RayHit.point;
            if (ObjectHit != null)
            {
                Debug.DrawLine(Camera.main.transform.position, HitPoint, Color.green);
                cursorPointAtObjectIgnoringPlayer = ObjectHit;
            }
        }
    }
}
