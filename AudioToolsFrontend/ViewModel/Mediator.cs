namespace AudioToolsFrontend.ViewModel
{
    public class Mediator 
    {
        private readonly Dictionary<Type, List<Action<object>>> _registeredViewModels = new Dictionary<Type, List<Action<object>>>();

        //Any viewmodel which wants to receive or send data must first Register themselves with this method
        public void Register<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!_registeredViewModels.ContainsKey(type))
                _registeredViewModels[type] = new List<Action<object>>();
            _registeredViewModels[type].Add(o => callback((T)o));
        }
        public void Send<T>(T message)
        {
            var type = typeof(T);
            if (_registeredViewModels.ContainsKey(type))
                _registeredViewModels[type].ForEach(v => v(message));
        }
    }
}
