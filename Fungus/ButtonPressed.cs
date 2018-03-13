using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Button press modes supported by Button Pressed event handler.
    /// </summary>
    public enum ButtonPressType
    {
        /// <summary> Execute once when the button is pressed down. </summary>
        ButtonDown,
        /// <summary> Execute once when the button is released </summary>
        //ButtonUp,
        /// <summary> Execute once per frame when button is held down. </summary>
        ButtonRepeat
    }

    /// <summary>
    /// The block will execute when a button press event occurs.
    /// </summary>
    [EventHandlerInfo("Input",
                      "Button Pressed",
                      "The block will execute when a button press event occurs.")]
    [AddComponentMenu("")]
    public class ButtonPressed : EventHandler
    {   
        [Tooltip("The type of buttonpress to activate on")]
        [SerializeField] protected ButtonPressType buttonPressType;

        [Tooltip("Device of the button to activate on, base 0")]
        [SerializeField] protected int deviceNumber;

        [Tooltip("Index of the button to activate on, base 0")]
        [SerializeField]
        protected int buttonNumber;

        protected virtual void Update()
        {
            switch (buttonPressType)
            {
            case ButtonPressType.ButtonDown:
                if (SerialController.Instance.WasPressed(deviceNumber, buttonNumber))
                {
                    ExecuteBlock();
                }
                break;
            /*
            case ButtonPressType.ButtonUp:
                if (SerialController.Instance.IsPressed(deviceNumber, buttonNumber))
                {
                    ExecuteBlock();
                }
                break;
            */
            case ButtonPressType.ButtonRepeat:
                if (SerialController.Instance.IsPressed(deviceNumber, buttonNumber))
                {
                    ExecuteBlock();
                }
                break;
            }
        }

        #region Public members

        public override string GetSummary()
        {
            return "Device: " + deviceNumber.ToString() + ", Button: " + buttonNumber.ToString();
        }

        #endregion
    }
}