// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Unit Scripts/Player Scripts/InputActions/PlayerInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""PlayerControl"",
            ""id"": ""b1ad847c-00df-4e42-a21e-de7f1e61709c"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""fba601af-8a96-4a96-92ff-a6991b312909"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""2c07ecee-f4db-4c0d-aa27-8d5634211398"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""540367f8-d1bb-4870-b3e2-32ac38fa919d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""2220fc03-f45c-4321-970a-3215eb00f551"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack 1"",
                    ""type"": ""Button"",
                    ""id"": ""af2278a4-f1c4-413f-ba70-f75d7ae2bba6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack 2"",
                    ""type"": ""Button"",
                    ""id"": ""726b3385-2b63-4be1-b644-25fd8ff7547d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack 3"",
                    ""type"": ""Button"",
                    ""id"": ""2d06b76a-c763-4971-b69a-c83e6f3bbe55"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Health Potion"",
                    ""type"": ""Button"",
                    ""id"": ""5533b8c5-b7bc-468c-b35e-aff62119790e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mana Potion"",
                    ""type"": ""Button"",
                    ""id"": ""e2f623b6-cedb-4880-be24-601d76f4f819"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""4b68d066-c287-4cd1-9fde-9311f4102ea4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""37bc5a42-8eb9-4515-ae69-893ccd75de71"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9974f77a-2ac7-4ea1-bb46-281ff6499ed6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""b8fed097-f9c1-46a2-aa18-ff152dc10537"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""37011c70-cdbb-4dce-8ad7-ece28c3490ef"",
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
                    ""id"": ""77649310-0960-43d2-8df4-d2a182de5d8a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6c54af89-c777-4d51-90b9-25ec94262d3c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6843fe4a-3aab-4ac9-8f37-a35ff8654927"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ad73d1d-9c50-4701-8e70-8dd4b2935464"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""833f1b6d-b570-4700-800a-37efc822c9a3"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4d0c513-7702-4cf4-9260-5d511b2c93df"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Health Potion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a871c80c-a46e-40c7-9368-b89ad17630e1"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mana Potion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6a0e355-fe88-4b09-96e9-3ecca218c3f5"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerControl
        m_PlayerControl = asset.FindActionMap("PlayerControl", throwIfNotFound: true);
        m_PlayerControl_Jump = m_PlayerControl.FindAction("Jump", throwIfNotFound: true);
        m_PlayerControl_Drop = m_PlayerControl.FindAction("Drop", throwIfNotFound: true);
        m_PlayerControl_Movement = m_PlayerControl.FindAction("Movement", throwIfNotFound: true);
        m_PlayerControl_Interact = m_PlayerControl.FindAction("Interact", throwIfNotFound: true);
        m_PlayerControl_Attack1 = m_PlayerControl.FindAction("Attack 1", throwIfNotFound: true);
        m_PlayerControl_Attack2 = m_PlayerControl.FindAction("Attack 2", throwIfNotFound: true);
        m_PlayerControl_Attack3 = m_PlayerControl.FindAction("Attack 3", throwIfNotFound: true);
        m_PlayerControl_HealthPotion = m_PlayerControl.FindAction("Health Potion", throwIfNotFound: true);
        m_PlayerControl_ManaPotion = m_PlayerControl.FindAction("Mana Potion", throwIfNotFound: true);
        m_PlayerControl_Pause = m_PlayerControl.FindAction("Pause", throwIfNotFound: true);
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

    // PlayerControl
    private readonly InputActionMap m_PlayerControl;
    private IPlayerControlActions m_PlayerControlActionsCallbackInterface;
    private readonly InputAction m_PlayerControl_Jump;
    private readonly InputAction m_PlayerControl_Drop;
    private readonly InputAction m_PlayerControl_Movement;
    private readonly InputAction m_PlayerControl_Interact;
    private readonly InputAction m_PlayerControl_Attack1;
    private readonly InputAction m_PlayerControl_Attack2;
    private readonly InputAction m_PlayerControl_Attack3;
    private readonly InputAction m_PlayerControl_HealthPotion;
    private readonly InputAction m_PlayerControl_ManaPotion;
    private readonly InputAction m_PlayerControl_Pause;
    public struct PlayerControlActions
    {
        private @PlayerInputs m_Wrapper;
        public PlayerControlActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_PlayerControl_Jump;
        public InputAction @Drop => m_Wrapper.m_PlayerControl_Drop;
        public InputAction @Movement => m_Wrapper.m_PlayerControl_Movement;
        public InputAction @Interact => m_Wrapper.m_PlayerControl_Interact;
        public InputAction @Attack1 => m_Wrapper.m_PlayerControl_Attack1;
        public InputAction @Attack2 => m_Wrapper.m_PlayerControl_Attack2;
        public InputAction @Attack3 => m_Wrapper.m_PlayerControl_Attack3;
        public InputAction @HealthPotion => m_Wrapper.m_PlayerControl_HealthPotion;
        public InputAction @ManaPotion => m_Wrapper.m_PlayerControl_ManaPotion;
        public InputAction @Pause => m_Wrapper.m_PlayerControl_Pause;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlActions instance)
        {
            if (m_Wrapper.m_PlayerControlActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnJump;
                @Drop.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnDrop;
                @Movement.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnMovement;
                @Interact.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnInteract;
                @Attack1.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack1;
                @Attack1.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack1;
                @Attack1.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack1;
                @Attack2.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack2;
                @Attack2.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack2;
                @Attack2.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack2;
                @Attack3.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack3;
                @Attack3.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack3;
                @Attack3.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack3;
                @HealthPotion.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnHealthPotion;
                @HealthPotion.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnHealthPotion;
                @HealthPotion.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnHealthPotion;
                @ManaPotion.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnManaPotion;
                @ManaPotion.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnManaPotion;
                @ManaPotion.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnManaPotion;
                @Pause.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_PlayerControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Attack1.started += instance.OnAttack1;
                @Attack1.performed += instance.OnAttack1;
                @Attack1.canceled += instance.OnAttack1;
                @Attack2.started += instance.OnAttack2;
                @Attack2.performed += instance.OnAttack2;
                @Attack2.canceled += instance.OnAttack2;
                @Attack3.started += instance.OnAttack3;
                @Attack3.performed += instance.OnAttack3;
                @Attack3.canceled += instance.OnAttack3;
                @HealthPotion.started += instance.OnHealthPotion;
                @HealthPotion.performed += instance.OnHealthPotion;
                @HealthPotion.canceled += instance.OnHealthPotion;
                @ManaPotion.started += instance.OnManaPotion;
                @ManaPotion.performed += instance.OnManaPotion;
                @ManaPotion.canceled += instance.OnManaPotion;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public PlayerControlActions @PlayerControl => new PlayerControlActions(this);
    public interface IPlayerControlActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnDrop(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnAttack1(InputAction.CallbackContext context);
        void OnAttack2(InputAction.CallbackContext context);
        void OnAttack3(InputAction.CallbackContext context);
        void OnHealthPotion(InputAction.CallbackContext context);
        void OnManaPotion(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
