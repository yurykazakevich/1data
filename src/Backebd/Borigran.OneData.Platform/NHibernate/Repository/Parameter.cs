using NHibernate.Type;

namespace Borigran.OneData.Platform.NHibernate.Repository
{
    /// <summary>
    /// Provides state container for passing database parameters using nHibernate specific types,
    /// see <see cref="IType"/>.
    /// </summary>
    public class Parameter
    {
        string name;
        object value;
        IType type;

        /// <summary>
        /// Type of the parameter as an nhibernate type
        /// </summary>
        public IType Type
        {
            get { return type; }
        }
        /// <summary>
        /// The name of the parameter
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// The values of the parameter
        /// </summary>
        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ValueIsIEnumerable { get; set; }

        /// <summary>
        /// Minimal constructor accepting the name and value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public Parameter(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
        /// <summary>
        /// Constructor accepts name, value and type
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <param name="type">Parameter type</param>
        public Parameter(string name, object value, IType type)
            : this(name, value)
        {
            this.type = type;
        }
    }
}
