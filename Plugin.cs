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
    private string statusText = "PCButtons: Off";
    private GUIStyle guiStyle;
    private Collider localPlayerCollider;
    private Collider localPlayerCosmetics;

    void Start()
    {
        cursorObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cursorObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        cursorRenderer = cursorObject.GetComponent<Renderer>();
        cursorRenderer.material.color = Color.red;

        cursorObject.SetActive(false);

        localPlayerCollider = GameObject.Find("Local Gorilla Player").GetComponent<Collider>();
        localPlayerCosmetics = GameObject.Find("GorillaPlayer").GetComponent<Collider>();

        Destroy(cursorObject.GetComponent<Collider>());

        Physics.IgnoreCollision(cursorObject.GetComponent<Collider>(), localPlayerCollider);
        Physics.IgnoreCollision(cursorObject.GetComponent<Collider>(), localPlayerCosmetics);
    }


    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleActivation();
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
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var targetClass = hit.collider.gameObject.GetComponent<GorillaPressableButton>();
            if (targetClass != null)
            {
                targetClass.ButtonActivationWithHand(true);
            }
            var targetClass1 = hit.collider.gameObject.GetComponent<WardrobeFunctionButton>();
            if (targetClass != null)
            {
                targetClass1.ButtonActivation();
            }
            var targetClass2 = hit.collider.gameObject.GetComponent<SoundPostMuteButton>();
            if (targetClass != null)
            {
                targetClass2.ButtonActivationWithHand(true);
            }
            var targetClass3 = hit.collider.gameObject.GetComponent<GorillaKeyboardButton>();
            if (targetClass != null)
            {
                targetClass3.testClick = true;
            }
            var targetClass4 = hit.collider.gameObject.GetComponent<GorillaPlayerScoreboardLine>();
            if (targetClass != null)
            {
                UnityEngine.UI.Text buttonText = targetClass.GetComponentInChildren<UnityEngine.UI.Text>();
                if (buttonText != null)
                {
                    string buttonTextString = buttonText.text;

                    GorillaPlayerLineButton.ButtonType buttonType = GorillaPlayerLineButton.ButtonType.Cancel;
                    switch (buttonTextString)
                    {
                        case "Mute":
                            buttonType = GorillaPlayerLineButton.ButtonType.Mute;
                            break;
                        case "Report":
                            buttonType = GorillaPlayerLineButton.ButtonType.Report;
                            break;
                        case "Hate Speech":
                            buttonType = GorillaPlayerLineButton.ButtonType.HateSpeech;
                            break;
                        case "Toxicity":
                            buttonType = GorillaPlayerLineButton.ButtonType.Toxicity;
                            break;
                        case "Cheating":
                            buttonType = GorillaPlayerLineButton.ButtonType.Cheating;
                            break;
                        case "Cancel":
                            buttonType = GorillaPlayerLineButton.ButtonType.Cancel;
                            break;
                        default:
                            Debug.LogWarning("Unknown button type: " + buttonTextString);
                            break;
                    }

                    targetClass4.PressButton(false, buttonType);
                }

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
    }

    private void UpdateStatusText()
    {
        statusText = $"Cursor Button Activation: {(isActive ? "On" : "Off")}";
    }

    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
        guiStyle.fontSize = 40;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.alignment = TextAnchor.UpperRight;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(Screen.width - 200, 10, 190, 30), statusText, guiStyle);
    }
}
public interface ButtonComponent
{
    void PressButton();
}