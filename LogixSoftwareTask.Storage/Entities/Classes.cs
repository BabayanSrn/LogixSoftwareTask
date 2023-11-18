namespace LogixSoftwareTask.Storage.Entities
{
    public class Classes 
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
