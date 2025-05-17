using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace Attendace.Helper
{
	public class QRCodeHelper
	{
		public static string GenerateQrSvg(string url)
		{
			var qrGenerator = new QRCodeGenerator();
			var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
			var qrCode = new SvgQRCode(qrCodeData);
			return qrCode.GetGraphic(10); // 10 = pixels per module
		}
	}
}
