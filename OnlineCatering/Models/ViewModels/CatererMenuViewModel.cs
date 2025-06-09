using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineCatering.Models.ViewModels
{
    public class CatererMenuViewModel
    {
        public int CatererId { get; set; }
        public string CatererName { get; set; }

        // filter inputs
        public int? SelectedCategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // for dropdown
        public List<SelectListItem> Categories { get; set; }

        // the filtered menu items
        public List<MenuItemViewModel> MenuItems { get; set; }
    }

}
