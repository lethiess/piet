namespace Piet.Interpreter.Events
{
    public class OutputService : IOutputService
    {
        public event EventHandler<OutputCharacterOperationEventArgs>? OutputCharacter;
        public event EventHandler<OutputIntegerOperationEventArgs>? OutputInteger;
        
        public void DispatchOutputCharacterEvent(char value)
        {
            OnOutputCharacterOperation(new OutputCharacterOperationEventArgs(value));
        }

        public void DispatchOutputIntegerEvent(int value)
        {
            OnOutputIntegerOperation(new OutputIntegerOperationEventArgs(value));
        }

        protected virtual void OnOutputCharacterOperation(OutputCharacterOperationEventArgs e)
        {
            var handler = OutputCharacter;
            handler?.Invoke(this, e);
        }
        
        protected virtual void OnOutputIntegerOperation(OutputIntegerOperationEventArgs e)
        {
            var handler = OutputInteger;
            handler?.Invoke(this, e);
        }
    }
}
