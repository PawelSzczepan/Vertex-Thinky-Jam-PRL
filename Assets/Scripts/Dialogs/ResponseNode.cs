using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogs
{
    public abstract class ResponseNode : NoChoiceDialogNode
    {
        private enum State { None, Executing, Finished }

        private State _state;

        public sealed override ExecuteResult Execute(IDialogRuntime runtime)
        {
            switch(_state)
            {
                case State.None:
                    _state = State.Executing;
                    runtime.onWorkDone += OnRuntimeWorkDone;
                    RequestDisplayingResponse(runtime);
                    return ExecuteResult.InProgress;

                case State.Executing:
                    return ExecuteResult.InProgress;

                case State.Finished:
                    _state = State.None;
                    runtime.onWorkDone -= OnRuntimeWorkDone;
                    return ExecuteResult.Finished;
            }

            throw new NotImplementedException();
        }

        protected abstract void RequestDisplayingResponse(IDialogRuntime runtime);

        private void OnRuntimeWorkDone()
        {
            _state = State.Finished;
        }
    }
}
