using UnityEngine;

namespace SupanthaPaul
{
    public class InputSystem : MonoBehaviour
    {
        // Кэширование строк ввода
        static readonly string HorizontalInput = "Horizontal";
        static readonly string JumpInput = "Jump";
        static readonly string CrouchInput = "Crouch";  // Новый ввод для приседания
        static readonly string RunInput = "Run";          // Новый ввод для бега

        public static float HorizontalRaw()
        {
            return Input.GetAxisRaw(HorizontalInput);
        }

        public static bool Jump()
        {
            return Input.GetButtonDown(JumpInput);
        }

        // Новый метод для приседания
        public static bool Crouch()
        {
            return Input.GetButton(CrouchInput);
        }

        // Новый метод для бега
        public static bool Run()
        {
            return Input.GetButton(RunInput);
        }
    }
}
