using System;

namespace MenuGen.Ioc
{
    public class DependencyMap<TAbstractType>
    {
        private readonly IContainer _container;
        private readonly Binding _binding;

        public DependencyMap(IContainer container)
        {
            _container = container;

            _binding = new Binding
            {
                AbstractType = typeof(TAbstractType),
                ConcreteType = ConcreteType
            };
        }

        public Type ConcreteType { get; private set; }

        public Type AbstractType
        {
            get
            {
                return _binding.AbstractType;
            }
        }

        public string Name { get; set; }

        public object Instance { get; set; }

        public DependencyOptions Use<TConcreteType>() where TConcreteType : TAbstractType
        {
            _binding.ConcreteType = typeof(TConcreteType);

            _container.AddBinding(_binding);

            return new DependencyOptions(_binding);
        }

        public DependencyOptions Use<TConcreteType>(TConcreteType type) where TConcreteType : TAbstractType
        {
            _binding.ConcreteType = typeof(TConcreteType);
            _binding.Instance = type;

            _container.AddBinding(_binding);

            return new DependencyOptions(_binding);
        }
    }
}
