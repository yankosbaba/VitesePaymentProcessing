using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Vitese_Payment_Processing.Model
{
    public class PaymentRequest
    {
        [JsonPropertyName("paymentId")]
        public string PaymentId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}
