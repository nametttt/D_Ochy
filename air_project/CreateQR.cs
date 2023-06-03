using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace air_project.pages
{
    public class CreateQR
    {

        public string GenerateQRCode(int idFlight, string place)
        {
            string qrCodeResult = "Рейс №" + idFlight + " Место " + place;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeResult, QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(10, Color.Black, Color.White, true);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                qrCodeImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);

                byte[] buffer = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(buffer);

                return base64String;
            }
        }
    }
}
