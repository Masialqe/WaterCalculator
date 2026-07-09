namespace WaterCalculator.Components.Shared.Utilities
{
    public class ComponentState
    {
        public ComponentStateType CurrentState { get; private set; } = ComponentStateType.Empty;
        public string ErrorMessage { get; private set; } = string.Empty;

        public void SetLoading()
        {
            CurrentState = ComponentStateType.Loading;
            ErrorMessage = string.Empty;
        }

        public void EndLoading()
        {
            CurrentState = ComponentStateType.Empty;
            ErrorMessage = string.Empty;
        }

        public void SetSuccess()
        {
            CurrentState = ComponentStateType.Success;
            ErrorMessage = string.Empty;
        }

        public void SetError(string errorMessage)
        {
            CurrentState = ComponentStateType.Error;
            ErrorMessage = errorMessage;
        }

        public void SetEmpty()
        {
            CurrentState = ComponentStateType.Empty;
            ErrorMessage = string.Empty;
        }

        public void SetStatusByCollection<T>(ICollection<T> collection)
        {
            CurrentState =
                collection.Count == 0
                    ? ComponentStateType.Empty
                    : ComponentStateType.Success;
        }

        public void SetStatusByState<T>(T? data)
        {
            CurrentState =
                data is null
                    ? ComponentStateType.Empty
                    : ComponentStateType.Success;
        }

        public bool IsLoading => CurrentState == ComponentStateType.Loading;
        public bool IsSuccess => CurrentState == ComponentStateType.Success;
        public bool IsError => CurrentState == ComponentStateType.Error;
        public bool IsEmpty => CurrentState == ComponentStateType.Empty;

    }
}
