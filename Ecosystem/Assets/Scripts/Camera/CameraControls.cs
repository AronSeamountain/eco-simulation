// GENERATED AUTOMATICALLY FROM 'Assets/Controls/CameraControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Object = UnityEngine.Object;

namespace Camera
{
  public class CameraControls : IInputActionCollection, IDisposable
  {
    // CameraMovement
    private readonly InputActionMap m_CameraMovement;
    private readonly InputAction m_CameraMovement_CancelTarget;
    private readonly InputAction m_CameraMovement_MoveFast;
    private readonly InputAction m_CameraMovement_Movement;
    private readonly InputAction m_CameraMovement_Rotate;
    private readonly InputAction m_CameraMovement_Selecting;
    private readonly InputAction m_CameraMovement_View;
    private ICameraMovementActions m_CameraMovementActionsCallbackInterface;

    public CameraControls()
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
                    ""name"": ""Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""e4bc9521-0404-4e6c-9591-2d8d3182184d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""View"",
                    ""type"": ""PassThrough"",
                    ""id"": ""93ebb323-4738-4986-9621-f6be66d7bcfb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveFast"",
                    ""type"": ""Button"",
                    ""id"": ""398919cc-1577-4bc1-afee-32f30f9d65fd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
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
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""48fca672-9762-45ff-8bce-f5a978764b35"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""View"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18d7cc31-e7a5-4b60-931b-e92a9bec1100"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveFast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
      // CameraMovement
      m_CameraMovement = asset.FindActionMap("CameraMovement", true);
      m_CameraMovement_Movement = m_CameraMovement.FindAction("Movement", true);
      m_CameraMovement_Selecting = m_CameraMovement.FindAction("Selecting", true);
      m_CameraMovement_CancelTarget = m_CameraMovement.FindAction("CancelTarget", true);
      m_CameraMovement_Rotate = m_CameraMovement.FindAction("Rotate", true);
      m_CameraMovement_View = m_CameraMovement.FindAction("View", true);
      m_CameraMovement_MoveFast = m_CameraMovement.FindAction("MoveFast", true);
    }

    public InputActionAsset asset { get; }
    public CameraMovementActions CameraMovement => new CameraMovementActions(this);

    public void Dispose()
    {
      Object.Destroy(asset);
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

    public struct CameraMovementActions
    {
      private readonly CameraControls m_Wrapper;

      public CameraMovementActions(CameraControls wrapper)
      {
        m_Wrapper = wrapper;
      }

      public InputAction Movement => m_Wrapper.m_CameraMovement_Movement;
      public InputAction Selecting => m_Wrapper.m_CameraMovement_Selecting;
      public InputAction CancelTarget => m_Wrapper.m_CameraMovement_CancelTarget;
      public InputAction Rotate => m_Wrapper.m_CameraMovement_Rotate;
      public InputAction View => m_Wrapper.m_CameraMovement_View;
      public InputAction MoveFast => m_Wrapper.m_CameraMovement_MoveFast;

      public InputActionMap Get()
      {
        return m_Wrapper.m_CameraMovement;
      }

      public void Enable()
      {
        Get().Enable();
      }

      public void Disable()
      {
        Get().Disable();
      }

      public bool enabled => Get().enabled;

      public static implicit operator InputActionMap(CameraMovementActions set)
      {
        return set.Get();
      }

      public void SetCallbacks(ICameraMovementActions instance)
      {
        if (m_Wrapper.m_CameraMovementActionsCallbackInterface != null)
        {
          Movement.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMovement;
          Movement.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMovement;
          Movement.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMovement;
          Selecting.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnSelecting;
          Selecting.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnSelecting;
          Selecting.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnSelecting;
          CancelTarget.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnCancelTarget;
          CancelTarget.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnCancelTarget;
          CancelTarget.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnCancelTarget;
          Rotate.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnRotate;
          Rotate.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnRotate;
          Rotate.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnRotate;
          View.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnView;
          View.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnView;
          View.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnView;
          MoveFast.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMoveFast;
          MoveFast.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMoveFast;
          MoveFast.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMoveFast;
        }

        m_Wrapper.m_CameraMovementActionsCallbackInterface = instance;
        if (instance != null)
        {
          Movement.started += instance.OnMovement;
          Movement.performed += instance.OnMovement;
          Movement.canceled += instance.OnMovement;
          Selecting.started += instance.OnSelecting;
          Selecting.performed += instance.OnSelecting;
          Selecting.canceled += instance.OnSelecting;
          CancelTarget.started += instance.OnCancelTarget;
          CancelTarget.performed += instance.OnCancelTarget;
          CancelTarget.canceled += instance.OnCancelTarget;
          Rotate.started += instance.OnRotate;
          Rotate.performed += instance.OnRotate;
          Rotate.canceled += instance.OnRotate;
          View.started += instance.OnView;
          View.performed += instance.OnView;
          View.canceled += instance.OnView;
          MoveFast.started += instance.OnMoveFast;
          MoveFast.performed += instance.OnMoveFast;
          MoveFast.canceled += instance.OnMoveFast;
        }
      }
    }

    public interface ICameraMovementActions
    {
      void OnMovement(InputAction.CallbackContext context);
      void OnSelecting(InputAction.CallbackContext context);
      void OnCancelTarget(InputAction.CallbackContext context);
      void OnRotate(InputAction.CallbackContext context);
      void OnView(InputAction.CallbackContext context);
      void OnMoveFast(InputAction.CallbackContext context);
    }
  }
}