using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace UserAuthApp.Services
{
    public class SmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhone;

        public SmsService(string accountSid, string authToken, string fromPhone)
        {
            _accountSid = accountSid;
            _authToken = authToken;
            _fromPhone = fromPhone;
        }

        public void SendVerificationCode(string toPhone, string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_accountSid) || _accountSid.Contains("TWILIO"))
                {
                    Console.WriteLine($"[SMS MOCK] Gửi mã '{code}' đến số {toPhone}");
                    return;
                }

                TwilioClient.Init(_accountSid, _authToken);

                MessageResource.Create(
                    to: new PhoneNumber(toPhone),
                    from: new PhoneNumber(_fromPhone),
                    body: $"Mã xác thực của bạn là: {code}"
                );

                Console.WriteLine($"Đã gửi mã xác thực tới {toPhone}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi SMS: {ex.Message}");
            }
        }
    }
}
