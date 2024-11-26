using DevHelper.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Controladoras
{
    public class PgProblemaController : Controller
    {
        private readonly DBdevhelperContext _context;

        public PgProblemaController(DBdevhelperContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Like(int id)
        {
            var problema = await _context.Problemas.FindAsync(id);
            if (problema == null)
            {
                return NotFound();
            }
            problema.LikeCount++;
            await _context.SaveChangesAsync();
            return Json(new { success = true, likeCount = problema.LikeCount });
        }

        [HttpPost]
        public async Task<IActionResult> Dislike(int id)
        {
            var problema = await _context.Problemas.FindAsync(id);
            if (problema == null)
            {
                return NotFound();
            }
            problema.DislikeCount++;
            await _context.SaveChangesAsync();
            return Json(new { success = true, dislikeCount = problema.DislikeCount });
        }
    }


}
