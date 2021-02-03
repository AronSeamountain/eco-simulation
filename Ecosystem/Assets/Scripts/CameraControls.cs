// GENERATED AUTOMATICALLY FROM 'Assets/Controls/CameraControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CameraControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraControls"",
    ""maps"": [
        {
            ""name"": ""CameraMovement"",
            ""id"": ""a2d4f9a9-2666-4d79-acf6-473892af4188"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6c34e008-8aa5-4fdd-86d5-2e48ab91afa0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Selecting"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b6975455-677d-4f50-bb8a-037cb68eb1c7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CancelTarget"",
                    ""type"": ""Button"",
                    ""id"": ""481897ee-1166-4e61-a621-329ab7ce3350"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StartRotate"",
                    ""type"": ""Button"",
                    ""id"": ""e4bc9521-0404-4e6c-9591-2d8d3182184d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EndRotate"",
                    ""type"": ""Button"",
                    ""id"": ""440097d6-cd83-431e-a9ba-fa1d7d6fe663"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WasdKeys"",
                    ""id"": ""f97ddd26-a985-4366-8217-0ce5b767b2bf"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9cffe9cf-475a-4af7-a6b2-a718f427be5e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ebb44942-936e-49d4-aa9a-1d9fb66cf684"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""41fccece-60ca-4a1e-8692-6af3fed577c9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b35cd731-b34d-4eaf-ad99-beb8ad01e51a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""14841e45-8542-4eed-9f2a-6111b2d2e37d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a65b5ea4-a20b-4da9-8706-e3fec352ff87"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""506d6dbe-6748-4c7c-a1cc-d261b792f220"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b29f3f9e-13b0-447f-889f-491739e92e98"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""26d05df6-4416-4e91-9941-1d3af78e63d9"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""7c5e2028-64bf-4897-8b54-9bafcd820bcc"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selecting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee74e069-4f0a-427f-95d3-2c8340d792b6"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CancelTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8f854a8-f55a-4da8-864d-4a078e7268cb"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66d288d4-5bcf-45f5-8662-b7e3a10d28f2"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EndRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CameraMovement
        m_CameraMovement = asset.FindActionMap("CameraMovement", throwIfNotFound: true);
        m_CameraMovement_Movement = m_CameraMovement.FindAction("Movement", throwIfNotFound: true);
        m_CameraMovement_Selecting = m_CameraMovement.FindAction("Selecting", throwIfNotFound: true);
        m_CameraMovement_CancelTarget = m_CameraMovement.FindAction("CancelTarget", throwIfNotFound: true);
        m_CameraMovement_StartRotate = m_CameraMovement.FindAction("StartRotate", throwIfNotFound: true);
        m_CameraMovement_EndRotate = m_CameraMovement.FindAction("EndRotate", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // CameraMovement
    private readonly InputActionMap m_CameraMovement;
    private ICameraMovementActions m_CameraMovementActionsCallbackInterface;
    private readonly InputAction m_CameraMovement_Movement;
    private readonly InputAction m_CameraMovement_Selecting;
    private readonly InputAction m_CameraMovement_CancelTarget;
    private readonly InputAction m_CameraMovement_StartRotate;
    private readonly InputAction m_CameraMovement_EndRotate;
    public struct CameraMovementActions
    {
        private @CameraControls m_Wrapper;
        public CameraMovementActions(@CameraControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_CameraMovement_Movement;
        public InputAction @Selecting => m_Wrapper.m_CameraMovement_Selecting;
        public InputAction @CancelTarget => m_Wrapper.m_CameraMovement_CancelTarget;
        public InputAction @StartRotate => m_Wrapper.m_CameraMovement_StartRotate;
        public InputAction @EndRotate => m_Wrapper.m_CameraMovement_EndRotate;
        public InputActionMap Get() { return m_Wrapper.m_CameraMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraMovementActions set) { return set.Get(); }
        public void SetCallbacks(ICameraMovementActions instance)
        {
            if (m_Wrapper.m_CameraMovementActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMovement;
                @Selecting.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnSelecting;
                @Selecting.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnSelecting;
                @Selecting.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnSelecting;
                @CancelTarget.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnCancelTarget;
                @CancelTarget.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnCancelTarget;
                @CancelTarget.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnCancelTarget;
                @StartRotate.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnStartRotate;
                @StartRotate.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnStartRotate;
                @StartRotate.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnStartRotate;
                @EndRotate.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnEndRotate;
                @EndRotate.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnEndRotate;
                @EndRotate.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnEndRotate;
            }
            m_Wrapper.m_CameraMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Selecting.started += instance.OnSelecting;
                @Selecting.performed += instance.OnSelecting;
                @Selecting.canceled += instance.OnSelecting;
                @CancelTarget.started += instance.OnCancelTarget;
                @CancelTarget.performed += instance.OnCancelTarget;
                @CancelTarget.canceled += instance.OnCancelTarget;
                @StartRotate.started += instance.OnStartRotate;
                @StartRotate.performed += instance.OnStartRotate;
                @StartRotate.canceled += instance.OnStartRotate;
                @EndRotate.started += instance.OnEndRotate;
                @EndRotate.performed += instance.OnEndRotate;
                @EndRotate.canceled += instance.OnEndRotate;
            }
        }
    }
    public CameraMovementActions @CameraMovement => new CameraMovementActions(this);
    public interface ICameraMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnSelecting(InputAction.CallbackContext context);
        void OnCancelTarget(InputAction.CallbackContext context);
        void OnStartRotate(InputAction.CallbackContext context);
        void OnEndRotate(InputAction.CallbackContext context);
    }
}
