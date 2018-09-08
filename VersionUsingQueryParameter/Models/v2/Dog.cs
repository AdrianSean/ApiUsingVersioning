namespace VersionUsingQueryParameter.Models.v2
{
    internal class Dog
    {
        internal int Id { get; set; }
        internal string Name { get; set; }

        public override string ToString()
        {
            return "Dog v2";
        }
    }
}
