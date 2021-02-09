#if MLA_INPUT_SYSTEM
using Unity.MLAgents.Actuators;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Unity.MLAgents.Extensions.Runtime.Input
{
    /// <summary>
    /// Translates data from a <see cref="UnityEngine.InputSystem.Controls.DoubleControl"/>.
    /// </summary>
    public class DoubleInputActionAdaptor : IRLActionInputAdaptor
    {
        /// <inheritdoc cref="IRLActionInputAdaptor.GetActionSpecForInputAction"/>
        public ActionSpec GetActionSpecForInputAction(InputAction action)
        {
            return ActionSpec.MakeContinuous(1);
        }

        /// <inheritdoc cref="IRLActionInputAdaptor.QueueInputEventForAction"/>
        public void QueueInputEventForAction(InputAction action, InputControl control, ActionSpec actionSpec, in ActionBuffers actionBuffers)
        {
            var val = actionBuffers.ContinuousActions[0];

            using (StateEvent.From(control.device, out var eventPtr))
            {
                control.WriteValueIntoEvent((double)val, eventPtr);
                InputSystem.QueueEvent(eventPtr);
            }
        }

        /// <inheritdoc cref="IRLActionInputAdaptor.WriteToHeuristic"/>
        public void WriteToHeuristic(InputAction action, in ActionBuffers actionBuffers)
        {
            var actions = actionBuffers.ContinuousActions;
            var val = (float)action.ReadValue<double>();
            actions[0] = val;
        }
    }
}
#endif // MLA_INPUT_SYSTEM
