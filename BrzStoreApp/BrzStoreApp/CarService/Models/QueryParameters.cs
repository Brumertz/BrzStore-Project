namespace CarService.Models
{
    public class QueryParameters
    {
        const int _maxSize = 100;
        private int _size = 50;

        public int Page { get; set; } = 1;

        public int Size
        {
            get { return _size; }
            set
            {
                _size = Math.Min(_maxSize, value);
            }
        }
        public string sortBy { get; set; } = "Id";

        private string sortOrder = "asc";
        public string SortOrder
        {
            get
            {
                return sortOrder;

            }
            set
            {
                if (value == "asc" || value == "desc")
                {
                    sortOrder = value;
                }
            }
        }
    }

}
