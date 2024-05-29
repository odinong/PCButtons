using BepInEx;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using GorillaNetworking;

[BepInPlugin("com.odin.PCButtons", "PCButtons", "1.0.0")]
public class PCButtons : BaseUnityPlugin
{
    private bool isActive = false;
    private GameObject cursorObject;
    private Renderer cursorRenderer;
    private string statusText = "PCButtons: False";
    private string statusText2 = "Right Hand: False";
    private GUIStyle guiStyle;
    private Collider localPlayerCollider;
    private Collider localPlayerCosmetics;
    private bool isRightHandEnabled;
    private bool isActive2 = false;

    void Start()
    {
        cursorObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cursorObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        cursorRenderer = cursorObject.GetComponent<Renderer>();
        cursorRenderer.material.color = Color.red;

        cursorObject.SetActive(false);

        localPlayerCollider = GameObject.Find("Local Gorilla Player").GetComponent<Collider>();
        localPlayerCosmetics = GameObject.Find("GorillaPlayer").GetComponent<Collider>();

        Destroy(cursorObject.GetComponent<Collider>());

        Physics.IgnoreCollision(cursorObject.GetComponent<Collider>(), localPlayerCollider);
        Physics.IgnoreCollision(cursorObject.GetComponent<Collider>(), localPlayerCosmetics);
        Debug.Log("Started PCButtons!");
    }


    void Update()
    {
        if (Keyboard.current.leftAltKey.wasPressedThisFrame)
        {
            ToggleActivation();
        }
        
        if (Keyboard.current.rightAltKey.wasPressedThisFrame)
        {
            isRightHandEnabled = !isRightHandEnabled;
            isActive2 = !isActive2;
            UpdateStatusText2();
        }

        if (isActive)
        {
            UpdateCursorPosition();

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                cursorRenderer.material.color = Color.green;
                HandleMouseClick();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                cursorRenderer.material.color = Color.red;
            }
        }
    }

    private void ToggleActivation()
    {
        isActive = !isActive;
        cursorObject.SetActive(isActive);
        UpdateStatusText();
    }

    private void HandleMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit) && isRightHandEnabled == false)
        {
            var targetClass = hit.collider.gameObject.GetComponent<GorillaPressableButton>();
            if (targetClass != null)
            {
                targetClass.ButtonActivationWithHand(true);
                Debug.Log("Clicked Button");
            }
            var targetClass1 = hit.collider.gameObject.GetComponent<WardrobeFunctionButton>();
            if (targetClass1 != null)
            {
                targetClass1.ButtonActivation();
                Debug.Log("Clicked Button");
            }
            var targetClass2 = hit.collider.gameObject.GetComponent<SoundPostMuteButton>();
            if (targetClass2 != null)
            {
                targetClass2.ButtonActivation();
                Debug.Log("Clicked Button");
            }
            var targetClass3 = hit.collider.gameObject.GetComponent<GorillaKeyboardButton>();
            if (targetClass3 != null)
            {
                targetClass3.testClick = true;
                targetClass3.Update();
                Debug.Log("Clicked Button");
            }
        }
        if (Physics.Raycast(ray, out RaycastHit hit2) && isRightHandEnabled == true)
        {
            var targetClass = hit.collider.gameObject.GetComponent<GorillaPressableButton>();
            if (targetClass != null)
            {
                targetClass.ButtonActivationWithHand(false);
                Debug.Log("Clicked Button");
            }
            var targetClass1 = hit.collider.gameObject.GetComponent<WardrobeFunctionButton>();
            if (targetClass1 != null)
            {
                targetClass1.ButtonActivationWithHand(false);
                targetClass1.UpdateColor();
                Debug.Log("Clicked Button");
            }
            var targetClass2 = hit.collider.gameObject.GetComponent<SoundPostMuteButton>();
            if (targetClass2 != null)
            {
                targetClass2.ButtonActivation();
                Debug.Log("Clicked Button");
            }
            var targetClass3 = hit.collider.gameObject.GetComponent<GorillaKeyboardButton>();
            if (targetClass3 != null)
            {
                targetClass3.testClick = true;
                targetClass3.Update();
                Debug.Log("Clicked Button");
            }
        }
    }

    private void UpdateCursorPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            cursorObject.transform.position = hit.point;
        }
        if (Physics.Raycast(ray, out RaycastHit hit2))
        {
            cursorObject.transform.position = hit.point;
        }
    }

    private void UpdateStatusText()
    {
        statusText = "PCButtons: " + isActive;
        Debug.Log("Switched Mod Activity");
    }
    private void UpdateStatusText2()
    {
        statusText2 = "Right Hand: " + isActive2;
        Debug.Log("Switched Hands");
    }

    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
        guiStyle.fontSize = 20;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.alignment = TextAnchor.UpperRight;
        if (isActive == false)
        {
            guiStyle.normal.textColor = Color.red;
        }
        if (isActive == true)
        {
            guiStyle.normal.textColor = Color.green;
        }
        GUI.Label(new Rect(Screen.width - 200, 10, 190, 30), statusText, guiStyle);

        GUIStyle guiStyle2 = new GUIStyle(GUI.skin.label);
        guiStyle2.fontSize = 20;
        guiStyle2.fontStyle = FontStyle.Bold;
        guiStyle2.alignment = TextAnchor.UpperRight;
        if (isActive2 == false)
        {
            guiStyle2.normal.textColor = Color.red;
        }
        if (isActive2 == true)
        {
            guiStyle2.normal.textColor = Color.green;
        }
        GUI.Label(new Rect(Screen.width - 200, 40, 190, 30), statusText2, guiStyle2);
        GUIStyle guiStyle22 = new GUIStyle(GUI.skin.label);
        guiStyle22.fontSize = 20;
        guiStyle22.fontStyle = FontStyle.Bold;
        guiStyle22.alignment = TextAnchor.UpperRight;
        guiStyle22.normal.textColor = Color.magenta;
        GUI.Label(new Rect(Screen.width - 150, 90, 300, 300), "Controls: \nLeft Alt: Enable The Mod\nRight Alt: Toggle Hand\nMade By Odin (lbaak. on discord)", guiStyle22);
    }
}