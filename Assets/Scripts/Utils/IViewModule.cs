namespace Utils
{
    public interface IViewModule<in T> where T : class
    {
        public void ExecuteView(T data);
    }
}
