using Microsoft.AspNetCore.Mvc;
using BL;
using Entities;
using System.Threading.Tasks;

namespace AspNetCoreUrunSitesi.ViewComponents
{
    public class Categories : ViewComponent
    {
        private readonly IRepository<Category> _categoryRepository;

        public Categories(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _categoryRepository.GetAllAsync());
        }
    }
}
