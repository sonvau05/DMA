using Microsoft.AspNetCore.Mvc;
using NewsApi.Models;
using Serilog;

namespace NewsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private static List<News> newsList = new List<News>
        {
            new News { Id = 1, Title = "Tin tức đầu tiên", Content = "Đây là nội dung của bài viết đầu tiên", CreatedAt = DateTime.Now },
            new News { Id = 2, Title = "Công nghệ hôm nay", Content = "Cập nhật tin tức công nghệ mới nhất", CreatedAt = DateTime.Now }
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            Log.Information("User lấy danh sách tin tức");
            return Ok(newsList);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = newsList.FirstOrDefault(n => n.Id == id);
            if (item == null)
            {
                Log.Warning("Không tìm thấy tin có Id {Id}", id);
                return NotFound();
            }
            Log.Information("User xem chi tiết tin có Id {Id}", id);
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create(News news)
        {
            news.Id = newsList.Max(n => n.Id) + 1;
            news.CreatedAt = DateTime.Now;
            newsList.Add(news);
            Log.Information("User tạo mới tin: {Title}", news.Title);
            return Ok(news);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, News updatedNews)
        {
            var item = newsList.FirstOrDefault(n => n.Id == id);
            if (item == null)
            {
                Log.Warning("Không thể cập nhật. Không tìm thấy tin có Id {Id}", id);
                return NotFound();
            }

            item.Title = updatedNews.Title;
            item.Content = updatedNews.Content;
            Log.Information("User cập nhật tin có Id {Id}", id);
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = newsList.FirstOrDefault(n => n.Id == id);
            if (item == null)
            {
                Log.Warning("Không thể xóa. Không tìm thấy tin có Id {Id}", id);
                return NotFound();
            }

            newsList.Remove(item);
            Log.Information("User xóa tin có Id {Id}", id);
            return Ok();
        }
    }
}
