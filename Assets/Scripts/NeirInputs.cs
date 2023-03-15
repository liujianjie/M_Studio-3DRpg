using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

public class NeirInputs : MonoBehaviour
{
    [Header("Ω«…´ ‰»Î÷µ")]
    public Vector2 look;

#if !UNITY_IOS || !UNITY_ANDROID
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	public void OnLook(InputValue value)
	{
		if (cursorInputForLook)
		{
			LookInput(value.Get<Vector2>());
		}
	}
#else
    // old input sys if we do decide to have it (most likely wont)...
#endif

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

#if !UNITY_IOS || !UNITY_ANDROID
    private void OnApplicationFocus(bool focus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
#endif
}

