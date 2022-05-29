// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Global"",
            ""id"": ""0e658846-b5b5-421d-bf4a-dd5de85741bd"",
            ""actions"": [
                {
                    ""name"": ""Mouse Position"",
                    ""type"": ""Value"",
                    ""id"": ""4b5b5e3f-455c-4070-8980-d7e716fd4386"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Step"",
                    ""type"": ""Button"",
                    ""id"": ""c42b3aff-f5ac-413a-9cf3-c1e17c2b544f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f3c7b5bb-551c-4f40-ae10-6fdb58f7a145"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dbd0ee80-16b2-40ec-9db3-58922b363065"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Step"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Global
        m_Global = asset.FindActionMap("Global", throwIfNotFound: true);
        m_Global_MousePosition = m_Global.FindAction("Mouse Position", throwIfNotFound: true);
        m_Global_Step = m_Global.FindAction("Step", throwIfNotFound: true);
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

    // Global
    private readonly InputActionMap m_Global;
    private IGlobalActions m_GlobalActionsCallbackInterface;
    private readonly InputAction m_Global_MousePosition;
    private readonly InputAction m_Global_Step;
    public struct GlobalActions
    {
        private @Controls m_Wrapper;
        public GlobalActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePosition => m_Wrapper.m_Global_MousePosition;
        public InputAction @Step => m_Wrapper.m_Global_Step;
        public InputActionMap Get() { return m_Wrapper.m_Global; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GlobalActions set) { return set.Get(); }
        public void SetCallbacks(IGlobalActions instance)
        {
            if (m_Wrapper.m_GlobalActionsCallbackInterface != null)
            {
                @MousePosition.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnMousePosition;
                @Step.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnStep;
                @Step.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnStep;
                @Step.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnStep;
            }
            m_Wrapper.m_GlobalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @Step.started += instance.OnStep;
                @Step.performed += instance.OnStep;
                @Step.canceled += instance.OnStep;
            }
        }
    }
    public GlobalActions @Global => new GlobalActions(this);
    public interface IGlobalActions
    {
        void OnMousePosition(InputAction.CallbackContext context);
        void OnStep(InputAction.CallbackContext context);
    }
}
