using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spis_Leków.Models;
using System.Security.Claims;
using System;

namespace Spis_Leków.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        

        public string Ranga { get; set; }

        
        public void OnGet()
        {

            Ranga = User.FindFirstValue("Ranga");

          
        }
    }
}