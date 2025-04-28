
namespace BrzSaleApi.Models
{
    public class QueryParameters
    {
        private const int _maxSize = 1000;
        private int _size = 50;

        /// <summary>
        /// Page number (default is 1)
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Page size (default is 50, maximum 1000)
        /// </summary>
        public int Size
        {
            get => _size;
            set => _size = Math.Min(_maxSize, value);
        }

        /// <summary>
        /// Field to sort by (default is Id)
        /// </summary>
        public string SortBy { get; set; } = "Id";

        private string _sortOrder = "asc";

        /// <summary>
        /// Sort order (asc or desc)
        /// </summary>
        public string SortOrder
        {
            get => _sortOrder;
            set
            {
                if (value.ToLower() == "asc" || value.ToLower() == "desc")
                {
                    _sortOrder = value.ToLower();
                }
                else
                {
                    _sortOrder = "asc"; // default to asc if invalid
                }
            }
        }
        public bool? OnSale { get; set; } // NEW! Optional OnSale filter
    }
}
