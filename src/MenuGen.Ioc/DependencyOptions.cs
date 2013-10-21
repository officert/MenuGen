
namespace MenuGen.Ioc
{
    public class DependencyOptions : IDependencyOptions
    {
        private readonly IBinding _binding;

        public DependencyOptions(IBinding binding)
        {
            _binding = binding;
        }

        public DependencyOptions Named(string name)
        {
            _binding.Name = name;
            return new DependencyOptions(_binding);
        }

        public void InSingletonScope()
        {
            throw new System.NotImplementedException();
        }

        public void InRequestScope()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IDependencyOptions
    {
        DependencyOptions Named(string name);
        void InSingletonScope();
        void InRequestScope();
    }
}
