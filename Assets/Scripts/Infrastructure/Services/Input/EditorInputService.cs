using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class EditorInputService : InputService
    {
        public override Vector2 Axis
        {
            get
            {
                var axis = GetSimpleInputAxis();

                if (axis == Vector2.zero) axis = UnityAxis();

                return axis;
            }
        }

        private static Vector2 UnityAxis()
        {
            return new Vector2(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));
        }
    }
}