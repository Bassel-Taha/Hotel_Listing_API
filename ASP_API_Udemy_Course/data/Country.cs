namespace ASP_API_Udemy_Course.data
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public virtual IList<Hotel> Hotels { get; set; }
    }
}