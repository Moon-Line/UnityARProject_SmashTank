using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 화면을 터치했을 때, 처음 터치한 지점을 중심으로 손가락을 슬라이드한 후, 뗀 방향으로 탱크를 날리고 싶다.
// 화면을 터치한 뒤, 슬라이드 하면 탱크가 들리고, 날아간 후, 위를 보게 돌아온다.

public class PlayerTouchMove : MonoBehaviour
{
    public enum State
    {
        Idle,
        Down,
    }
    public State state;



    //public LineRenderer lr_Touch;
    public LineRenderer lr_GuideArrow;

    // - 처음위치를 저장하는 변수, - 현재위치를 저장하는 변수, - 날아가는 방향(처음위치 - 현재위치)을 저장하는 변수 
    Vector3 firstTouch, currentTouch, dir;

    Rigidbody rig;

    // - 터치 중인 상태인지 확인
    bool isDrag;

    // - 터치 후 당긴 거리만큼 힘이 커져야함
    float force;
    public float maxForce = 20f;
    float power;
    Vector3 arrowDir;

    RaycastHit hitinfo;

    // Idle상태인지 확인
    bool isIdle;

    // PowerGageUI
    public Image image_PowerGage;
    public GameObject Btn_Fire;
    bool isFillingGage;
    bool isGageActive;
    float stopPower;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;

        lr_GuideArrow.enabled = false;
        Btn_Fire.SetActive(false);
        image_PowerGage.fillAmount = 0;
        isFillingGage = true;
        isGageActive = false;
        //lr_Touch.enabled = false;
        isDrag = false;
        isIdle = true;
        rig = GetComponent<Rigidbody>();

        Color c = new Color(0f, 1f, 0f);
        lr_GuideArrow.material.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTouch();

