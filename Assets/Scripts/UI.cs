//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/UI.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @UI: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @UI()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UI"",
    ""maps"": [
        {
            ""name"": ""UIPlayer"",
            ""id"": ""b34d42da-c271-4f5e-a64e-d3daf1dfc32e"",
            ""actions"": [
                {
                    ""name"": ""ToggleScoreboard"",
                    ""type"": ""Button"",
                    ""id"": ""c7ed7c88-0c2b-403e-9104-2c1afa7b2306"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UILeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""1fd06773-d3bd-481b-a900-36dbf2eaad26"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UIRightClick"",
                    ""type"": ""Button"",
                    ""id"": ""16a68b3e-4690-4ccf-96f4-3f903388a819"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleBuyMenu"",
                    ""type"": ""Button"",
                    ""id"": ""826b80fe-f525-4c81-8513-b35abb952d27"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d2c8a17e-ce91-4555-9447-805b8b2114f6"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleScoreboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fc56d572-3676-44b9-aec6-c10f604654fb"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UILeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a46545e-b442-489a-a3c6-aeda48c1b3cc"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UIRightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6aeea0cb-c2fb-4584-9448-77edd43a2f83"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleBuyMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UIGlobal"",
            ""id"": ""e6ce54e8-58d2-400b-926a-dce35f3263b3"",
            ""actions"": [
                {
                    ""name"": ""ToggleTeamSelector"",
                    ""type"": ""Button"",
                    ""id"": ""a3bf7e88-7fb8-464f-95d3-85e99be76550"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""97747de8-97fe-415f-8b68-c2131401dc33"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleTeamSelector"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // UIPlayer
        m_UIPlayer = asset.FindActionMap("UIPlayer", throwIfNotFound: true);
        m_UIPlayer_ToggleScoreboard = m_UIPlayer.FindAction("ToggleScoreboard", throwIfNotFound: true);
        m_UIPlayer_UILeftClick = m_UIPlayer.FindAction("UILeftClick", throwIfNotFound: true);
        m_UIPlayer_UIRightClick = m_UIPlayer.FindAction("UIRightClick", throwIfNotFound: true);
        m_UIPlayer_ToggleBuyMenu = m_UIPlayer.FindAction("ToggleBuyMenu", throwIfNotFound: true);
        // UIGlobal
        m_UIGlobal = asset.FindActionMap("UIGlobal", throwIfNotFound: true);
        m_UIGlobal_ToggleTeamSelector = m_UIGlobal.FindAction("ToggleTeamSelector", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // UIPlayer
    private readonly InputActionMap m_UIPlayer;
    private List<IUIPlayerActions> m_UIPlayerActionsCallbackInterfaces = new List<IUIPlayerActions>();
    private readonly InputAction m_UIPlayer_ToggleScoreboard;
    private readonly InputAction m_UIPlayer_UILeftClick;
    private readonly InputAction m_UIPlayer_UIRightClick;
    private readonly InputAction m_UIPlayer_ToggleBuyMenu;
    public struct UIPlayerActions
    {
        private @UI m_Wrapper;
        public UIPlayerActions(@UI wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleScoreboard => m_Wrapper.m_UIPlayer_ToggleScoreboard;
        public InputAction @UILeftClick => m_Wrapper.m_UIPlayer_UILeftClick;
        public InputAction @UIRightClick => m_Wrapper.m_UIPlayer_UIRightClick;
        public InputAction @ToggleBuyMenu => m_Wrapper.m_UIPlayer_ToggleBuyMenu;
        public InputActionMap Get() { return m_Wrapper.m_UIPlayer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIPlayerActions set) { return set.Get(); }
        public void AddCallbacks(IUIPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_UIPlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIPlayerActionsCallbackInterfaces.Add(instance);
            @ToggleScoreboard.started += instance.OnToggleScoreboard;
            @ToggleScoreboard.performed += instance.OnToggleScoreboard;
            @ToggleScoreboard.canceled += instance.OnToggleScoreboard;
            @UILeftClick.started += instance.OnUILeftClick;
            @UILeftClick.performed += instance.OnUILeftClick;
            @UILeftClick.canceled += instance.OnUILeftClick;
            @UIRightClick.started += instance.OnUIRightClick;
            @UIRightClick.performed += instance.OnUIRightClick;
            @UIRightClick.canceled += instance.OnUIRightClick;
            @ToggleBuyMenu.started += instance.OnToggleBuyMenu;
            @ToggleBuyMenu.performed += instance.OnToggleBuyMenu;
            @ToggleBuyMenu.canceled += instance.OnToggleBuyMenu;
        }

        private void UnregisterCallbacks(IUIPlayerActions instance)
        {
            @ToggleScoreboard.started -= instance.OnToggleScoreboard;
            @ToggleScoreboard.performed -= instance.OnToggleScoreboard;
            @ToggleScoreboard.canceled -= instance.OnToggleScoreboard;
            @UILeftClick.started -= instance.OnUILeftClick;
            @UILeftClick.performed -= instance.OnUILeftClick;
            @UILeftClick.canceled -= instance.OnUILeftClick;
            @UIRightClick.started -= instance.OnUIRightClick;
            @UIRightClick.performed -= instance.OnUIRightClick;
            @UIRightClick.canceled -= instance.OnUIRightClick;
            @ToggleBuyMenu.started -= instance.OnToggleBuyMenu;
            @ToggleBuyMenu.performed -= instance.OnToggleBuyMenu;
            @ToggleBuyMenu.canceled -= instance.OnToggleBuyMenu;
        }

        public void RemoveCallbacks(IUIPlayerActions instance)
        {
            if (m_Wrapper.m_UIPlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_UIPlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIPlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIPlayerActions @UIPlayer => new UIPlayerActions(this);

    // UIGlobal
    private readonly InputActionMap m_UIGlobal;
    private List<IUIGlobalActions> m_UIGlobalActionsCallbackInterfaces = new List<IUIGlobalActions>();
    private readonly InputAction m_UIGlobal_ToggleTeamSelector;
    public struct UIGlobalActions
    {
        private @UI m_Wrapper;
        public UIGlobalActions(@UI wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleTeamSelector => m_Wrapper.m_UIGlobal_ToggleTeamSelector;
        public InputActionMap Get() { return m_Wrapper.m_UIGlobal; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIGlobalActions set) { return set.Get(); }
        public void AddCallbacks(IUIGlobalActions instance)
        {
            if (instance == null || m_Wrapper.m_UIGlobalActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIGlobalActionsCallbackInterfaces.Add(instance);
            @ToggleTeamSelector.started += instance.OnToggleTeamSelector;
            @ToggleTeamSelector.performed += instance.OnToggleTeamSelector;
            @ToggleTeamSelector.canceled += instance.OnToggleTeamSelector;
        }

        private void UnregisterCallbacks(IUIGlobalActions instance)
        {
            @ToggleTeamSelector.started -= instance.OnToggleTeamSelector;
            @ToggleTeamSelector.performed -= instance.OnToggleTeamSelector;
            @ToggleTeamSelector.canceled -= instance.OnToggleTeamSelector;
        }

        public void RemoveCallbacks(IUIGlobalActions instance)
        {
            if (m_Wrapper.m_UIGlobalActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIGlobalActions instance)
        {
            foreach (var item in m_Wrapper.m_UIGlobalActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIGlobalActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIGlobalActions @UIGlobal => new UIGlobalActions(this);
    public interface IUIPlayerActions
    {
        void OnToggleScoreboard(InputAction.CallbackContext context);
        void OnUILeftClick(InputAction.CallbackContext context);
        void OnUIRightClick(InputAction.CallbackContext context);
        void OnToggleBuyMenu(InputAction.CallbackContext context);
    }
    public interface IUIGlobalActions
    {
        void OnToggleTeamSelector(InputAction.CallbackContext context);
    }
}
