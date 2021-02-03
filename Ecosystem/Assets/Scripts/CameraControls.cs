// GENERATED AUTOMATICALLY FROM 'Assets/Controls/CameraControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Object = UnityEngine.Object;

public class CameraControls : IInputActionCollection, IDisposable
{
  // FreeMovement
  private readonly InputActionMap m_FreeMovement;
  private readonly InputAction m_FreeMovement_CancelTarget;
  private readonly InputAction m_FreeMovement_Movement;
  private readonly InputAction m_FreeMovement_Selecting;
  private IFreeMovementActions m_FreeMovementActionsCallbackInterface;

  public CameraControls()
  {
    asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraControls"",
    ""maps"": [
        {
            ""name"": ""FreeMovement"",
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
                    ""path"": ""<Mouse>/press"",
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
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
    // FreeMovement
    m_FreeMovement = asset.FindActionMap("FreeMovement", true);
    m_FreeMovement_Movement = m_FreeMovement.FindAction("Movement", true);
    m_FreeMovement_Selecting = m_FreeMovement.FindAction("Selecting", true);
    m_FreeMovement_CancelTarget = m_FreeMovement.FindAction("CancelTarget", true);
  }

  public InputActionAsset asset { get; }
  public FreeMovementActions FreeMovement => new FreeMovementActions(this);

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

  public struct FreeMovementActions
  {
    private readonly CameraControls m_Wrapper;

    public FreeMovementActions(CameraControls wrapper)
    {
      m_Wrapper = wrapper;
    }

    public InputAction Movement => m_Wrapper.m_FreeMovement_Movement;
    public InputAction Selecting => m_Wrapper.m_FreeMovement_Selecting;
    public InputAction CancelTarget => m_Wrapper.m_FreeMovement_CancelTarget;

    public InputActionMap Get()
    {
      return m_Wrapper.m_FreeMovement;
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

    public static implicit operator InputActionMap(FreeMovementActions set)
    {
      return set.Get();
    }

    public void SetCallbacks(IFreeMovementActions instance)
    {
      if (m_Wrapper.m_FreeMovementActionsCallbackInterface != null)
      {
        Movement.started -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnMovement;
        Movement.performed -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnMovement;
        Movement.canceled -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnMovement;
        Selecting.started -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnSelecting;
        Selecting.performed -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnSelecting;
        Selecting.canceled -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnSelecting;
        CancelTarget.started -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnCancelTarget;
        CancelTarget.performed -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnCancelTarget;
        CancelTarget.canceled -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnCancelTarget;
      }

      m_Wrapper.m_FreeMovementActionsCallbackInterface = instance;
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
      }
    }
  }

  public interface IFreeMovementActions
  {
    void OnMovement(InputAction.CallbackContext context);
    void OnSelecting(InputAction.CallbackContext context);
    void OnCancelTarget(InputAction.CallbackContext context);
  }
}