        // 아무것도 하지 않고 있을 시, 탱크가 뒤집히지 않은 채로 있게 함
        if (isIdle)
        {
            Vector3 pos = new Vector3(0, transform.localEulerAngles.y, 0);
            transform.eulerAngles = pos;
        }

    }

    private void UpdateTouch()
    {
        // 위치를 확인하기 위한 Ray 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layer = 1 << LayerMask.NameToLayer("TouchScreen");
        bool raycast = Physics.Raycast(ray, out hitinfo, float.MaxValue, layer);

        // 1. 만약 화면을 터치를 했을 때
        if (state == State.Idle && Input.GetMouseButtonDown(0)) // AR 화면 터치까지 가능하다
        {
            // UI가 있으면 밑에는 실행을 안함
            if (EventSystem.current.IsPointerOverGameObject()) return; // PC에서 클릭 시

            if (Input.touchCount > 0)
            {    //터치가 1개 이상이면.
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (EventSystem.current.IsPointerOverGameObject(i)) return;
                }
            }

            // 2. 터치한 곳에서 카메라의 앞방향으로 Ray를 만들고
            if (raycast)
            {
                isIdle = false;

                // 3. 처음 터치한 위치를 저장
                firstTouch = hitinfo.point;
                // 확인용 LineRenderer 활성
                //lr_Touch.enabled = true;
                lr_GuideArrow.enabled = true;

                //lr_Touch.SetPosition(0, firstTouch);
                // 4. 만약 화면을 터치를 하고있으면, 현재 위치를 계속 추적
                isDrag = true;
                state = State.Down;
            }
        }

        // 6. 만약 화면에서 손을 뗐을 때, "처음위치 - 현재위치 = 탱크가 날아갈 방향"을 지정

        if (state == State.Down && Input.GetMouseButtonUp(0))
        {
            rig.isKinematic = false;
            isDrag = false;

            dir = firstTouch - currentTouch;
            dir.Normalize();

            // LineRenderer 비활성
            //lr_Touch.enabled = false;
            //lr_GuideArrow.enabled = false;

            Btn_Fire.SetActive(true);
            FillPowerGage();

            state = State.Idle;
        }

        if (isDrag)
        {
            if (raycast)
            {
                // 5. 현재위치를 갱신
                currentTouch = hitinfo.point;
                currentTouch.y = firstTouch.y;
                // 확인용 LineRenderer
                //lr_Touch.SetPosition(1, currentTouch);

                arrowDir = firstTouch - currentTouch;
                //power = arrowDir.magnitude;
                //power = Mathf.Clamp(power, 0f, 2f);

                #region #Arrow색변화
                /*
                Color c = lr_GuideArrow.material.color;
                if (power == 0)
                {
                    c = new Color(0, 1, 0);
                    lr_GuideArrow.material.color = c;
                }
                if (power < 1)
                {
                    c = new Color(power, c.g, 0);
                    lr_GuideArrow.material.color = c;
                }
                else if (1 == power)
                {
                    c = new Color(1, 1, 0);
                    lr_GuideArrow.material.color = c;
                }
                else if (1 <= power && power < 2)
                {
                    c = new Color(c.r, 2 - power, c.b);
                    lr_GuideArrow.material.color = c;
                }
                else if (power == 2)
                {
                    c = new Color(1, 0, 0);
                    lr_GuideArrow.material.color = c;
                }
                */
                #endregion 

                arrowDir.Normalize();
                arrowDir = new Vector3(arrowDir.x, arrowDir.y + 0.01f, arrowDir.z);

                Vector3 offset = new Vector3(0, 0.1f, 0);

                // - 화살표 방향 지정
                lr_GuideArrow.SetPosition(0, transform.localPosition + offset);
                lr_GuideArrow.SetPosition(1, transform.localPosition + offset + arrowDir * 1.5f); //* power);

                //lr_Touch.startWidth = 0.1f * GetComponentInParent<Transform>().localScale.x; ;
                //lr_Touch.endWidth = 0.1f * GetComponentInParent<Transform>().localScale.x; ;
                lr_GuideArrow.startWidth = 0.1f * GetComponentInParent<Transform>().localScale.x;
                lr_GuideArrow.endWidth = 0.1f * GetComponentInParent<Transform>().localScale.x;

                if (firstTouch != currentTouch)
                {
                    // 탱크가 날아갈 방향을 정면으로 바라보며 회전
                    transform.forward = arrowDir;
                    // 탱크가 들렸을 때, Colider로 인해 움직이는 것을 막기위해 물리연산off
                    rig.isKinematic = true;
                    // 당기는 정도에 따라 최대 30도까지 들리도록 회전시킴
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - (power / 2f * 30f), transform.localEulerAngles.y, transform.localEulerAngles.z);
                }
            }
        }
    }

    void Stand()
    {
        isIdle = true;
    }

    //Coroutine Corfire;
    void FillPowerGage()
    {
        if (isGageActive == true)
            return;
        isGageActive = true;
        StartCoroutine("IEPowerGage");
    }

    IEnumerator IEPowerGage()
    {

        while (isGageActive)
        {
            if (isFillingGage)
            {
                image_PowerGage.fillAmount += 0.02f;
                if (image_PowerGage.fillAmount < 0.5f)
                {
                    // r -> 1, g = 1;
                    Color c = new Color(image_PowerGage.fillAmount * 2, 1, 0);
                    image_PowerGage.color = c;
                }
                else if (0.5f <= image_PowerGage.fillAmount)
                {
                    // r = 1, g -> 0;
                    Color c = new Color(1, 1 - (image_PowerGage.fillAmount - 0.5f) * 2, 0);
                    image_PowerGage.color = c;
                }

                if (image_PowerGage.fillAmount == 1)
                {
                    isFillingGage = !isFillingGage;
                }
            }
            else
            {
                image_PowerGage.fillAmount -= 0.02f;
                if (0.5f < image_PowerGage.fillAmount)
                {
                    // r = 1, g 0 -> 1; 0.9 0.8
                    Color c = new Color(1, (1 - image_PowerGage.fillAmount) * 2, 0);
                    image_PowerGage.color = c;
                }
                else if (image_PowerGage.fillAmount <= 0.5f)
                {
                    // r 1->0, g = 1;
                    Color c = new Color((image_PowerGage.fillAmount * 2), 1, 0);
                    image_PowerGage.color = c;
                }

                if (image_PowerGage.fillAmount == 0)
                {
                    isFillingGage = !isFillingGage;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void OnClickFire()
    {
        print("stop");
        isGageActive = false;
        stopPower = Mathf.Clamp01(0.3f + image_PowerGage.fillAmount);
        StopCoroutine("IEPowerGage");

        Fire();
        image_PowerGage.fillAmount = 0;
    }

    void Fire()
    {
        force = stopPower * maxForce;
        rig.AddForce(dir * force, ForceMode.Impulse);
        lr_GuideArrow.enabled = false;
        Btn_Fire.SetActive(false);

        // ※ Invoke를 사용 시, 2.5초가 지나기전에 두번 당기면 탱크가 바로 서는 현상이 발생되지만 턴제 게임을 생각하고 있으므로 2.5초안에 다시 움직일 일이 없으므로 그대로 사용;
        Invoke("Stand", 2.5f);
    }
}
