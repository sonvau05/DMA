using Microsoft.AspNetCore.Mvc;
using UserAuthApp.Models;
using UserAuthApp.Services;

namespace UserAuthApp.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly UserService _userService;
        private readonly SmsService _smsService;
        private static string verificationCode;
        private static string pendingUser;

        public AuthController()
        {
            _userService = new UserService();
            _smsService = new SmsService("TWILIO_ACCOUNT_SID", "TWILIO_AUTH_TOKEN", "+1234567890");
        }


        [HttpGet("Login")]
        public IActionResult Login() => View();

        [HttpPost("Login")]
        public IActionResult Login(string username, string password)
        {
            var user = _userService.GetUser(username);
            if (user != null && user.Password == password && user.Verified)
                return RedirectToAction("Dashboard");

            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu, hoặc tài khoản chưa xác thực.";
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register() => View();

        [HttpPost("Register")]
        public IActionResult Register(string username, string password, string phone)
        {
            var code = new Random().Next(100000, 999999).ToString();
            verificationCode = code;
            pendingUser = username;
            _smsService.SendVerificationCode(phone, code);
            TempData["Username"] = username;
            TempData["Password"] = password;
            TempData["Phone"] = phone;
            return RedirectToAction("Verify");
        }

        [HttpGet("Verify")]
        public IActionResult Verify() => View();

        [HttpPost("Verify")]
        public IActionResult Verify(string code)
        {
            if (code == verificationCode)
            {
                var user = new UserModel
                {
                    Username = TempData["Username"].ToString(),
                    Password = TempData["Password"].ToString(),
                    Phone = TempData["Phone"].ToString(),
                    Verified = true
                };

                _userService.AddUser(user);
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Sai mã xác thực.";
            return View();
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard() => Content("Chào mừng, bạn đã đăng nhập thành công!");


        [HttpGet("api/users")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Json(users);
        }

        [HttpGet("api/users/{username}")]
        public IActionResult GetUser(string username)
        {
            var user = _userService.GetUser(username);
            if (user == null) return NotFound();
            return Json(user);
        }

        [HttpPost("api/users")]
        public IActionResult AddUser([FromBody] UserModel user)
        {
            _userService.AddUser(user);
            return Ok(user);
        }

        [HttpPut("api/users/{username}")]
        public IActionResult UpdateUser(string username, [FromBody] UserModel updated)
        {
            var success = _userService.UpdateUser(username, updated);
            if (!success) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("api/users/{username}")]
        public IActionResult DeleteUser(string username)
        {
            var success = _userService.DeleteUser(username);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
