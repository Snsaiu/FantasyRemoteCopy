#if WINDOWS
using Microsoft.UI.Xaml.Media.Imaging;

using Windows.Storage.Streams;

#elif MACCATALYST
#elif ANDROID
#else
#endif

namespace Fantasy.UIKit;

public partial class Avatar
{
    public override void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.GetClipPath(dirtyRect));
        if (Source is null)
        {
            canvas.DrawBackground(this, dirtyRect);
        }
        else
        {
#if WINDOWS


            IImageSourceServiceProvider imageSourceServiceProvider =
                Handler.MauiContext.Services.GetRequiredService<IImageSourceServiceProvider>();

            IImageSourceService? imageSourceService = imageSourceServiceProvider.GetImageSourceService(Source);


            BitmapImage? bitmapImage =
                imageSourceService.GetImageSourceAsync(Source).GetAwaiter().GetResult().Value as BitmapImage;

            IRandomAccessStreamReference streamReference =
                RandomAccessStreamReference.CreateFromUri(bitmapImage.UriSource);

            using Stream stream = streamReference.OpenReadAsync().GetAwaiter().GetResult().AsStreamForRead();
            //image = PlatformImage.FromStream(stream);
#endif


            //  canvas.DrawImage(image, dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);
        }

        //if (Enable == false)
        //{
        //    canvas.FillColor =
        //        Colors.White.MultiplyAlpha(0.5f);
        //    canvas.FillRectangle(dirtyRect);
        //}


        canvas.DrawBorderColor(this, dirtyRect);
        canvas.ResetState();
    }
}