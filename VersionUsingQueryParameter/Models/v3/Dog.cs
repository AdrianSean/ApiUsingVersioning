namespace VersionUsingQueryParameter.Models.v3
{
    internal class Dog
    {
        internal int Id { get; set; }
        internal string Name { get; set; }
        internal string Colour { get; set; }

        public override string ToString()
        {
            return "Dog v3";
        }
    }
}
